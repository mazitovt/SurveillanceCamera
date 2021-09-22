using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SurveillanceCamera.Models;
using System.Drawing;
using System.IO;
using System.Web;


namespace SurveillanceCamera.Services
{
    public static class StreamService
    {

        // private static string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static UriBuilder uriBuilder = new UriBuilder(AppSettingsLoader.AppSettings.Stream);
        // private string _streamUrl;
        
        public static string GetStreamUrl(string newId)
        {
            var qs = HttpUtility.ParseQueryString(uriBuilder.Query); 
            qs.Set("channelid",newId); 
            uriBuilder.Query = qs.ToString(); 
            return uriBuilder.Uri.ToString();
        }
    }
}