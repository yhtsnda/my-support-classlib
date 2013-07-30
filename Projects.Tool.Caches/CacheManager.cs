using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Projects.Tool.Reflection;

namespace Projects.Tool
{
    public static class CacheManager
    {
        static InnerManager<ICacheFactory> manager;
        static object syncObj = new object();

        static InnerManager<ICacheFactory> Manager
        {
            get
            {
                if (manager == null)
                {
                    lock (syncObj)
                    {
                        if (manager == null)
                        {
                            var m = new InnerManager<ICacheFactory>();
                            m.AssignFactory(ToolSection.Instance.TryGetInstance<ICacheFactory>("cache/factoryType") ?? new DefaultCacheFactory());
                            manager = m;
                        }
                    }
                }
                return manager;
            }
        }

        public static ICacheFactory Factory
        {
            get { return Manager.Factory; }
        }

        public static void AssignFactory(ICacheFactory factory)
        {
            Manager.AssignFactory(factory);
        }

        public static ICache GetCacher(string name)
        {
            return Manager.Factory.GetCacher(name);
        }

        public static ICache GetCacher(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return Manager.Factory.GetCacher(type);
        }

        public static ICache GetCacher<T>()
        {
            return Manager.Factory.GetCacher(typeof(T));
        }
    }
}
