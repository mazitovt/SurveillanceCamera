using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SurveillanceCamera.Models;

namespace SurveillanceCamera.Services
{
    public class SerializationService
    {

        public static List<ChannelInfo> Serialize(string xmlDocument)
        {
            return Read(xmlDocument);
        }
        
        private static ChannelInfo CreateChannelInfo(Dictionary<string, string> args)
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

        private static void ReadLinq()
        {
            var xDoc = XDocument.Load("D:\\response.xml");
        }

        private static List<ChannelInfo> Read(string xmlDocument)
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

            var channelInfos = listOfArgs.Select(args => CreateChannelInfo(args)).ToList();

            return channelInfos;
        }

        private static void GetListOfSecObjects(XmlNode xnode, List<SecObjectInfo> listOfSecObjectInfo)
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


        private static void ReadSerializer()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ChannelInfo));
            
            // десериализация
            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                ChannelInfo[] cameras = (ChannelInfo [])formatter.Deserialize(fs);
 
                Console.WriteLine("Объект десериализован");
                foreach (var camera in cameras)
                {
                    Console.WriteLine($"Id: {camera.Id} --- Name: {camera.Name}");
                }
            }
        }
    }
}