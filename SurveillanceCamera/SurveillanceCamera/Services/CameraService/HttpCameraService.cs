using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SurveillanceCamera.Services.CameraService
{
    public class HttpCameraService : ICameraService
    {
        private readonly string _channelsUrl;
        private static readonly UriBuilder UriBuilder = new UriBuilder(AppSettingsLoader.AppSettings.Stream);

        public HttpCameraService()
        {
            _channelsUrl = AppSettingsLoader.AppSettings.Channels;
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
            return await client.GetStringAsync(_channelsUrl);
        }
        
        public string GetStreamUrl(string newId)
        {
            var qs = HttpUtility.ParseQueryString(UriBuilder.Query); 
            qs.Set("channelid",newId); 
            UriBuilder.Query = qs.ToString(); 
            return UriBuilder.Uri.ToString();
        }
    }
}