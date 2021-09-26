namespace SurveillanceCamera.ViewModels
{
    public class MasterViewModel : BaseViewModel
    {
        public ChannelInfoListViewModel ChannelInfoListViewModel { get; set; }
        public StreamViewModel StreamViewModel { get; set; }

        public MasterViewModel()
        {
            ChannelInfoListViewModel = new ChannelInfoListViewModel();
            StreamViewModel = new StreamViewModel();

            ChannelInfoListViewModel.SelectionChanged += StreamViewModel.HandleSelectedChannels;
        }

        public void CurrentPageChangedHandler()
        {
            ChannelInfoListViewModel.CheckSelectedChannels();
        }
    }
}