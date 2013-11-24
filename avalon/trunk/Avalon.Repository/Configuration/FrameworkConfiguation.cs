using Avalon.Framework.Shards;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Configurations
{
    internal class FrameworkConfiguation
    {
        bool inited = false;

        ShardConfiguration shardConfig;
        MetadataConfiguration metadataConfig;
        InterceptorConfiguation interceptorConfig;
        RepositoryConfiguation repositoryConfig;
        ServiceConfiguation serviceConfig;
        CommandConfiguation commandConfig;

        //仓储相关的程序集
        HashSet<Assembly> repositoryAssemblies = new HashSet<Assembly>();
        //服务相关的程序集
        HashSet<Assembly> serviceAssemblies = new HashSet<Assembly>();

        public FrameworkConfiguation()
        {
            CacheKeyGenerator = new DefaultCacheKeyGenerator();
        }

        /// <summary>
        /// 仓储相关的程序集
        /// </summary>
        public IEnumerable<Assembly> RepositoryAssemblies
        {
            get { return repositoryAssemblies; }
        }

        /// <summary>
        /// 方法调用的缓存键生成器
        /// </summary>
        public ICacheKeyGenerator CacheKeyGenerator
        {
            get;
            set;
        }

        public ShardConfiguration ShardConfiguration
        {
            get { return shardConfig; }
        }

        public MetadataConfiguration MetadataConfiguration
        {
            get { return metadataConfig; }
        }

        public InterceptorConfiguation InterceptorConfiguation
        {
            get { return interceptorConfig; }
        }

        public RepositoryConfiguation RepositoryConfiguation
        {
            get { return repositoryConfig; }
        }

        public ServiceConfiguation ServiceConfiguation
        {
            get { return serviceConfig; }
        }

        public CommandConfiguation CommandConfiguation
        {
            get { return commandConfig; }
        }

        public void Configure(IDependencyRegister register)
        {
            var section = ToolSection.Instance;
            if (section == null)
                throw new ConfigurationException("缺少 nd.tool 配直节信息。");

            repositoryAssemblies.UnionWith(LoadAssemblies(section, "repository/assembly"));
            serviceAssemblies.UnionWith(LoadAssemblies(section, "service/assembly"));

            inited = true;
            shardConfig = new ShardConfiguration(section);
            metadataConfig = new MetadataConfiguration(repositoryAssemblies);
            interceptorConfig = new InterceptorConfiguation(section);
            repositoryConfig = new RepositoryConfiguation(repositoryAssemblies, register);
            serviceConfig = new ServiceConfiguation(serviceAssemblies, register);
            commandConfig = new CommandConfiguation(serviceAssemblies, register);

            shardConfig.Load();
            metadataConfig.Load();
            interceptorConfig.Load();
            repositoryConfig.Load();
            serviceConfig.Load();
            commandConfig.Load();
        }

        public void RegisterRepositoryAssemblies(params Assembly[] assemblies)
        {
            CheckUnInited();
            repositoryAssemblies.UnionWith(assemblies);
        }

        public void RegisterServiceAssemblies(params Assembly[] assemblies)
        {
            CheckUnInited();
            serviceAssemblies.UnionWith(assemblies);
        }

        /// <summary>
        /// 获取指定类型的 IShardSessionFactory 实例
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IShardSessionFactory GetSessionFactory(Type entityType, bool valid = false)
        {
            CheckInited();
            return shardConfig.GetSessionFactory(entityType, valid);
        }

        /// <summary>
        /// 注册类型的 IShardSessionFactory 实例
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="shardSessionFactoryType"></param>
        public void RegisterShardSessionFactory(Type entityType, Type shardSessionFactoryType)
        {
            shardConfig.RegisterShardSessionFactory(entityType, shardSessionFactoryType);
        }

        ///// <summary>
        ///// 注册元数据定义
        ///// </summary>
        ///// <param name="assembly"></param>
        //public void RegisterDefineMetadata(Assembly assembly)
        //{
        //    metadataConfig.RegisterDefineMetadata(assembly);
        //}

        /// <summary>
        /// 获取指定类型的的仓储拦截器
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IEnumerable<IRepositoryFrameworkInterceptor> GetInterceptors(Type entityType)
        {
            CheckInited();
            return interceptorConfig.GetInterceptors(entityType);
        }

        /// <summary>
        /// 注册指定类型的仓储拦截器
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="interceptor"></param>
        public void RegisterInterceptor(Type entityType, IRepositoryFrameworkInterceptor interceptor)
        {
            CheckInited();
            interceptorConfig.RegisterInterceptor(entityType, interceptor);
        }

        /// <summary>
        /// 注册仓储拦截器
        /// </summary>
        /// <param name="interceptor"></param>
        public void RegisterInterceptor(IRepositoryFrameworkInterceptor interceptor)
        {
            CheckInited();
            interceptorConfig.RegisterInterceptor(interceptor);
        }

        /// <summary>
        /// 获取指定类型的分区策略
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IShardStrategy GetShardStrategy(Type entityType)
        {
            CheckInited();
            return shardConfig.GetShardStrategy(entityType);
        }

        /// <summary>
        /// 注册类型的分区策略
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="shardStragegyType"></param>
        /// <param name="attributes"></param>
        public IShardStrategy RegisterShardStragety(Type entityType, Type shardStragegyType, IDictionary<string, string> attributes = null)
        {
            CheckInited();
            return shardConfig.RegisterShardStragety(entityType, shardStragegyType, attributes);
        }

        public ClassDefineMetadata GetDefineMetadata(Type entityType)
        {
            CheckInited();
            return metadataConfig.GetDefineMetadata(entityType);
        }

        List<Assembly> LoadAssemblies(ToolSection section, string path)
        {
            var assemblies = new List<Assembly>();

            foreach (var node in section.TryGetNodes(path))
            {
                var assemblyName = node.Attributes.TryGetValue("name");
                try
                {
                    var assembly = Assembly.Load(assemblyName);
                    assemblies.Add(assembly);
                }
                //catch (FileNotFoundException ex)
                //{
                //    //忽略无法加载的程序集
                //}
                catch (Exception ex)
                {
                    throw new AvalonException("无法加载程序集 " + assemblyName, ex);
                }
            }

            return assemblies;
        }

        public void CheckInited()
        {
            if (!inited)
                throw new AvalonException("必须在框架初始化 RepositoryFramework.Config() 后执行该操作");
        }

        public void CheckUnInited()
        {
            if (inited)
                throw new AvalonException("在框架初始化 RepositoryFramework.Config() 后无法执行该操作");
        }
    }
}
