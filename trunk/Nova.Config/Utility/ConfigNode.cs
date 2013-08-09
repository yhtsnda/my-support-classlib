using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nova.Config
{
    public class ConfigNode
    {
        public string Name { get; internal set; }

        public ConfigNode Parent { get; internal set; }

        public IEnumerable<ConfigNode> Nodes { get; internal set; }

        public IDictionary<string, string> Attributes { get; internal set; }

        public string InnerText { get; internal set; }

        public bool IsRoot
        {
            get
            {
                return this == NovaSection.Instance.RootNode;
            }
        }

        public string Path
        {
            get
            {
                string path = "";
                ConfigNode node = this;
                while (node != null)
                {
                    path = node.Name + "/" + path;
                    node = node.Parent;
                }
                return path;
            }
        }

        public static ConfigNode ConvertFrom(XElement element, ConfigNode parent)
        {
            ConfigNode node = new ConfigNode();
            node.Name = element.Name.LocalName;
            node.Parent = parent;
            node.Attributes = element.Attributes().ToDictionary(o => o.Name.LocalName, o => o.Value);
            node.Nodes = element.Elements().Select(o => ConvertFrom(o, node));
            node.InnerText = element.HasElements ? String.Empty : element.Value;
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
            ConfigNode node = Nodes.FirstOrDefault(o => o.Name == name);
            if (node == null)
                return false;
            return node.Contains(path.Substring(f + 1));
        }

        public ConfigNode TryGetNode(string path)
        {
            path = path.Replace("\\", "/");
            int f = path.IndexOf("/");
            if (f == -1)
                return Nodes.FirstOrDefault(o => o.Name == path);

            string name = path.Substring(0, f);
            ConfigNode node = Nodes.FirstOrDefault(o => o.Name == name);
            if (node == null)
                return null;
            return node.TryGetNode(path.Substring(f + 1));
        }

        public IEnumerable<ConfigNode> TryGetNodes(string path)
        {
            path = path.Replace("\\", "/");
            int f = path.IndexOf("/");
            if (f == -1)
                return Nodes.Where(o => o.Name == path);

            string name = path.Substring(0, f);
            ConfigNode node = Nodes.FirstOrDefault(o => o.Name == name);
            if (node == null)
                return new List<ConfigNode>();
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

            ConfigNode node = Nodes.FirstOrDefault(o => o.Name == name);
            if (node == null)
                return null;

            return node.TryGetValue(path.Substring(f + 1));
        }
    }
}
