using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal static class CacheKeyUtil
    {
        public const string DefaultRegionName = "_default_";

        public static string GetRegionCacheKey(Type entityType, string regionName, object value)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");
            if (String.IsNullOrEmpty(regionName))
                throw new ArgumentNullException("regionName");

            if (regionName == DefaultRegionName && value != null)
                throw new ArgumentOutOfRangeException("当为默认区域时，值应该为 null");

            if (regionName == DefaultRegionName)
                return String.Format("##{0}", entityType.FullName);

            return String.Format("##{0}:{1}:{2}", entityType.FullName, regionName, value);
        }

        public static string GetQueryCacheKey(Type entityType, string queryKey)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");
            if (String.IsNullOrEmpty(queryKey))
                throw new ArgumentNullException("queryKey");

            return entityType.FullName.ToLower() + ":" + queryKey;
        }
    }
}
