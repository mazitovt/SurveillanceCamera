using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using SurveillanceCamera.Models;

namespace SurveillanceCamera.Services
{
    public static class AppSettingsLoader
    {
        private static AppSettings _appSettings;

        public static AppSettings AppSettings
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
        
        private static void LoadAppSettings()
        {
            // Get the assembly this code is executing in
            var stream = Assembly.GetAssembly(typeof(AppSettings))
                .GetManifestResourceStream("SurveillanceCamera.appsettings.json");

            if (stream == null)
            {
                return;
            }

            using var streamReader = new StreamReader(stream);
            var jsonSettings = streamReader.ReadToEnd();
            _appSettings = JsonConvert.DeserializeObject<AppSettings>(jsonSettings);
        }
    }
}