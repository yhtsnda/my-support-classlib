using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Projects.Framework.Specification
{
    /// <summary>
    /// 规约的标致，于缓存键的产生
    /// </summary>
    public abstract class AbstractSpecification : ICacheKeySupport
    {
        public abstract IDictionary<string, object> Serialize();
    }
}
