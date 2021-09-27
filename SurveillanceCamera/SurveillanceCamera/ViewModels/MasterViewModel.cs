using SurveillanceCamera.Services;
using SurveillanceCamera.Services.CameraService;
using SurveillanceCamera.Services.Serialization;
using SurveillanceCamera.Services.SnapshotSaver;

namespace SurveillanceCamera.ViewModels
{
    public class MasterViewModel : BaseViewModel
    {
        public ChannelInfoListViewModel ChannelInfoListViewModel { get; set; }
        public StreamViewModel StreamViewModel { get; set; }

        public MasterViewModel()
        {
            ChannelInfoListViewModel = new ChannelInfoListViewModel(new HttpCameraService(), new CustomSerializationService());
            StreamViewModel = new StreamViewModel(new HttpCameraService(), new CustomSnapshotSaver());

            ChannelInfoListViewModel.SelectionChanged += StreamViewModel.HandleSelectedChannels;
        }

        public void CurrentPageChangedHandler()
        {
            ChannelInfoListViewModel.CheckSelectedChannels();
        }
    }
}