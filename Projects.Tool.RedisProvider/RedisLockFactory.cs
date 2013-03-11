using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Lock;

namespace Projects.Tool.RedisProvider
{
    public class RedisLockFactory : ILockFactory
    {
        public ILock GetLocker(string name)
        {
            return new RedisLock();
        }
    }
}
