namespace SurveillanceCamera.Services.SnapshotSaver
{
    public interface ISnapshotSaver
    {
        void SaveFrame(uint width, uint height, string url, string destination, string filePath);
    }
}