using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web;

[assembly: TagPrefix("Nd.Resource", "Nd")]
namespace Avalon.Resource
{
    [ToolboxData("<{0}:Resource runat=\"server\" ></{0}:Resource>")]
    [ParseChildren(true, "Path")]
    [PersistChildren(false, true)]
    public class Resource : Control, INamingContainer
    {
        /// <summary>
        /// 资源地址，多个资源使用英文分号或者英文逗号分隔
        /// </summary>
        [BrowsableAttribute(true)]
        [DescriptionAttribute("资源的Uri地址")]
        [Category("Data")]
        [DefaultValue("")]
        public string Path { get; set; }

        private bool debug = GlobalConfig.Debug;
        /// <summary>
        /// 是否调试
        /// </summary>
        [BrowsableAttribute(true)]
        [DescriptionAttribute("是否调试")]
        [Category("Data")]
        [DefaultValue("false")]
        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        /// <summary>
        /// 静态资源分组
        /// </summary>
        [BrowsableAttribute(true)]
        [DescriptionAttribute("静态资源分组名称")]
        [Category("Data")]
        [DefaultValue("")]
        public string Group { get; set; }

        /// <summary>
        /// 基础目录
        /// </summary>
        [BrowsableAttribute(true)]
        [DescriptionAttribute("基础目录")]
        [Category("Data")]
        [DefaultValue("")]
        public string BaseFolder { get; set; }

        private ResourceType resourceType = ResourceType.Undefined;
        [BrowsableAttribute(true)]
        [DescriptionAttribute("资源类型")]
        [Category("Data")]
        [DefaultValue("")]
        public ResourceType ResourceType
        {
            get { return resourceType; }
            set { resourceType = value; }
        }

        /// <summary>
        /// 控件呈现
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            IResourceCollector collector = GenerateResources();
            writer.Write(collector.BuildHtmlTag());
        }

        /// <summary>
        /// 组装控件引用的所有资源项
        /// </summary>
        private ResourceCollector GenerateResources()
        {
            ResourceCollector collector = new ResourceCollector()
            {
                BaseFolder = BaseFolder,
                Debug = Debug,
                Group = Group,
                ResourceType = resourceType
            };
            string[] pathList = Path.Split(new char[] { ';', '\n', ',', '；', '|' }, StringSplitOptions.RemoveEmptyEntries);
            string path = string.Empty;
            foreach (string url in pathList)
            {
                path = url.Trim();
                if (!string.IsNullOrEmpty(path))
                {
                    collector.Add(path);
                }
            }

            return collector;
        }

        /// <summary>
        /// 引用静态资源文件
        /// </summary>
        /// <param name="collector"></param>
        /// <returns></returns>
        public static string Include(Action<IResourceCollector> collector)
        {
            IResourceCollector resources = new ResourceCollector();
            collector(resources);
            return resources.BuildHtmlTag();
            //return new HtmlString(resources.BuildHtmlTag());
        }

        /// <summary>
        /// 获取静态服务器地址
        /// </summary>
        public static string ServerUrl
        {
            get
            {
                return GlobalConfig.ServerList[new Random().Next(GlobalConfig.ServerList.Count)];
            }
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        public static string Version
        {
            get
            {
                return GlobalConfig.Version;
            }
            set
            {
                GlobalConfig.Version = value;
            }
        }

        /// <summary>
        /// 包装静态文件地址
        /// </summary>
        /// <param name="staticFileUrl"></param>
        /// <returns></returns>
        public static string WrapperUrl(string staticFileUrl)
        {
            return string.Concat(ServerUrl, staticFileUrl, "?version=", Version);
        }
    }
}
