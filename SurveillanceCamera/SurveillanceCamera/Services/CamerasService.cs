using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SurveillanceCamera.Models;
using SurveillanceCamera.Services;

namespace SurveillanceCamera
{
    public class CamerasService
    {

        private string _url;
        
        public CamerasService(string url)
        {
            _url = url;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            // client.DefaultRequestHeaders.Add("Accept","application/xml");
            return client;
        }

        // public async Task<List<ChannelInfo>> Get()
        // {
        //     HttpClient client = GetClient();
        //     string result = await client.GetStringAsync(_url);
        //     return SerializationService.Serialize(result);
        // }
        
        public string Get()
        {
            HttpClient client = GetClient();
            string result = client.GetStringAsync(_url).Result;
            return result;
        }
    }
}