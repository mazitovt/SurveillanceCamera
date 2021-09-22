using System;
using SurveillanceCamera.Models;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms;
using SurveillanceCamera.Services;
using SurveillanceCamera.ViewModels;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace SurveillanceCamera
{
    public partial class MainPage : TabbedPage
    {

        public MainPage()
        {
            InitializeComponent();

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);


            var masterViewModel = new MasterViewModel();
            
            
            
            
        }
        
        private void SelectableItemsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            return;
        }

        private void MainPage_OnCurrentPageChanged(object sender, EventArgs e)
        {
            return;
        }
        
        private void SetSnapshots()
        {
            // image1.Source = "/storage/emulated/0/Download/preview2/snapshot.jpg";
        }

        private void Btn_OnClicked(object sender, EventArgs e)
        {
            var destination = "/storage/emulated/0/Download/preview3/";

            var appSettings = AppSettingsLoader.AppSettings;
            
            var streamUrl = StreamService.GetStreamUrl("2410f79c-8f7e-4cd4-8baf-f7be29869a7e");
            
            new SnapshotSaver(appSettings.Width, appSettings.Height).SaveFrame(streamUrl, destination);
            
            SetSnapshots();
        }

        private void Btn1_OnClicked(object sender, EventArgs e)
        {
            var destination = "/storage/emulated/0/Download/2/2410f79c-8f7e-4cd4-8baf-f7be29869a7e";

            var appSettings = AppSettingsLoader.AppSettings;
            
            var streamUrl = StreamService.GetStreamUrl("2410f79c-8f7e-4cd4-8baf-f7be29869a7e");
            
            new SnapshotSaver(appSettings.Width, appSettings.Height).SaveFrame(streamUrl, destination);
            
            SetSnapshots();
        }
    }
}