using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SurveillanceCamera.Models;
using SurveillanceCamera.Services;

namespace SurveillanceCamera
{
    public partial class MainPage : TabbedPage
    {
        
        public MainPage()
        {            
            InitializeComponent();

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            
        }

        protected override void OnAppearing()
        {

            
            
        }

        
        private void OpenResources()
        {
            
        }

        public void ReadAppSettings()
        {
            
        }
        private void CheckConnection()
        {
            // if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            // {
            //     _connectionStateLbl.Text = "Доступные типы подключений:\n";
            //     var profiles = Connectivity.ConnectionProfiles;
            //     foreach (var profile in profiles)
            //     {
            //         _connectionStateLbl.Text += $"{profile.ToString()} \n";
            //     }
            // }
            // else
            // { 
            //     _connectionStateLbl.Text = "Подключение отсутствует";
            // }
        }

        private void SelectableItemsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            return;
        }

        private void MainPage_OnCurrentPageChanged(object sender, EventArgs e)
        {
            return;
        }
    }
}