using System.Collections.ObjectModel;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SurveillanceCamera.Models;
using SurveillanceCamera.Services;
using Xamarin.Forms;


namespace SurveillanceCamera.ViewModels
{
    public class StreamViewModel : BaseViewModel
    {

        private ObservableCollection<StreamModel> _streamList;

        public ObservableCollection<StreamModel> StreamList
        {
            get => _streamList;
            set
            {
                _streamList = value;
                OnPropertyChanged();
            }
        }

        private void LoadStreamModelList(string id)
        {
            var destination = $"/storage/emulated/0/Download/res/{id}/";

            var appSettings = AppSettingsLoader.AppSettings;
        
            var streamUrl = CamerasService.GetStreamUrl(id);
        
            new SnapshotSaver(appSettings.Width, appSettings.Height).SaveFrame(streamUrl, destination);
            
        }

        public void SelectedChannelEventHandler(ObservableCollection<object> chan, NotifyCollectionChangedEventArgs args)
        {

            var channels = chan.Select(ch => ((ChannelInfo) ch));
            
            foreach (var channelInfo in channels)
            {
                LoadStreamModelList(channelInfo.Id);
            }

            var temp = new ObservableCollection<StreamModel>();
            
            foreach (var channelInfo in channels)
            {
                var path = $"/storage/emulated/0/Download/res/{channelInfo.Id}/snapshot.jpg";
                
                while (!File.Exists(path))
                {
                    Console.WriteLine("DOESNT EXIST");
                    Task.Delay(100);
                }

                temp.Add(
                    new StreamModel()
                    {
                        Id = channelInfo.Id,
                        Image = ImageSource.FromFile(path)
                    });
            }

            StreamList = temp;
        }
    }
}