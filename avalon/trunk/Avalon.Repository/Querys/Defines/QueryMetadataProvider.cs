using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    public static class QueryMetadataProvider
    {
        static Dictionary<Type, QueryViewMetadata> viewMetadatas = new Dictionary<Type, QueryViewMetadata>();
        static Dictionary<Type, QueryEntityMetadata> entityMetadatas = new Dictionary<Type, QueryEntityMetadata>();
        static IQuerySourceProvider sourceProvider;
        static IQuerySpecificationProvider specificationProvider;

        public static IQuerySourceProvider SourceProvider
        {
            get
            {
                LoadProvider();
                return sourceProvider;
            }
            set { sourceProvider = value; }
        }

        public static IQuerySpecificationProvider SpecificationProvider
        {
            get
            {
                LoadProvider();
                return specificationProvider;
            }
            set { specificationProvider = value; }
        }

        static void LoadProvider()
        {
            if (specificationProvider == null)
            {
                var assembly = Assembly.Load("Nd.Platform.NHibernateAccess");
                if (assembly == null)
                    throw new ArgumentNullException("请先设置 RepositoryFramework.QuerySourceProvider 及　RepositoryFramework.QuerySpecificationProvider 值");

                var specificationProviderType = assembly.GetType("Nd.Platform.NHibernateAccess.NHibernateQuerySpecificationProvider");
                specificationProvider = (IQuerySpecificationProvider)FastActivator.Create(specificationProviderType);
                var sourceProviderType = assembly.GetType("Nd.Platform.NHibernateAccess.NHibernateQuerySourceProvider");
                sourceProvider = (IQuerySourceProvider)FastActivator.Create(sourceProviderType);
            }
        }

        public static void Reset()
        {
            viewMetadatas.Clear();
            entityMetadatas.Clear();
        }

        internal static void Register(QueryViewMetadata viewMetadata)
        {
            viewMetadatas.Add(viewMetadata.ViewType, viewMetadata);
        }

        internal static void Register(QueryEntityMetadata entityMetadata)
        {
            entityMetadatas.Add(entityMetadata.EntityType, entityMetadata);
        }

        internal static QueryEntityMetadata GetEntityMetadata(Type entityType)
        {
            var metadata = entityMetadatas.TryGetValue(entityType);
            if (metadata == null)
                throw new QueryDefineException("不存在视图对象类型 {0} 的定义。", entityType.FullName);

            if (!metadata.IsValid)
                metadata.Valid(SourceProvider);

            return metadata;
        }

        internal static QueryViewMetadata GetMetadata(Type viewType)
        {
            var metadata = viewMetadatas.TryGetValue(viewType);
            if (metadata == null)
                throw new QueryDefineException("不存在视图类型 {0} 的定义。", viewType.FullName);
            return metadata;
        }

    }
}
