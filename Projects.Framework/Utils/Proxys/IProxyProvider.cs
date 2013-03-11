using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal interface IProxyProvider
    {
        bool IsMatch(Type type);

        object Proxy(object entity, Func<object, object> subProxyHandler);

        object Poco(object entity, Func<object, object> subPocoHandler);

        void Fetch(object entity, Action<object> subFetchHandler);
    }
}
