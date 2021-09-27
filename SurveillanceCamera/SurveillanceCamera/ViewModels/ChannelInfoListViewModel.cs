using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SurveillanceCamera.Models;
using SurveillanceCamera.Services;
using SurveillanceCamera.Services.CameraService;
using SurveillanceCamera.Services.Serialization;
using Xamarin.Forms;

namespace SurveillanceCamera.ViewModels
{
    public class ChannelInfoListViewModel : BaseViewModel
    {
        public delegate void Handler(ObservableCollection<ChannelInfo> channels);
        public event Handler SelectionChanged;

        private ICameraService _cameraService;
        private ISerializationService _serializationService;
        private ObservableCollection<ChannelInfo> _channelList;
        private bool _isRefreshing;
        private bool _isSelectionChanged;
        
        public Command SelectionChangedCommand { get; }
        public Command RefreshCommand { get; }

        public bool IsRefreshing
        {
            get => _isRefreshing; 
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
        
        public ObservableCollection<object> SelectedChannels { get; set; } = new();

        public ObservableCollection<ChannelInfo> ChannelList
        {
            get => _channelList;
            set
            {
                _channelList = value;
                _channelList = new ObservableCollection<ChannelInfo>(_channelList.OrderBy(channel => channel.Name));
                OnPropertyChanged();
            }
        }

        public ChannelInfoListViewModel(ICameraService cameraService, ISerializationService serializationService)
        {
            _cameraService = cameraService;
            _serializationService = serializationService;
            
            LoadChannelInfoList();

            SelectionChangedCommand = new Command(() =>
            {
                _isSelectionChanged = true;
            });

            RefreshCommand = new Command(() =>
            {
                LoadChannelInfoList();
                SelectedChannels.Clear();
                _isSelectionChanged = true;
                IsRefreshing = false;
            });
        }

        private void LoadChannelInfoList()
        {
            Task.Run(async () =>
            {
                var xmlResult = await _cameraService.GetChannelInfo();
                ChannelList = _serializationService.Deserialize(xmlResult);
            });
        }


        public void CheckSelectedChannels()
        {
            if (!_isSelectionChanged) return;
            _isSelectionChanged = false;
            SelectionChanged?.Invoke(new ObservableCollection<ChannelInfo>(SelectedChannels.Select(ch => (ChannelInfo) ch)));
        }
    }
}