using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    internal class CacheDomainUtil
    {
        public static string CreateCacheName<TEntity>()
        {
            return GetTypeName(typeof(TEntity));
        }

        public static string CreateCacheName<TEntity, TKey>()
        {
            return GetTypeName(typeof(TEntity));
        }

        public static string CreateCacheName<TEntity, TParam, TKey>()
        {
            return GetTypeName(typeof(TEntity)) + ":" + GetTypeName(typeof(TParam));
        }

        public static string CreateCacheName<TEntity, TParam1, TParam2, TKey>()
        {
            return GetTypeName(typeof(TEntity)) + ":" + GetTypeName(typeof(TParam1)) + ":" + GetTypeName(typeof(TParam2));
        }

        public static string CreateCacheKeyFormat<TEntity>()
        {
            return CreateCacheName<TEntity>();
        }

        public static string CreateCacheKeyFormat<TEntity, TKey>()
        {
            return CreateCacheName<TEntity, TKey>() + ":{0}";
        }

        public static string CreateCacheKeyFormat<TEntity, TParam, TKey>()
        {
            return CreateCacheName<TEntity, TParam, TKey>() + ":{0}:{1}";
        }

        public static string CreateCacheKeyFormat<TEntity, TParam1, TParam2, TKey>()
        {
            return CreateCacheName<TEntity, TParam1, TParam2, TKey>() + ":{0}:{1}:{2}";
        }

        static string GetTypeName(Type type)
        {
            if (type.IsGenericType)
                return type.FullName + ":" + String.Join(":", type.GetGenericArguments().Select(o => o.Name));
            return type.FullName;
        }
    }
}
