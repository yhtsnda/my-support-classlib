using Avalon.Framework;
using Avalon.Framework.Querys;
using Avalon.Utility;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.NHibernateAccess
{
    public class NHibernateQuerySourceProvider : IQuerySourceProvider
    {
        public bool IsSource(Type entityType)
        {
            return GetClassMetadata(entityType) != null;
        }

        public string GetIdentityName(Type entityType)
        {
            var persister = GetClassMetadata(entityType);
            if (persister == null)
                throw new ArgumentException(String.Format("给定的类型为进行 NHibernate 映射定义 {0}", entityType.FullName));
            return persister.IdentifierColumnNames[0];
        }

        public string GetTableName(Type entityType)
        {
            var persister = GetClassMetadata(entityType);
            if (persister == null)
                throw new ArgumentException(String.Format("给定的类型为进行 NHibernate 映射定义 {0}", entityType.FullName));
            return persister.TableName;
        }

        public string GetColumnName(Type entityType, PropertyInfo property)
        {
            var persister = GetClassMetadata(entityType);
            if (persister == null)
                throw new ArgumentException(String.Format("给定的类型为进行 NHibernate 映射定义 {0}", entityType.FullName));

            return persister.ToColumns(property.Name).First();
        }

        AbstractEntityPersister GetClassMetadata(Type entityType)
        {
            var factory = (NHibernateShardSessionFactory)RepositoryFramework.GetSessionFactory(entityType);
            return factory.GetSessionFactory(entityType).GetClassMetadata(entityType) as AbstractEntityPersister;
        }
    }
}
