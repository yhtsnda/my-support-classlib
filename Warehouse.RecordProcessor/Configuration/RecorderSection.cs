using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace Warehouse.RecordProcessor
{
    internal class RecorderSection : ConfigurationSection
    {
        private RecorderNode mRoot;

        public RecorderNode RootNode
        {
            get
            {
                return mRoot;
            }
        }

        public static RecorderSection Instance
        {
            get
            {
                return ConfigurationManager.GetSection("warehouse.recorder") as RecorderSection;
            }
        }

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            string xml = reader.ReadOuterXml();
            XDocument doc = XDocument.Parse(xml);
            mRoot = RecorderNode.ConvertFrom((XElement)doc.FirstNode, null);
        }

        public IEnumerable<RecorderNode> TryGetNodes(string path)
        {
            return mRoot.TryGetNodes(path);
        }

        public RecorderNode TryGetNode(string path)
        {
            return mRoot.TryGetNode(path);
        }

        public string TryGetValue(string path)
        {
            return mRoot.TryGetValue(path);
        }

        public Type TryGetType(string path)
        {
            string type = TryGetValue(path);
            if (type == null)
                return null;
            return Type.GetType(type);
        }

        public List<Type> TryGetTypes(string path, string attrKey)
        {
            var typeList = mRoot.TryGetNodes(path);
            if (typeList != null)
            {
                return typeList.Select(item =>
                {
                    string type = item.Attributes[attrKey];
                    if (type != null)
                        return Type.GetType(type);
                    return null;
                }).ToList();
            }
            return null;
        }

        public T TryGetInstance<T>(string path) where T : class
        {
            Type type = TryGetType(path);
            if (type == null)
                return null;
            return (T)FastActivator.Create(type);
        }
    }
}
