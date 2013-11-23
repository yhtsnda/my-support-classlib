using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Lock
{
    public interface ILock
    {
        bool AcquireLock(string key, int timeout);

        void ReleaseLock(string key);
    }
}
