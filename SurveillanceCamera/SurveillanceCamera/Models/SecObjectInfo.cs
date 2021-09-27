using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SurveillanceCamera.Models
{
    [Serializable]
    public class SecObjectInfo
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }
        
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlArray("ChildChannels")]
        public List<string> ChildChannels { get; set; }
        
        public SecObjectInfo()
        {
            
        }

        public override string ToString()
        {
            return $"{Id}, {Name}, {string.Join(" ", ChildChannels)}";
        }
    }
}