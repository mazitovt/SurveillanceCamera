using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using SurveillanceCamera.Models;

namespace SurveillanceCamera.Services.Serialization
{
    public class CustomSerializationService : ISerializationService
    {
        public ObservableCollection<ChannelInfo> Deserialize(string xmlDocument)
        {
            List<Dictionary<string, string>> listOfArgs = new List<Dictionary<string, string>>();
            var listOfSecObjectInfo = new List<SecObjectInfo>();

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlDocument);
            XmlElement xRoot = xDoc.DocumentElement;
            
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == "Channels")
                {
                    foreach (XmlElement? info in xnode)
                    {
                        listOfArgs.Add(new Dictionary<string, string>()
                        {
                            ["Id"] = info.Attributes["Id"]?.Value,
                            ["Name"] = info.Attributes["Name"]?.Value,
                            ["IsDisabled"] = info.Attributes["IsDisabled"]?.Value,
                            ["IsSoundOn"] = info.Attributes["IsSoundOn"]?.Value
                        });
                    }
                }

                if (xnode.Name == "RootSecurityObject")
                {
                    GetListOfSecObjects(xnode, listOfSecObjectInfo);
                }
            }

            foreach (var args in listOfArgs)
            {
                foreach (var secObjectInfo in listOfSecObjectInfo)
                {
                    if (secObjectInfo.ChildChannels.Contains(args["Id"]))
                    {
                        args["RootDirName"] = secObjectInfo.Name;
                    }
                }
            }

            return new ObservableCollection<ChannelInfo>(listOfArgs.Select(CreateChannelInfo));
        }
        
        private ChannelInfo CreateChannelInfo(Dictionary<string, string> args)
        {
            var channelInfo = new ChannelInfo()
            {
                Id = args["Id"],
                Name = args["Name"],
                RootDirName = args["RootDirName"]
            };

            bool parsedValue;

            if (bool.TryParse(args["IsDisabled"], out parsedValue))
            {
                channelInfo.IsDisabled = parsedValue;
            }
            
            if (bool.TryParse(args["IsSoundOn"], out parsedValue))
            {
                channelInfo.IsSoundOn = parsedValue;
            }
            
            return channelInfo;
        }

        private void GetListOfSecObjects(XmlNode xnode, List<SecObjectInfo> listOfSecObjectInfo)
        {
            // Console.WriteLine("RootSecurityObject Id: " + xnode.Attributes?["Id"]?.Value);

            SecObjectInfo secObjectInfo;

            foreach (XmlNode node in xnode["ChildSecurityObjects"])
            {
                secObjectInfo = new SecObjectInfo();

                if (node.Attributes != null)
                {
                    secObjectInfo.Id = node.Attributes["Id"]?.Value;
                    secObjectInfo.Name = node.Attributes["Name"]?.Value;
                }

                secObjectInfo.ChildChannels = new List<string>();

                foreach (XmlNode channelId in node["ChildChannels"])
                {
                    secObjectInfo.ChildChannels.Add(channelId.InnerText);
                }

                listOfSecObjectInfo.Add(secObjectInfo);
            }
        }

        
    }
}