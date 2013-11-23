using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public interface ILogFactory
    {
        ILog GetLogger(string name);

        ILog GetLogger(Type type);

        void Flush();
    }
}
