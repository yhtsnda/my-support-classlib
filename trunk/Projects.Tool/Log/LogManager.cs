using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public class LogManager
    {
        static InnerManager<ILogFactory> mManager;
        static object mSyncObj = new object();

        static InnerManager<ILogFactory> Manager
        {
            get
            {
                if (mManager == null)
                {
                    lock (mSyncObj)
                    {
                        if (mManager == null)
                        {
                            var m = new InnerManager<ILogFactory>();
                            m.AssignFactory(ToolSection.Instance.TryGetInstance<ILogFactory>("log/factory") ?? new NullLogFactory());
                            mManager = m;
                        }
                    }
                }
                return mManager;
            }
        }

        public static void AssignFactory(ILogFactory factory)
        {
            Manager.AssignFactory(factory);
        }

        public static ILog GetLogger(string name = "default")
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
