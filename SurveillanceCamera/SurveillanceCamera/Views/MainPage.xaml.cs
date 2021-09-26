using System;
using System.Linq;
using LibVLCSharp.Shared;
using SurveillanceCamera.ViewModels;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace SurveillanceCamera.Views
{
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        private bool _isFirstCurrentPageChanged = true;
        public MainPage()
        {
            InitializeComponent();

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            
            Core.Initialize();
        }

        private void MainPage_OnCurrentPageChanged(object sender, EventArgs e)
        {
            var masterViewModel = BindingContext as MasterViewModel;

            if (_isFirstCurrentPageChanged)
            {
                _isFirstCurrentPageChanged = false;
            }
            else
            {
                var snapShotsPage = Children.First( page => page.Title == "Snapshots");
                if (CurrentPage == snapShotsPage)
                {
                    masterViewModel?.CurrentPageChangedHandler();
                }
            }
        }

    }
}