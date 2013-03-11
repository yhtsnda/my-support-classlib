using Castle.DynamicProxy;
using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal abstract class AbstractCacheableRepositoryProvider<TEntity> : ICacheableRepositoryProvider<TEntity>
        where TEntity : class
    {
        IInvocation invocation;
        ClassDefineMetadata metadata;
        string cacheKey;

        public IInvocation Invocation
        {
            get { return invocation; }
            set { invocation = value; }
        }

        public ClassDefineMetadata DefineMetadata
        {
            get
            {
                if (metadata == null)
                {
                    var entityType = ReflectionHelper.GetEntityTypeFromRepositoryType(invocation.TargetType);
                    metadata = RepositoryFramework.GetDefineMetadata(entityType);
                }
                return metadata;
            }
        }

        public string CacheKey
        {
            get
            {
                if (cacheKey == null)
                    cacheKey = RepositoryFramework.CacheKeyGenerator.CreateCacheKey(invocation.Method, invocation.Arguments);
                return cacheKey;
            }
            set
            {
                cacheKey = value;
            }
        }

        public abstract bool IsMatch();

        public abstract IQueryTimestamp GetCacheData();

        public abstract void ProcessCache(IQueryTimestamp cacheData);

        /// <summary>
        /// 处理原始数据
        /// </summary>
        /// <returns>数据源是否具备数据</returns>
        public abstract bool ProcessSource();
    }
}
