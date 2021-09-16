using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SurveillanceCamera.Models;
using SurveillanceCamera.Services;
using SurveillanceCamera.Services.Serialization;
using Xamarin.Forms;

namespace SurveillanceCamera.ViewModels
{
    public class ChannelInfoListViewModel : BaseViewModel
    {
        private ObservableCollection<ChannelInfo> _channelList;
        private ObservableCollection<ChannelInfo> _selectedChannels;

        public ObservableCollection<ChannelInfo> SelectedChannels
        {
            get => _selectedChannels;
            set
            {
                if (!Equals(_selectedChannels, value))
                {
                    _selectedChannels = value;
                }
            }
        }
        public ObservableCollection<ChannelInfo> ChannelList
        {
            get => _channelList;
            set
            {
                _channelList = value;
                OnPropertyChanged();
            }
        }

        public ChannelInfoListViewModel()
        {
            LoadChannelInfoList();
        }

        private void LoadChannelInfoList()
        {
            Task.Run(async () =>
            {
                var xmlResult = await new CamerasService().GetChannelInfo();
                ChannelList = new CustomSerializationService().Deserialize(xmlResult);
            });
        }
    }
}