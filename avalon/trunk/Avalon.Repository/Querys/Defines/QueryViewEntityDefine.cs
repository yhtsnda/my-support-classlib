using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class QueryViewEntityDefine<TEntity>
    {
        QueryViewMetadata viewMetadata;

        internal QueryViewEntityDefine(QueryViewMetadata viewMetadata)
        {
            this.viewMetadata = viewMetadata;
            Metadata = new QueryEntityMetadata(typeof(TEntity));
            Metadata.ViewMetadata = viewMetadata;
        }

        internal QueryEntityMetadata Metadata
        {
            get;
            private set;
        }

        public QueryViewEntityDefine<TEntity> Map<TKey>(Expression<Func<TEntity, TKey>> entityMemberExp, Expression<Func<TKey>> mappingMemberExp)
        {
            Type entityType;
            viewMetadata.ValidAlias(mappingMemberExp, out entityType);
            Metadata.Properties.Add(new PropertyMapping()
            {
                Property = GetProperty(entityMemberExp),
                MappingProperty = GetProperty(mappingMemberExp),
                MappingType = entityType
            });
            return this;
        }

        PropertyInfo GetProperty(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
                expression = ((LambdaExpression)expression).Body;

            MemberExpression memberExpression = null;
            if (expression.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression as MemberExpression;
            }

            if (memberExpression == null)
            {
                return null;
            }

            return (PropertyInfo)memberExpression.Member;
        }
    }
}
