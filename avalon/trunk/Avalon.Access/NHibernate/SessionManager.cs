using Avalon.Utility;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using Avalon.Utility;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Stat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalon.Framework;
using Avalon.Framework.Shards;
using Avalon.Profiler;

namespace Avalon.NHibernateAccess
{
    internal class SessionManager : AbstractConnectionManager<ISessionFactory>
    {
        const string SessionCacheKey = "_SessionCacheKey_";

        public SessionManager()
        {
            EntityUtil.OriginalObjectProvider = new NHibernateOriginalObjectProvider();
            LoggerProvider.SetLoggersFactory(new ProfilterLoggerFactory());
        }

        public ISession OpenSession(ShardId shardId, PartitionId partitionId)
        {
            var factory = GetConnection(shardId);
            return OpenSession(factory, shardId, partitionId);
        }
     
        protected override ISessionFactory CreateConnection(string connStr)
        {
            var mysqlConfig = MySQLConfiguration.Standard
                .ConnectionString(connStr)
                .AdoNetBatchSize(500)
                .Dialect<MySQL5Dialect>();

            List<Assembly> assemblies = SortAssemblyByDepend(RepositoryFramework.RepositoryAssemblies);

            var fluentConfig = Fluently.Configure().Database(mysqlConfig)
               .ProxyFactoryFactory<NHibernate.Bytecode.DefaultProxyFactoryFactory>()
               .CurrentSessionContext<NHibernate.Context.ManagedWebSessionContext>()
               .Mappings(p =>
               {
                   foreach (var assmebly in assemblies)
                   {
                       p.FluentMappings.AddFromAssembly(assmebly).Conventions.Add(DynamicInsert.AlwaysTrue(), DynamicUpdate.AlwaysTrue());
                   }
                   //p.FluentMappings.ExportTo(@"C:\hmt");
               });

            Configuration cfg;
            try
            {
                cfg = fluentConfig.BuildConfiguration();
                cfg.SetProperty("generate_statistics", "true");
            }
            catch (Exception ex)
            {
                throw new SystemException("创建 NHibernate Configuration 发生错误：" + ex.Message, ex);
            }

            var factory = cfg.BuildSessionFactory();
            var statisticsField = typeof(SessionFactoryImpl).GetField("statistics", BindingFlags.Instance | BindingFlags.NonPublic);
            var statistics = (StatisticsImpl)statisticsField.GetValue(factory);
            var wrapper = new StatisticsImplWapper(statistics);
            wrapper.IsStatisticsEnabled = statistics.IsStatisticsEnabled;
            statisticsField.SetValue(factory, wrapper);
            return factory;
        }

        ISession OpenSession(ISessionFactory factory, ShardId shardId, PartitionId partitionId)
        {
            var session = WorkbenchUtil<ShardId, ISession>.GetValue(SessionCacheKey, shardId);

            //检测同一个session是否有相同的逻辑表但真实表不一致的情况
            if (session != null)
            {
                var interceptor = (ShardInterceptor)((ISessionImplementor)session).Interceptor;
                if (!interceptor.Register(partitionId))
                    session = null;
            }

            if (session == null)
            {
                Workbench.Current.AttachDisposeHandler(DisposeSessions);
                using (ProfilerContext.Watch("open nhibernate inner session"))
                {
                    session = factory.OpenSession(new ShardInterceptor(partitionId));
                }
                WorkbenchUtil<ShardId, ISession>.SetValue(SessionCacheKey, shardId, session);
            }
            if (TransactionScope.IsScope && !TransactionScope.IsLinkWidhTranscation(session))
            {
                TransactionScope.Push(new NHibernateTranscation(session.BeginTransaction()), session);
            }
            return session;
        }

        void DisposeSessions(Workbench workbench)
        {
            var sessions = WorkbenchUtil<ShardId, ISession>.GetValues(SessionCacheKey);
            foreach (var session in sessions)
            {
                session.Dispose();
            }
            WorkbenchUtil<ShardId, ISession>.Clear(SessionCacheKey);
        }

        /// <summary>
        /// 给程序集列表排序，让继承classmapping类的排到最后，如果含有继承classmapping类的早于普通实体定义先加入nhibernate中会发生异常
        /// </summary>
        /// <param name="assemblys"></param>
        /// <returns></returns>
        List<Assembly> SortAssemblyByDepend(IEnumerable<Assembly> assemblies)
        {
            Dictionary<string, AssemblyDepend> dic = assemblies.ToDictionary(o => o.FullName, o => new AssemblyDepend() { Assembly = o });

            foreach (var assembly in assemblies)
            {
                dic[assembly.FullName].Depends = assembly.GetReferencedAssemblies()
                    .Where(o => dic.ContainsKey(o.FullName))
                    .Select(o => dic[o.FullName]).ToList();
            }

            var depends = dic.Values.Cast<AssemblyDepend>().OrderBy(o => o.DependCount);
            return depends.Select(o => o.Assembly).ToList();
        }

        class AssemblyDepend
        {
            public Assembly Assembly;

            public List<AssemblyDepend> Depends;

            public int DependCount
            {
                get
                {
                    return Depends.Count + Depends.Sum(o => o.DependCount);
                }
            }
        }

        class NHibernateTranscation : Avalon.Framework.ITransaction
        {
            NHibernate.ITransaction innerTran;

            public NHibernateTranscation(NHibernate.ITransaction innerTran)
            {
                this.innerTran = innerTran;
            }

            public void Commit()
            {
                innerTran.Commit();
            }

            public void Rollback()
            {
                innerTran.Rollback();
            }
        }
    }
}
