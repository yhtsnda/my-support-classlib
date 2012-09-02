using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;


namespace BuildingSiteCheck.Resource
{
    public class GlobalConfig
    {
        private static NestedConfig mConfig;

        static GlobalConfig()
        {
            mConfig = InitConfig();
        }

        /// <summary>
        /// 是否调试
        /// </summary>
        public static bool Debug { get { return mConfig.Debug; } }

        /// <summary>
        /// 当前版本
        /// </summary>
        public static string Version { get { return mConfig.Version; } }

        /// <summary>
        /// 静态资源站列表 
        /// </summary>
        public static List<string> ServerList { get { return mConfig.ServerList; } }

        private static NestedConfig InitConfig()
        {
            var configs = ConfigurationManager.GetSection("projects.resource") as NameValueCollection;
            if (configs == null)
                throw new Exception("配置文件中缺少projects.resource节点");

            var config = new NestedConfig();
            //Debug
            if (!String.IsNullOrEmpty(configs["Debug"]))
                config.Debug = Convert.ToBoolean(configs["Debug"]);
            //Version
            if (!String.IsNullOrEmpty(configs["Version"]))
                config.Version = Convert.ToString(configs["Version"]);
            //ServierList
            if (String.IsNullOrEmpty(configs["StaticSite"]))
                throw new Exception("配置节点StaticSite不能为空");
            var servers = configs["StaticSite"].Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var url in servers)
            {
                string server = String.Empty;
                if (!url.StartsWith("http"))
                    server = String.Concat("http://", url);
                else
                    server = url;

                if (!url.EndsWith("/"))
                    server += "/";
                config.ServerList.Add(url);
            }
            return config;
        }

        private class NestedConfig
        {
            public NestedConfig()
            {
                ServerList = new List<string>();
            }

            public string Version { get; set; }

            public bool Debug { get; set; }

            public List<string> ServerList { get; set; }
        }
    }

}
