using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Xml.Linq;
using System.IO;
using System.Web.Hosting;

namespace Avalon.Utility
{
    public class ToolSection : ConfigurationSection
    {
        const string SectionName = "avalon";
        SettingNode rootNode;
        static ToolSection toolSection;
        static FileSystemWatcher watcher;

        static ToolSection()
        {
            if (HostingEnvironment.IsHosted)
            {
                var file = HostingEnvironment.MapPath("~/web.config");
                XDocument doc = XDocument.Load(file);
                var configSourceAttr = doc.Descendants("avalon").Attributes("configSource").FirstOrDefault();

                if (configSourceAttr != null)
                {
                    var configSource = configSourceAttr.Value;
                    if (!configSource.StartsWith("~"))
                        configSource = Path.Combine("~/", configSource);
                    var path = HostingEnvironment.MapPath(configSource);

                    watcher = new FileSystemWatcher(Path.GetDirectoryName(path), Path.GetFileName(path));
                    watcher.NotifyFilter = NotifyFilters.LastWrite;
                    watcher.Changed += watcher_Changed;
                    watcher.EnableRaisingEvents = true;
                }
            }
        }

        public static string ConfigFilePath
        {
            get;
            set;
        }

        public static event EventHandler ToolSectionChanged;

        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            ConfigurationManager.RefreshSection(SectionName);
            if (ToolSectionChanged != null)
                ToolSectionChanged(sender, e);
        }

        ToolSection()
        {
            rootNode = new SettingNode()
            {
                Attributes = new Dictionary<string, string>(),
                Nodes = new List<SettingNode>()
            };
        }

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
                if (toolSection == null)
                {
                    if (!String.IsNullOrEmpty(ConfigFilePath))
                    {
                        if (!File.Exists(ConfigFilePath))
                            throw new FileNotFoundException(ConfigFilePath);

                        var mp = new ExeConfigurationFileMap() { ExeConfigFilename = ConfigFilePath };
                        var config = ConfigurationManager.OpenMappedExeConfiguration(mp, ConfigurationUserLevel.None);
                        toolSection = (ToolSection)config.GetSection("avalon");
                    }
                    else
                    {
                        toolSection = ConfigurationManager.GetSection(SectionName) as ToolSection;
                        if (toolSection == null)
                            toolSection = new ToolSection();
                    }
                }
                return toolSection;
            }
        }

        public static void Reset()
        {
            toolSection = null;
        }
    }
}
