using System.Threading.Tasks;

namespace SurveillanceCamera.Services
{
    public interface ISnapshotSaver
    {
        void SaveFrame(uint width, uint height, string url, string destination, string filePath);
    }
}