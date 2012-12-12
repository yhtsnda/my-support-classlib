using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Warehouse.RecordProcessor
{
    internal class RecorderNode
    {
        public string Name { get; internal set; }

        public RecorderNode Parent { get; internal set; }

        public IEnumerable<RecorderNode> Nodes { get; internal set; }

        public IDictionary<string, string> Attributes { get; internal set; }

        internal bool IsRoot
        {
            get
            {
                return this == RecorderSection.Instance.RootNode;
            }
        }

        public string Path
        {
            get
            {
                string path = "";
                RecorderNode node = this;
                while (node != null)
                {
                    path = node.Name + "/" + path;
                    node = node.Parent;
                }
                return path;
            }
        }

        public static RecorderNode ConvertFrom(XElement element, RecorderNode parent)
        {
            RecorderNode node = new RecorderNode();
            node.Name = element.Name.LocalName;
            node.Parent = parent;
            node.Attributes = element.Attributes().ToDictionary(o => o.Name.LocalName, o => o.Value);
            node.Nodes = element.Elements().Select(o => ConvertFrom(o, node));
            return node;
        }

        public bool Contains(string path)
        {
            path = path.Replace("\\", "/");
            int f = path.IndexOf("/");
            if (f == -1)
            {
                string v;
                bool hasAttr = Attributes.TryGetValue(path, out v);
                if (hasAttr)
                    return true;
                return Nodes.Any(o => o.Name == path);
            }

            string name = path.Substring(0, f);
            RecorderNode node = Nodes.FirstOrDefault(o => o.Name == name);
            if (node == null)
                return false;
            return node.Contains(path.Substring(f + 1));
        }

        public RecorderNode TryGetNode(string path)
        {
            path = path.Replace("\\", "/");
            int f = path.IndexOf("/");
            if (f == -1)
                return Nodes.FirstOrDefault(o => o.Name == path);

            string name = path.Substring(0, f);
            RecorderNode node = Nodes.FirstOrDefault(o => o.Name == name);
            if (node == null)
                return null;
            return node.TryGetNode(path.Substring(f + 1));
        }

        public IEnumerable<RecorderNode> TryGetNodes(string path)
        {
            path = path.Replace("\\", "/");
            int f = path.IndexOf("/");
            if (f == -1)
                return Nodes.Where(o => o.Name == path);

            string name = path.Substring(0, f);
            RecorderNode node = Nodes.FirstOrDefault(o => o.Name == name);
            if (node == null)
                return new List<RecorderNode>();
            return node.TryGetNodes(path.Substring(f + 1));
        }

        public string TryGetValue(string path)
        {
            path = path.Replace("\\", "/");
            int f = path.IndexOf("/");
            if (f == -1)
            {
                string v;
                if (Attributes.TryGetValue(path, out v))
                    return v;
                return null;
            }
            string name = path.Substring(0, f);

            RecorderNode node = Nodes.FirstOrDefault(o => o.Name == name);
            if (node == null)
                return null;

            return node.TryGetValue(path.Substring(f + 1));
        }
    }
}
