using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class DefaultLogFactory : ILogFactory
    {
        public ILog GetLogger(string name)
        {
            return new ConsoleLog();
        }

        public ILog GetLogger(Type type)
        {
            return new ConsoleLog();
        }

        public void Flush()
        {

        }
    }
}
