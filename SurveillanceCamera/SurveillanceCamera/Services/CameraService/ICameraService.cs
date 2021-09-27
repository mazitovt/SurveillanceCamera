using System.Threading.Tasks;

namespace SurveillanceCamera.Services.CameraService
{
    public interface ICameraService
    {
        Task<string> GetChannelInfo(); 
        string GetStreamUrl(string newId); 
    }
}