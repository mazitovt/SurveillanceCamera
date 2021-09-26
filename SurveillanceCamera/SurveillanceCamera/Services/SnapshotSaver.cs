using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using LibVLCSharp.Shared;
using SkiaSharp;


namespace SurveillanceCamera.Services{

    public class SnapshotSaver : ISnapshotSaver
    {

        private bool _isFrameSaved = false;
        
        private MediaPlayer _player;
        
        private bool IsFrameSaved
        {
            get => _isFrameSaved;
            set
            {
                if (value)
                {
                    _isFrameSaved = true;
                    _player.Stop();
                }
            }
        }

        private uint _width;
        private uint _height;

        private static LibVLC _libvlc = new LibVLC();
            
        /// <summary>
        /// RGBA is used, so 4 byte per pixel, or 32 bits.
        /// </summary>
        private const uint BytePerPixel = 4;

        /// <summary>
        /// the number of bytes per "line"
        /// For performance reasons inside the core of VLC, it must be aligned to multiples of 32.
        /// </summary>
        private uint _pitch;

        /// <summary>
        /// The number of lines in the buffer.
        /// For performance reasons inside the core of VLC, it must be aligned to multiples of 32.
        /// </summary>
        private uint _lines;

        private SKBitmap _currentBitmap;
        private readonly ConcurrentQueue<SKBitmap> _filesToProcess = new ConcurrentQueue<SKBitmap>();
        private long _frameCounter = 0;


        private void Initialize(uint w, uint h)
        {
            _width = w;
            _height = h;
            
            _pitch = Align(_width * BytePerPixel);
            _lines = Align(_height);

            
            uint Align(uint size)
            {
                if (size % 32 == 0)
                {
                    return size;
                }

                return ((size / 32) + 1) * 32;// Align on the next multiple of 32
            }
        }
        public void SaveFrame(uint width, uint height, string url, string destination, string filePath)
        {
            Initialize(width, height);
            
            using var mediaPlayer = new MediaPlayer(_libvlc);
            _player = mediaPlayer;
                
            // Listen to events
            var processingCancellationTokenSource = new CancellationTokenSource();
            mediaPlayer.Stopped += (s, e) => processingCancellationTokenSource.CancelAfter(1);

            // Create new media
            var media = new Media(_libvlc, new Uri(url));

            media.AddOption(":no-audio");
            // Set the size and format of the video here.
            mediaPlayer.SetVideoFormat("RV32", _width, _height, _pitch);
            mediaPlayer.SetVideoCallbacks(Lock, null, Display);

            // Start recording
            mediaPlayer.Play(media);


            // Waits for the processing to stop
            try
            {
                ProcessThumbnails(filePath ,processingCancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ProcessThumbnails(string filePath, CancellationToken token)
        {
            var number = 0;
            var surface = SKSurface.Create(new SKImageInfo((int) _width, (int) _height));
            var canvas = surface.Canvas;
            while (!token.IsCancellationRequested)
            {
                if (_filesToProcess.TryDequeue(out var bitmap))
                {
                    if (IsFrameSaved) continue;
                    canvas.DrawBitmap(bitmap, 0, 0);
                    using (var outputImage = surface.Snapshot())
                    using (var data = outputImage.Encode(SKEncodedImageFormat.Jpeg, 50))
                    using (var outputFile = File.Open(filePath, FileMode.Create))
                    {
                        number++;
                        data.SaveTo(outputFile);
                        Console.WriteLine($"----------------------------{number}SAVED {outputFile}");
                        bitmap.Dispose(); 
                        IsFrameSaved = true;
                    }
                }
            }
        }

        private IntPtr Lock(IntPtr opaque, IntPtr planes)
        {
            _currentBitmap = new SKBitmap(new SKImageInfo((int)(_pitch / BytePerPixel), (int)_lines, SKColorType.Bgra8888));
            Marshal.WriteIntPtr(planes, _currentBitmap.GetPixels());
            return IntPtr.Zero;
        }

        private void Display(IntPtr opaque, IntPtr picture)
        {
            if (_frameCounter < 2)
            {
                _filesToProcess.Enqueue(_currentBitmap);
                _currentBitmap = null;
            }
            else
            {
                _currentBitmap.Dispose();
                _currentBitmap = null;
            }
            _frameCounter++;
        }
    }
}
