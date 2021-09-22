using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using SurveillanceCamera.Models;

namespace SurveillanceCamera.Services
{
    public class CamerasService
    {
        private string _channelsURL;
        private static UriBuilder uriBuilder = new UriBuilder(AppSettingsLoader.AppSettings.Stream);

        public CamerasService()
        {
            _channelsURL = AppSettingsLoader.AppSettings.Channels;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            // client.DefaultRequestHeaders.Add("Accept","application/xml");
            return client;
        }
        
        public async Task<string> GetChannelInfo()
        {
            HttpClient client = GetClient();
            return await client.GetStringAsync(_channelsURL);
        }
        
        public static string GetStreamUrl(string newId)
        {
            var qs = HttpUtility.ParseQueryString(uriBuilder.Query); 
            qs.Set("channelid",newId); 
            uriBuilder.Query = qs.ToString(); 
            return uriBuilder.Uri.ToString();
        }
        
    }
}