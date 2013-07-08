using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Event;

using Projects.Tool;
using Projects.Tool.Shards;
using Projects.Tool.Util;
using Projects.Framework;

namespace Projects.Framework.NHibernateAccess
{
    internal class SessionManager
    {
        const string SessionCacheKey = "_SessionCacheKey_";

        Dictionary<ShardId, ISessionFactory> sessionFactories;
        object syscRoot = new object();

        public SessionManager()
        {
            EntityUtil.OriginalObjectProvider = new NHibernateOriginalObjectProvider();
            LoggerProvider.SetLoggersFactory(new ProfilterLoggerFactory());
            sessionFactories = new Dictionary<ShardId, ISessionFactory>();
        }

        public ISession OpenSession(ShardId shardId, PartitionId partitionId)
        {
            var factory = GetSessionFactory(shardId);
            return OpenSession(factory, shardId, partitionId);
        }

        ISession OpenSession(ISessionFactory factory, ShardId shardId, PartitionId partitionId)
        {
            var workbench = Workbench.Current;
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
                workbench.AttachDisposeHandler(DisposeSessions);
                session = factory.OpenSession(new ShardInterceptor(partitionId));
                WorkbenchUtil<ShardId, ISession>.SetValue(SessionCacheKey, shardId, session);
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

        ISessionFactory GetSessionFactory(ShardId shardId)
        {
            ISessionFactory factory = sessionFactories.TryGetValue(shardId);
            if (factory == null)
            {
                lock (syscRoot)
                {
                    factory = sessionFactories.TryGetValue(shardId);
                    if (factory == null)
                    {
                        factory = CreateSessionFactory(shardId);
                        sessionFactories.Add(shardId, factory);
                    }
                }
            }
            return factory;
        }

        ISessionFactory CreateSessionFactory(ShardId shardId)
        {
            var shardNodes = ToolSection.Instance.TryGetNodes("shard/shardIds/shardId");
            var shardNode = shardNodes.FirstOrDefault(o => o.Attributes.TryGetValue("id") == shardId.Id);
            if (shardNode == null)
                throw new MissConfigurationException(ToolSection.Instance.RootNode, "shard/shardIds/shardId");

            var connectionName = shardNode.Attributes.TryGetValue("connectionName");
            if (String.IsNullOrEmpty(connectionName))
                throw new ArgumentNullException("必须为分区 " + shardId.Id + " 指定数据源链接的名称路径 shard/shardIds/shardId/connectionName ");

            var mysqlConfig = MySQLConfiguration.Standard
                .ConnectionString(p => p.FromConnectionStringWithKey(connectionName))
                .AdoNetBatchSize(500)
                .Dialect<MySQL5Dialect>();

            if (shardNode.TryGetValue("showSql") == "true")
                mysqlConfig = mysqlConfig.ShowSql().FormatSql();

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

                //var raiseEvent = new RaiseEventListener();
                //cfg.AppendListeners(ListenerType.PreDelete, new IPreDeleteEventListener[] { raiseEvent });
                //cfg.AppendListeners(ListenerType.PreUpdate, new IPreUpdateEventListener[] { raiseEvent });
                //cfg.AppendListeners(ListenerType.PreDelete, new IPreDeleteEventListener[] { raiseEvent });
                //cfg.AppendListeners(ListenerType.PostInsert, new IPostInsertEventListener[] { raiseEvent });
                //cfg.AppendListeners(ListenerType.PostUpdate, new IPostUpdateEventListener[] { raiseEvent });
                //cfg.AppendListeners(ListenerType.PostDelete, new IPostDeleteEventListener[] { raiseEvent });
                //cfg.AppendListeners(ListenerType.PostLoad, new IPostLoadEventListener[] { raiseEvent });
            }
            catch (Exception ex)
            {
                throw new SystemException("创建 NHibernate Configuration 发生错误：" + ex.Message, ex);
            }

            return cfg.BuildSessionFactory();
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
    }
}
