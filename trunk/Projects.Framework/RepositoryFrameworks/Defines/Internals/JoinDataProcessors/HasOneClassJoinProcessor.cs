using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    internal class HasOneClassJoinProcessor<TEntity, TJoin> : IClassJoinDataProcessor where TJoin : class
    {
        ClassJoinDefineMetadata metadata;
        Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> specAction;

        public HasOneClassJoinProcessor(Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> specAction, ClassJoinDefineMetadata metadata)
        {
            this.specAction = specAction;
            this.metadata = metadata;
        }

        public TJoin Process(TEntity entity)
        {
            ISpecification<TJoin> specification = SpecificationFactory.Create<TJoin>();
            IRepository<TJoin> repository = DependencyResolver.Resolve<IRepository<TJoin>>();
            if (repository == null)
                throw new PlatformException("类型 {0} 未实现仓储。", typeof(TJoin).FullName);
            specification = specAction(entity, specification);

            //如果为主键的获取表达式，则使用Get方式。
            object id;
            if (IsIdentityExpression(specification.CriteriaExpression, out id))
                return repository.Get(specification.ShardParams, id);

            ///缓存查询
            if (metadata.JoinCache.IsCacheable)
                return repository
                    .Cache()
                    .Depend(metadata.JoinCache.GetCacheRegions(entity))
                    .Proxy()
                    .FindOne(specification);
            return repository.FindOne(specification);
        }

        object IClassJoinDataProcessor.Process(object entity)
        {
            return Process((TEntity)entity);
        }

        bool IsIdentityExpression(Expression<Func<TJoin, bool>> expression, out object value)
        {
            value = null;
            var metadata = RepositoryFramework.GetDefineMetadata(typeof(TJoin));
            if (metadata == null || metadata.IdMember == null)
                return false;

            var valider = new IdentityExpressionValider(metadata.IdMember, expression);

            value = valider.Value;
            return valider.IsValid;
        }
    }

    internal class IdentityExpressionValider : ExpressionVisitor
    {
        bool isValid = false;
        bool hasMember = false;
        bool hasConstant = false;

        object value = null;
        MemberInfo member;

        public IdentityExpressionValider(MemberInfo member, Expression expression)
        {
            this.member = member;
            this.Visit(expression);
        }

        public bool IsValid
        {
            get { return isValid; }
        }

        public object Value
        {
            get { return value; }
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Equal)
            {
                bool leftHasMember, leftHasConstant, rightHasMember, rightHasConstant;
                Visit(node.Left);
                leftHasMember = hasMember;
                leftHasConstant = hasConstant;

                Visit(node.Right);
                rightHasMember = hasMember;
                rightHasConstant = hasConstant;

                if (leftHasMember && rightHasConstant || leftHasConstant && rightHasMember)
                {
                    value = GetExpressionValue(leftHasConstant ? node.Left : node.Right);
                    isValid = true;
                }
                return node;
            }
            isValid &= false;
            return base.VisitBinary(node);
        }

        object GetExpressionValue(Expression expr)
        {
            var convertExpr = Expression.Convert(expr, typeof(object));
            var getterExpr = Expression.Lambda<Func<object>>(convertExpr);
            var getter = getterExpr.Compile();
            return getter();
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member == member)
                hasMember = true;

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            hasConstant = true;
            return base.VisitConstant(node);
        }
    }
}
