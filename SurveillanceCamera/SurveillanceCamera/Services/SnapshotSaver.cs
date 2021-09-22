using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using LibVLCSharp.Shared;
using SkiaSharp;


namespace SurveillanceCamera.Services{

    public class SnapshotSaver
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

        private LibVLC _libvlc;
        /// <summary>
        /// RGBA is used, so 4 byte per pixel, or 32 bits.
        /// </summary>
        private const uint BytePerPixel = 4;

        /// <summary>
        /// the number of bytes per "line"
        /// For performance reasons inside the core of VLC, it must be aligned to multiples of 32.
        /// </summary>
        private  readonly uint Pitch;

        /// <summary>
        /// The number of lines in the buffer.
        /// For performance reasons inside the core of VLC, it must be aligned to multiples of 32.
        /// </summary>
        private  readonly uint Lines;

        public SnapshotSaver(uint w, uint h)
        {

            _width = w;
            _height = h;
            
            Pitch = Align(_width * BytePerPixel);
            Lines = Align(_height);

            _libvlc = new LibVLC();
            
            uint Align(uint size)
            {
                if (size % 32 == 0)
                {
                    return size;
                }

                return ((size / 32) + 1) * 32;// Align on the next multiple of 32
            }
        }

        private  SKBitmap CurrentBitmap;
        private  readonly ConcurrentQueue<SKBitmap> FilesToProcess = new ConcurrentQueue<SKBitmap>();
        private  long FrameCounter = 0;
        
        public async Task SaveFrame(string url, string destination)
        {
            // Extract thumbnails in the "preview" folder next to the app
            // var currentDirectory = "/storage/emulated/0/Download/";
            // var destination = Path.Combine(currentDirectory, "preview1");
            Directory.CreateDirectory(destination);

            // Load native libvlc library
            Core.Initialize();

            
            using (var mediaPlayer = new MediaPlayer(_libvlc))
            {


                _player = mediaPlayer;
                
                // Listen to events
                var processingCancellationTokenSource = new CancellationTokenSource();
                mediaPlayer.Stopped += (s, e) => processingCancellationTokenSource.CancelAfter(1);

                // Create new media
                var media = new Media(_libvlc, new Uri(url));

                media.AddOption(":no-audio");
                // Set the size and format of the video here.
                mediaPlayer.SetVideoFormat("RV32", _width, _height, Pitch);
                mediaPlayer.SetVideoCallbacks(Lock, null, Display);

                // Start recording
                mediaPlayer.Play(media);
                
                

                // Waits for the processing to stop
                try
                {
                    await ProcessThumbnailsAsync(destination, processingCancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                { }

                
            }
        }

        private async Task ProcessThumbnailsAsync(string destination, CancellationToken token)
        {
            var frameNumber = 0;
            var surface = SKSurface.Create(new SKImageInfo((int) _width, (int) _height));
            var canvas = surface.Canvas;
            while (!token.IsCancellationRequested)
            {
                if (FilesToProcess.TryDequeue(out var bitmap))
                {
                    canvas.DrawBitmap(bitmap, 0, 0); // Effectively crops the original bitmap to get only the visible area

                    // var name = DateTime.Now.ToString("yyyy_MM_dd_T_HH_mm_ss");
                    
                    // Console.WriteLine($"Writing {name}.jpg");
                    var fileName = Path.Combine(destination, $"snapshot.jpg");
                    using (var outputImage = surface.Snapshot())
                    using (var data = outputImage.Encode(SKEncodedImageFormat.Jpeg, 50))
                    using (var outputFile = File.Open(fileName, FileMode.Create))
                    {
                        data.SaveTo(outputFile);
                        bitmap.Dispose(); 
                        IsFrameSaved = true;
                    }

                    frameNumber++;

                   
                }
                else
                {
                    await Task.Delay(TimeSpan.Zero, token);
                }
            }
        }

        private  IntPtr Lock(IntPtr opaque, IntPtr planes)
        {
            CurrentBitmap = new SKBitmap(new SKImageInfo((int)(Pitch / BytePerPixel), (int)Lines, SKColorType.Bgra8888));
            Marshal.WriteIntPtr(planes, CurrentBitmap.GetPixels());
            return IntPtr.Zero;
        }

        private void Display(IntPtr opaque, IntPtr picture)
        {
            if (FrameCounter % 100 == 0)
            {
                FilesToProcess.Enqueue(CurrentBitmap);
                CurrentBitmap = null;
            }
            else
            {
                CurrentBitmap.Dispose();
                CurrentBitmap = null;
            }
            FrameCounter++;
       
        }
    }
}
