using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Reflection;

namespace Projects.Tool.Lock
{
    internal class NullLockFactory : ILockFactory
    {
        ILock syncLock;

        public NullLockFactory()
        {
            syncLock = ToolSection.Instance.TryGetInstance<ILock>("synclock/type") ?? NullLocker.Instance;
        }

        public ILock GetLocker(string name)
        {
            return syncLock;
        }

        private class NullLocker : ILock
        {
            internal static readonly ILock Instance = new NullLockFactory.NullLocker();

            public bool AcquireLock(string key, int timeout)
            {
                return true;
            }

            public void ReleaseLock(string key)
            {
            }
        }

    }
}
