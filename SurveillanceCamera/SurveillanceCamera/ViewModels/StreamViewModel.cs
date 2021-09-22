using System.Collections.ObjectModel;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
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

        public StreamViewModel()
        {
           StreamList = new( new []
           {
               new StreamModel() {Id = "1", Image = GetImage("/storage/emulated/0/Download/preview2/snapshot.jpg")},
               new StreamModel() {Id = "2", Image = GetImage("/storage/emulated/0/Download/preview2/snapshot.jpg")},
               new StreamModel() {Id = "3", Image = GetImage("/storage/emulated/0/Download/preview2/snapshot.jpg")}
           });
        }

        private Xamarin.Forms.ImageSource GetImage(string path)
        {
            return ImageSource.FromFile(path);
        }
        private void LoadStreamModelList(string id)
        {
            Task.Run(async () =>
            {
                var destination = $"/storage/emulated/0/Download/preview2/{id}/";

                var appSettings = AppSettingsLoader.AppSettings;
            
                var streamUrl = StreamService.GetStreamUrl(id);
            
                new SnapshotSaver(640, 360).SaveFrame("http://demo.macroscop.com/mobile?channelid=2410f79c-8f7e-4cd4-8baf-f7be29869a7e&oneframeonly=false&login=root", "/storage/emulated/0/Download/preview2/1/");
            });
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

                var path = $"/storage/emulated/0/Download/preview2/1/snapshot.jpg";
                while (!File.Exists(path))
                {
                    Console.WriteLine("DOESNT EXISTS");
                    Task.Delay(100);
                }

                temp.Add(
                    new StreamModel()
                    {
                        Id = channelInfo.Id,
                        Image = ImageSource.FromFile(path)
                    });
            }
            // var streamModels=  channels1.Select( ch => new StreamModel{ Id = ch.Id, Image = ImageSource.FromFile("/storage/emulated/0/Download/preview2/snapshot.jpg")});

            // StreamList = new ObservableCollection<StreamModel>(streamModels);

            StreamList = temp;
        }
    }
}