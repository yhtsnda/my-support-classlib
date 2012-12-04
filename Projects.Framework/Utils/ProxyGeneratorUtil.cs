using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

namespace Projects.Framework
{
    internal class ProxyGeneratorUtil
    {
        private static ProxyGenerator mGenerator;

        static ProxyGeneratorUtil()
        {
            mGenerator = new ProxyGenerator();
        }

        public static ProxyGenerator Instance
        {
            get { return mGenerator; }
        }
    }
}
