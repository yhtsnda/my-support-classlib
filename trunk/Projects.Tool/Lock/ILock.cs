using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Lock
{
    public interface ILock
    {
        bool AcquireLock(string key, int timeout);

        void ReleaseLock(string key);
    }
}
