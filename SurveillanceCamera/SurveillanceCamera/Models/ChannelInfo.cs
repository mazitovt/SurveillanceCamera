using System;
using System.Xml.Serialization;

namespace SurveillanceCamera.Models
{
    [Serializable]
    public class ChannelInfo
    {
        
        [XmlAttribute("Id")]
        public string Id { get; set; }
        
        [XmlAttribute("Name")]
        public string Name { get; set; }
        
        [XmlAttribute("IsDisabled")]
        public bool IsDisabled { get; set; }
        
        [XmlAttribute("IsSoundOn")]
        public bool IsSoundOn { get; set; }
        
        public string RootDirName { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, disabled: {IsDisabled}, sound: {IsSoundOn}, root: {RootDirName} ";
        }
    }
}