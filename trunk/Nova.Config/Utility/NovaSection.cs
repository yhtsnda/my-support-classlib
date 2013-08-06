using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace Nova.Config
{
    public class NovaSection : ConfigurationSection
    {
        private ConfigNode rootNode;

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            string xml = reader.ReadOuterXml();
            XDocument doc = XDocument.Parse(xml);
            rootNode = ConfigNode.ConvertFrom((XElement)doc.FirstNode, null);
        }

        public static NovaSection Instance
        {
            get { return ConfigurationManager.GetSection("Nova") as NovaSection; }
        }
    }
}
