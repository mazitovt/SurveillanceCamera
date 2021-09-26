using System.Collections.ObjectModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SurveillanceCamera.Models;
using SurveillanceCamera.Services;
using Xamarin.Forms;


namespace SurveillanceCamera.ViewModels
{
    public class StreamViewModel : BaseViewModel
    {

        private ObservableCollection<StreamModel> _streamList;
        private bool _isRefreshing;

        public ICommand RefreshingCommand { get; }


        public ObservableCollection<StreamModel> StreamList
        {
            get => _streamList;
            set
            {
                _streamList = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            } }


        public StreamViewModel()
        {
            StreamList = new ObservableCollection<StreamModel>();
            
            RefreshingCommand = new Command(() =>
            {
                HandleSelectedChannels(StreamList.Select(stream => stream as ChannelInfo));
                IsRefreshing = false;
            });
        }
                
        private async Task<string> SaveFrame(string id)
        {
            var destination = $"/storage/emulated/0/Download/res/{id}/";
            Directory.CreateDirectory(destination);

            // var fileName = DateTime.Now.ToString("yyyy_MM_dd_T_HH_mm_ss_ms");
            var fileName = "snapshot";
            var filePath = Path.Combine(destination, $"{fileName}.jpg");

            var snapshotSaver = new SnapshotSaver();
            var appSettings = AppSettingsLoader.AppSettings;
            var streamUrl = CamerasService.GetStreamUrl(id);

            await Task.Run( () =>
                snapshotSaver.SaveFrame(appSettings.Width, appSettings.Height, streamUrl, destination, filePath));
            
            return filePath;
        }

        private StreamModel CreateStreamModel(ChannelInfo channelInfo, string filePath)
        {
            return new StreamModel
            {
                Id = channelInfo.Id,
                Image = ImageSource.FromFile(filePath),
                Name = channelInfo.Name
            };
        }

        private async Task UpdateFrame(ChannelInfo channelInfo, ConcurrentBag<StreamModel> updatedStreams)
        {
            var filePath = await SaveFrame(channelInfo.Id);
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine(filePath);
            var streamModel = CreateStreamModel(channelInfo, filePath);
            updatedStreams.Add(streamModel);
        }
        
        private async Task UpdateFrames(IEnumerable<ChannelInfo> channels)
        {
            try
            {
                var bag = new ConcurrentBag<StreamModel>();
                
                // var updatedStreams = new ObservableCollection<StreamModel>();

                var tasks = channels.Select(channel => UpdateFrame(channel, bag)).ToArray();
                
                Console.WriteLine("=====================================");
            
                await Task.WhenAll(tasks);

                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++");

                StreamList = new ObservableCollection<StreamModel>(bag.OrderBy(stream => stream.Name));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void HandleSelectedChannels(IEnumerable<ChannelInfo> channels)
        {
            Task.Run( async () =>
            {
                 await UpdateFrames(channels);
            });
        }
    }
}