using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Util;
using System.Configuration;
using System.ComponentModel;
using Projects.Tool.Lock;
using System.Xml.Linq;
using Projects.Tool.Reflection;

namespace Projects.Tool
{
    public class ToolSection : ConfigurationSection
    {
        SettingNode rootNode;

        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            string xml = reader.ReadOuterXml();
            XDocument doc = XDocument.Parse(xml);
            rootNode = SettingNode.ConvertFrom((XElement)doc.FirstNode, null);
        }

        public IEnumerable<SettingNode> TryGetNodes(string path)
        {
            return rootNode.TryGetNodes(path);
        }

        public SettingNode TryGetNode(string path)
        {
            return rootNode.TryGetNode(path);
        }

        public string TryGetValue(string path)
        {
            return rootNode.TryGetValue(path);
        }

        public Type TryGetType(string path)
        {
            string type = TryGetValue(path);
            if (type == null)
                return null;
            return Type.GetType(type);
        }

        public T TryGetInstance<T>(string path) where T : class
        {
            Type type = TryGetType(path);
            if (type == null)
                return null;
            return (T)FastActivator.Create(type);
        }

        public SettingNode RootNode
        {
            get
            {
                return rootNode;
            }
        }

        public static ToolSection Instance
        {
            get
            {
                return ConfigurationManager.GetSection("Projects.Tool") as ToolSection;
            }
        }
    }
}
