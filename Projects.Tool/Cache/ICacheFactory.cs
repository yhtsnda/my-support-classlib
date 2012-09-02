using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public interface ICacheFactory
    {
        ICache GetCacher(string name);
        ICache GetCacher(Type type);
    }
}
