using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    /// <summary>
    /// 对象与视图的映射元数据
    /// </summary>
    internal class QueryEntityMetadata
    {
        public QueryEntityMetadata(Type entityType)
        {
            EntityType = entityType;
            Properties = new List<PropertyMapping>();
        }

        public Type EntityType { get; set; }

        public QueryViewMetadata ViewMetadata { get; set; }

        public List<PropertyMapping> Properties { get; private set; }

        public bool IsSourceEntity { get; private set; }

        public bool IsValid { get; private set; }

        public void Valid(IQuerySourceProvider provider)
        {
            IsSourceEntity = provider.IsSource(EntityType);
            if (!IsSourceEntity)
            {
                var repProperties = Properties.GroupBy(o => o.Property).Where(g => g.Count() > 1).Select(o => o.Key);
                if (repProperties.Count() > 0)
                    throw new QueryDefineException("类型 {0} 的属性 {1} 重复定义。", EntityType.FullName, String.Join(", ", repProperties.Select(o => o.Name)));

                var properties = EntityType.GetProperties().Except(Properties.Select(o => o.Property));
                if (properties.Count() > 0)
                    throw new QueryDefineException("类型 {0} 的属性 {1} 缺少映射定义。", EntityType.FullName, String.Join(", ", properties.Select(o => o.Name)));
            }
            IsValid = true;
        }
    }

}
