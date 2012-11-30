using Castle.DynamicProxy;
using Nd.Tool;
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

        public ClassDefineMetadata CacheMetadata
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
        }

        public abstract bool IsMatch();

        public abstract IQueryTimestamp GetCacheData();

        public abstract void ProcessCache(IQueryTimestamp cacheData);

        public abstract void ProcessSource();
    }
}
