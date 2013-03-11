using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Lock
{
    public interface ILockFactory
    {
        ILock GetLocker(string name);
    }
}
