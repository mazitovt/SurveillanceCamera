using System.Collections.ObjectModel;
using SurveillanceCamera.Models;

namespace SurveillanceCamera.Services
{
    public interface ISerializationService
    {
        public ObservableCollection<ChannelInfo> Deserialize(string xmlDoc);
    }
}