using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    internal class InnerManager<TFactory> where TFactory : class
    {
        TFactory factory;

        public TFactory Factory
        {
            get { return factory; }
        }

        public void AssignFactory(TFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");
            this.factory = factory;
        }
    }
}
