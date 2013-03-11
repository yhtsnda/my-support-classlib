using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Projects.Tool.Util;
using Projects.Tool.Reflection;

namespace Projects.Tool
{
    public static class LogManager
    {
        static InnerManager<ILogFactory> manager;
        static object syncObj = new object();

        static InnerManager<ILogFactory> Manager
        {
            get
            {
                if (manager == null)
                {
                    lock (syncObj)
                    {
                        if (manager == null)
                        {
                            var m = new InnerManager<ILogFactory>();
                            var factory = new NullLogFactory();
                            if (ToolSection.Instance == null)
                                m.AssignFactory(factory);
                            else
                                m.AssignFactory(ToolSection.Instance.TryGetInstance<ILogFactory>("log/factory") ?? factory);
                            manager = m;
                        }
                    }
                }
                return manager;
            }
        }

        public static void AssignFactory(ILogFactory factory)
        {
            Manager.AssignFactory(factory);
        }

        public static ILog GetLogger(string name)
        {
            return Manager.Factory.GetLogger(name);
        }

        public static ILog GetLogger(Type type)
        {
            return Manager.Factory.GetLogger(type);
        }

        public static ILog GetLogger<T>()
        {
            return Manager.Factory.GetLogger(typeof(T));
        }

        public static void Flush()
        {
            Manager.Factory.Flush();
        }
    }
}
