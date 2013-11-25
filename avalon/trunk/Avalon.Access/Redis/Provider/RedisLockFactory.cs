using Avalon.Lock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.RedisProvider
{
    public class RedisLockFactory : ILockFactory
    {
        public ILock GetLocker(string name)
        {
            return new RedisLock();
        }
    }
}
