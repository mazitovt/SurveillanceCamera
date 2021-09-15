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
        private Label _connectionStateLbl;
        private AppSettings _appSettings;

        public AppSettings AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    LoadAppSettings();
                }

                return _appSettings;
            }
        }

        private void LoadAppSettings()
        {
            // Get the assembly this code is executing in
            var stream = Assembly.GetAssembly(typeof(AppSettings))
                .GetManifestResourceStream("SurveillanceCamera.appsettings.json");

            if (stream == null)
            {
                return;
            }

            using (var streamReader = new StreamReader(stream))
            {
                var jsonSettings = streamReader.ReadToEnd();
                _appSettings = JsonConvert.DeserializeObject<AppSettings>(jsonSettings);
            }
        }

        public MainPage()
        {            
            InitializeComponent();

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
 
            btn_1.Clicked += (sender, args) =>
            {
                // DisplayAlert("Title", "stream: " + AppSettings.Stream, "ok");
                GetCameras();
            };

            lbl_networkState.Text = Connectivity.NetworkAccess.ToString();
        }

        protected override void OnAppearing()
        {

            
            
        }

        private void GetCameras()
        {
            // DisplayAlert("test0","t", "ok");
            // var c = new HttpClient();
            // var res = c.GetStringAsync("http://demo.macroscop.com/configex?login=root");
            // DisplayAlert("title", res.Result.Length.ToString(), "ok");
            var cameraService = new CamerasService("http://demo.macroscop.com/configex?login=root");
            // DisplayAlert("test1", "t", "ok");
            var res = cameraService.Get();
            // DisplayAlert("test2", res.Result.Length.ToString(), "ok");
            //
            // Label lbl = null;
            var channels = SerializationService.Serialize(res);
            //
            foreach (var channelInfo in channels)
            {
                // Displ\ayAlert("Title", channelInfo.Id, "ok");
                Label lbl = new Label();
                lbl.Text = channelInfo.ToString();
                stackLayout1.Children.Add(lbl);
            }
        }
        private void OpenResources()
        {
            
        }

        public void ReadAppSettings()
        {
            
        }
        private void CheckConnection()
        {
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                _connectionStateLbl.Text = "Доступные типы подключений:\n";
                var profiles = Connectivity.ConnectionProfiles;
                foreach (var profile in profiles)
                {
                    _connectionStateLbl.Text += $"{profile.ToString()} \n";
                }
            }
            else
            { 
                _connectionStateLbl.Text = "Подключение отсутствует";
            }
        }
    }
}