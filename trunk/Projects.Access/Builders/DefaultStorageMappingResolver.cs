using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Projects.Framework;

namespace Projects.Accesses.Builders
{
    /// <summary>
    /// 默认的存储映射解析器
    /// </summary>
    public sealed class DefaultStorageMappingResolver<TEntity> : 
        IStorageMappingResolver<TEntity> where TEntity : class,new()
    {
        /// <summary>
        /// 解析数据表
        /// </summary>
        /// <returns></returns>
        public string ResolveTableName()
        {
            Type entityType = typeof(TEntity);
            var attrs = entityType.GetCustomAttributes(typeof(TableNameAttribute), false);
            if (attrs.Length == 1)
            {
                var attr = (TableNameAttribute)attrs[0];
                if (attr.ContainSchema)
                    return String.Format("{0}.{1}", attr.SchemaName, attr.Table);
                return attr.Table;
            }
            //如果不包含
            return entityType.Name;
        }

        public bool IsAutoIdentityField(string propertyName)
        {
            Type entityType = typeof(TEntity);
            var pi = entityType.GetProperty(propertyName);
            if (pi != null)
            {
                var attrs = pi.GetCustomAttributes(typeof(ColumnNameAttribute), false);
                if (attrs.Length == 1)
                {
                    var attr = (ColumnNameAttribute)attrs[0];
                    return attr.IsAutoIdentity;
                }
                return false;
            }
            return false;
        }

        public string ResolveFieldName(string propertyName)
        {
            Type entityType = typeof(TEntity);
            var pi= entityType.GetProperty(propertyName);
            if(pi != null)
            {
                var attrs = pi.GetCustomAttributes(typeof(ColumnNameAttribute), false);
                if (attrs.Length == 1)
                {
                    var attr = (ColumnNameAttribute)attrs[0];
                    return attr.Column;
                }
                return propertyName;
            }
            return propertyName;
        }
    }
}
