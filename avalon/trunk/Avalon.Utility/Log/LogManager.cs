using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Avalon.Utility
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
                            var factory = ToolSection.Instance.TryGetInstance<ILogFactory>("log/factory") ?? new NullLogFactory();

                            m.AssignFactory(factory);
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
