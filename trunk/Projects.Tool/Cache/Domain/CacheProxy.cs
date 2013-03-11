using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    internal class CacheProxy : SecondaryCache
    {
        public CacheProxy(ICache baseCache)
        {
            if (baseCache == null)
                throw new ArgumentNullException("baseCache");


            var firstCache = new HttpContextCache();
            if (baseCache is AbstractCache)
                firstCache.CacheName = ((AbstractCache)baseCache).CacheName;
            FirstCache = firstCache;
            SecondCache = baseCache;
        }

        public override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            throw new NotSupportedException();
        }
    }
}
