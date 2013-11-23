using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CacheDominPrefixAttribute : Attribute
    {
        readonly string cacheDominPrefix;
        readonly string schemeName;

        public CacheDominPrefixAttribute(string cacheDominPrefix)
            : this(null, cacheDominPrefix)
        {
        }

        public CacheDominPrefixAttribute(string schemeName, string cacheDominPrefix)
        {
            this.schemeName = schemeName;
            this.cacheDominPrefix = cacheDominPrefix;
        }

        /// <summary>
        /// 获取方案名称
        /// </summary>
        public string SchemeName
        {
            get { return schemeName; }
        }

        public string CacheDominPrefix
        {
            get { return cacheDominPrefix; }
        }
    }
}
