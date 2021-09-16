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
    public class StreamService
    {

        private static string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static UriBuilder uriBuilder = new UriBuilder(AppSettingsService.AppSettings.Stream);
        private string _streamUrl;

        public StreamService(string channelId)
        {
            _streamUrl = CreateStreamUrl(channelId);
        }
        
        private HttpClient GetClient()
        {
            var client = new HttpClient();
            return client;
        }

        public void GetStream()
        {
           
        }

        private string CreateStreamUrl(string newId)
        {
            var qs = HttpUtility.ParseQueryString(uriBuilder.Query); 
            qs.Set("channelid",newId); 
            uriBuilder.Query = qs.ToString(); 
            return uriBuilder.Uri.ToString();
        }

      
    }
}