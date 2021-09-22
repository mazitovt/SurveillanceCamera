using System.Net.Http;
using System.Threading.Tasks;
using SurveillanceCamera.Models;

namespace SurveillanceCamera.Services
{
    public class CamerasService
    {
        private string _channelsURL;

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
        
    }
}