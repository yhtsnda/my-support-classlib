using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Projects.Tool.Interfaces;

namespace Projects.Tool
{
    public class Log4NetLogFactory : ILogFactory
    {
        public Log4NetLogFactory()
        {
            var node = ToolSection.Instance.TryGetNode("log/logProvider");
            string path = node.TryGetValue("configPath");
            if (System.Web.HttpContext.Current == null)
            {
                path = System.IO.Path.Combine(System.Environment.CurrentDirectory, path);
            }
            else
            {
                path = System.Web.HttpContext.Current.Server.MapPath(path);
            }
            if (File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                log4net.Config.XmlConfigurator.Configure(fi);
            }
            else
            {
                log4net.Config.XmlConfigurator.Configure();
            }
        }

        public ILog GetLogger(string name = "default")
        {
            return new Log4NetLog(log4net.LogManager.GetLogger(name));
        }

        public ILog GetLogger(Type type)
        {
            return new Log4NetLog(log4net.LogManager.GetLogger(type));
        }

        public void Flush()
        {
            
        }
    }
}
