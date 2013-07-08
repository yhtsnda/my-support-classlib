using Castle.DynamicProxy;
using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal interface ICacheableRepositoryProvider<TEntity>
        where TEntity : class
    {

        IInvocation Invocation { get; }

        ClassDefineMetadata DefineMetadata { get; }

        string CacheKey { get; }

        bool IsMatch();

        IQueryTimestamp GetCacheData();

        void ProcessCache(IQueryTimestamp cacheData);

        bool ProcessSource();
    }
}
