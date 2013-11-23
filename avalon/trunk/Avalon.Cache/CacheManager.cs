using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Avalon.Utility
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
                            m.AssignFactory(ToolSection.Instance.TryGetInstance<ICacheFactory>("cache/factoryType") ?? new SpecifiableCacheFactory());
                            manager = m;
                        }
                    }
                }
                return manager;
            }
        }

      
        public static void Reset()
        {
            manager = null;
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
            return Manager.Factory.GetCacher(GetTypeName(type));
        }

        public static ICache GetCacher<T>()
        {
            return GetCacher(typeof(T));
        }

        public static void RegisterCacher(Type type, ICache cache)
        {
            RegisterCacher(GetTypeName(type), cache);
        }

        public static void RegisterCacher<T>(ICache cache)
        {
            RegisterCacher(typeof(T), cache);
        }

        public static void RegisterCacher(string cacheName, ICache cache)
        {
            Manager.Factory.Register(cacheName, cache);
        }

        public static string GetTypeName(Type type)
        {
            return GetTypeNameInner(type);
        }

        static string GetTypeNameInner(Type type)
        {
            if (!type.IsGenericType)
                return type.FullName;

            StringBuilder sb = new StringBuilder();
            var gtype = type.GetGenericTypeDefinition();
            sb.Append(gtype.FullName.Remove(gtype.FullName.IndexOf("`")) + "<");

            bool flag = false;
            foreach (var stype in type.GetGenericArguments())
            {
                if (flag)
                    sb.Append(",");
                sb.Append(GetTypeNameInner(stype));
                flag = true;
            }
            sb.Append(">");
            return sb.ToString(); ;
        }
    }
}
