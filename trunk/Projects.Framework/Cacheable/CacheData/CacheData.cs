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
            if (Data != null)
            {
                type = ToCacheType(type);
                var pa = PropertyAccessorFactory.GetPropertyAccess(type);
                object entity = pa.CreateInstance();
                pa.SetPropertyValues(entity, Data);
                return entity;
            }
            return null;
        }

        public static CacheData FromEntity(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var type = ToCacheType(entity.GetType());
            var pa = PropertyAccessorFactory.GetPropertyAccess(type);
            CacheData cd = new CacheData()
            {
                Data = pa.GetPropertyValues(entity)
            };
            return cd;
        }

        static Type ToCacheType(Type type)
        {
            var metadata = RepositoryFramework.GetDefineMetadata(type);
            if (metadata != null)
                return metadata.EntityType;

            return type;
        }
    }
}
