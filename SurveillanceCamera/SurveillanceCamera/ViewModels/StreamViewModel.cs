using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SurveillanceCamera.Models;
using SurveillanceCamera.Services;
using SurveillanceCamera.Services.Serialization;
using Xamarin.Essentials;
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
            // LoadStreamModelList();
        }

        private void LoadStreamModelList()
        {
            Task.Run(async () =>
            {
                var destination = "/storage/emulated/0/Download/preview2/";

                var appSettings = AppSettingsLoader.AppSettings;
            
                var streamUrl = StreamService.GetStreamUrl("2410f79c-8f7e-4cd4-8baf-f7be29869a7e");
            
                new SnapshotSaver(appSettings.Width, appSettings.Height).SaveFrame(streamUrl, destination);
                // var xmlResult = await new .GetStreamModel();
                // ChannelList = new CustomSerializationService().Deserialize(xmlResult);
            });
        }
    }
}