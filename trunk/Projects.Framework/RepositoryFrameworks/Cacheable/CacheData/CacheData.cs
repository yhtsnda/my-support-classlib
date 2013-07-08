using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 用来提交给 ICache 的缓存数据对象
    /// </summary>
    [Serializable]
    public class CacheData
    {
        public object[] Data;

        public object Convert(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (Data != null)
                return EntityUtil.SetValueForCache(type, Data);

            return null;
        }

        public static CacheData FromEntity(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            CacheData cd = new CacheData()
            {
                Data = EntityUtil.GetValuesForCache(entity)
            };
            return cd;
        }
    }
}
