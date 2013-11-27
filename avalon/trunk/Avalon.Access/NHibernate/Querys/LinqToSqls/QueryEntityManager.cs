using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalon.Framework.Querys;

namespace Avalon.NHibernateAccess
{
    /// <summary>
    /// ODATA 解析执行过程的数据存储及处理
    /// </summary>
    public class QueryEntityManager
    {
        // 查询相关的实体及别名
        Dictionary<Type, string> entities = new Dictionary<Type, string>();
        QueryEntityMetadata queryMetadata;
        QueryEntityMetadata resultMetadata;
        QueryViewMetadata metadata;
        IQuerySourceProvider provider;
        bool resultIsSource = false;

        public QueryEntityManager(Type queryType, Type resultType = null)
        {
            QueryType = queryType;
            ResultType = resultType;
            this.provider = QueryMetadataProvider.SourceProvider;

            this.queryMetadata = QueryMetadataProvider.GetEntityMetadata(queryType);
            if (resultType != null)
            {
                resultIsSource = provider.IsSource(resultType);
                if (!resultIsSource)
                    this.resultMetadata = QueryMetadataProvider.GetEntityMetadata(resultType);
            }
            this.metadata = queryMetadata.ViewMetadata;
        }

        public Type QueryType { get; private set; }

        public Type ResultType { get; private set; }

        public IQuerySourceProvider Provider
        {
            get { return provider; }
        }

        internal QueryViewMetadata Metadata
        {
            get { return metadata; }
        }

        internal QueryEntityMetadata QueryMetadata
        {
            get { return queryMetadata; }
        }

        public string GetAlias(Type entityType)
        {
            string alias;
            if (!entities.TryGetValue(entityType, out alias))
            {
                alias = RegisterAlias(entityType);
            }
            return alias;
        }

        public string QueryPropertyToColumnName(string name)
        {
            var pm = GetQueryPropertyMapping(name);

            string alis = GetAlias(pm.MappingType);
            return String.Format("{0}.{1}", alis, GetColumnName(pm));
        }

        public string ResultPropertyNameToColumn(string name)
        {
            var pm = GetResultPropertyMapping(name);

            string alis = GetAlias(pm.MappingType);
            return String.Format("{0}.{1}", alis, GetColumnName(pm));
        }

        public string GetColumnName(PropertyMapping propertyMapping)
        {
            return provider.GetColumnName(propertyMapping.MappingType, propertyMapping.MappingProperty);
        }

        public string GetColumnName(Type mappingType, PropertyInfo mappingProperty)
        {
            return provider.GetColumnName(mappingType, mappingProperty);
        }

        public string GetTableName(Type entityType)
        {
            return provider.GetTableName(entityType);
        }

        internal IList<QueryViewJoinMetadata> GetJoins()
        {
            var joinDic = metadata.Joins.ToDictionary(o => o.JoinEntityType);

            var joinHash = new HashSet<Type>();
            ScanJoinTypes(entities.Keys, joinDic, joinHash);
            ScanJoinTypes(metadata.Joins.Where(o => o.JoinType == JoinType.InnerJoin).Select(o => o.JoinEntityType), joinDic, joinHash);

            List<QueryViewJoinMetadata> joins = new List<QueryViewJoinMetadata>();
            foreach (var join in metadata.Joins)
            {
                if (joinHash.Contains(join.JoinEntityType))
                    joins.Add(join);
            }
            return joins;
        }

        void ScanJoinTypes(IEnumerable<Type> joinTypes, Dictionary<Type, QueryViewJoinMetadata> joinDic, HashSet<Type> joinHash)
        {
            foreach (var joinType in joinTypes)
            {
                if (!joinHash.Contains(joinType) && joinType != metadata.EntityType)
                {
                    joinHash.Add(joinType);
                    var join = joinDic.TryGetValue(joinType);
                    ScanJoinTypes(join.ConditionEntityTypes, joinDic, joinHash);
                }
            }
        }

        string RegisterAlias(Type entityType)
        {
            var lc = new string(entityType.Name.Where(o => Char.IsUpper(o)).ToArray()).ToLower();
            if (lc.Length == 0)
                lc = entityType.Name.Substring(0, 1);

            var alis = lc;
            int index = 2;
            while (entities.Values.Any(o => o == alis))
            {
                alis = lc + index;
                index++;
            }
            entities.Add(entityType, alis);
            return alis;
        }

        PropertyMapping GetQueryPropertyMapping(string propertyName)
        {
            var pm = queryMetadata.Properties.FirstOrDefault(o => o.Property.Name == propertyName);
            if (pm == null)
                throw new ArgumentException(String.Format("在对象 {0} 不存在属性 {1}", queryMetadata.EntityType.FullName, propertyName));
            return pm;
        }


        PropertyMapping GetResultPropertyMapping(string propertyName)
        {
            if (resultIsSource)
                throw new ArgumentException(String.Format("返回类型 {0} 为已定义的类型，无需对属性进行解析。", ResultType.FullName));

            var pm = resultMetadata.Properties.FirstOrDefault(o => o.Property.Name == propertyName);
            if (pm == null)
                throw new ArgumentException(String.Format("在对象 {0} 不存在属性 {1}", resultMetadata.EntityType.FullName, propertyName));
            return pm;
        }
    }
}
