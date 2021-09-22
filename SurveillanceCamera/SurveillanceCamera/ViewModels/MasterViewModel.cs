namespace SurveillanceCamera.ViewModels
{
    public class MasterViewModel : BaseViewModel
    {
        public ChannelInfoListViewModel ChannelInfoListViewModel { get; set; }
        public StreamViewModel StreamViewModel { get; set; }

        public MasterViewModel()
        {
            ChannelInfoListViewModel = new();
            StreamViewModel = new();
        }
        
        
    }
}