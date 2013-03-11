using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Reflection;

namespace Projects.Tool.Lock
{
    public static class LockManager
    {
        static InnerManager<ILockFactory> manager;
        static object syncObj = new object();

        static InnerManager<ILockFactory> Manager
        {
            get
            {
                if (manager == null)
                {
                    lock (syncObj)
                    {
                        if (manager == null)
                        {
                            var m = new InnerManager<ILockFactory>();
                            m.AssignFactory(ToolSection.Instance.TryGetInstance<ILockFactory>("synclock/factory") ?? new NullLockFactory());
                            manager = m;
                        }
                    }
                }
                return manager;
            }
        }

        public static void AssignFactory(ILockFactory factory)
        {
            Manager.AssignFactory(factory);
        }


        public static ILock GetLocker(string name)
        {
            return Manager.Factory.GetLocker(name);
        }
    }
}
