using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

using Projects.Tool.Util;

namespace Projects.Accesses.Builders
{
    public abstract class WhereClauseBuilder<TEntity> : ExpressionVisitor, IWhereClauseBuilder<TEntity>
        where TEntity : class, new()
    {
        private readonly StringBuilder mBuilder = new StringBuilder();
        private readonly Dictionary<string, object> mParameterValues = new Dictionary<string, object>();
        private readonly IStorageMappingResolver<TEntity> mStorageMappingResolver = null;
        private bool mStartWith = false;
        private bool mEndWith = false;
        private bool mContains = false;

        protected virtual string And { get { return "And"; } }

        protected virtual string Or { get { return "Or"; } }

        protected virtual string Equal { get { return "="; } }

        protected virtual string Not { get { return "Not"; } }

        protected virtual string NotEqual { get { return "<>"; } }

        protected virtual string Like { get { return "Like"; } }

        protected virtual string LikeSymbol { get { return "%"; } }

        protected internal abstract char ParameterChar { get; }

        #region 构造函数
        public WhereClauseBuilder(IStorageMappingResolver<TEntity> mappingResolver)
        {
            this.mStorageMappingResolver = mappingResolver;
        }
        #endregion

        protected override Expression VisitBinary(BinaryExpression node)
        {
            string operate = String.Empty;
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    operate = "+";
                    break;
                case ExpressionType.AddChecked:
                    operate = "+";
                    break;
                case ExpressionType.AndAlso:
                    operate = this.And;
                    break;
                case ExpressionType.Divide:
                    operate = "/";
                    break;
                case ExpressionType.Equal:
                    operate = this.Equal;
                    break;
                case ExpressionType.GreaterThan:
                    operate = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    operate = ">=";
                    break;
                case ExpressionType.LessThan:
                    operate = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    operate = "<=";
                    break;
                case ExpressionType.Modulo:
                    operate = "%";
                    break;
                case ExpressionType.Multiply:
                    operate = "*";
                    break;
                case ExpressionType.MultiplyChecked:
                    operate = "*";
                    break;
                case ExpressionType.Not:
                    operate = this.Not;
                    break;
                case ExpressionType.NotEqual:
                    operate = this.NotEqual;
                    break;
                case ExpressionType.OrElse:
                    operate = this.Or;
                    break;
                case ExpressionType.Subtract:
                    operate = "-";
                    break;
                case ExpressionType.SubtractChecked:
                    operate = "-";
                    break;
                default:
                    throw new NotSupportedException("无法支持的数据操作");
            }
            mBuilder.Append("(");
            Visit(node.Left);
            mBuilder.Append(" ");
            mBuilder.Append(operate);
            mBuilder.Append(" ");
            Visit(node.Right);
            mBuilder.Append(")");
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == typeof(TEntity) ||
                typeof(TEntity).IsSubclassOf(node.Member.DeclaringType))
            {
                string fieldName = this.mStorageMappingResolver
                    .ResolveFieldName(node.Member.Name);
                mBuilder.Append(fieldName);
            }
            else
            {
                if (node.Member is MemberInfo)
                {
                    ConstantExpression ce = node.Expression as ConstantExpression;
                    PropertyInfo fi = node.Member as PropertyInfo;
                    object fieldValue = fi.GetValue(ce.Value, null);
                    Expression constantExpr = Expression.Constant(fieldValue);
                    Visit(constantExpr);
                }
                else
                    throw new NotSupportedException();
            }
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            string paramName = String.Format("{0}{1}", ParameterChar, StringUtil.GetUniqueIdentifier(5));
            mBuilder.Append(paramName);
            if (!mParameterValues.ContainsKey(paramName))
            {
                object v = null;
                if(mStartWith && node.Value is String)
                {
                    mStartWith = false;
                    v = node.Value.ToString() + LikeSymbol;
                }
                else if (mEndWith && node.Value is String)
                {
                    mEndWith = false;
                    v = LikeSymbol + node.Value.ToString();
                }
                else if (mContains && node.Value is String)
                {
                    mContains = false;
                    v = LikeSymbol + node.Value.ToString() + LikeSymbol;
                }
                else
                {
                    v = node.Value;
                }
                mParameterValues.Add(paramName, v);
            }
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            mBuilder.Append("(");
            Visit(node.Object);
            if (node.Arguments == null || node.Arguments.Count != 1)
                throw new NotSupportedException();
            Expression expr = node.Arguments[0];
            switch (node.Method.Name)
            {
                case "StartsWith":
                    mStartWith = true;
                    mBuilder.Append(" ").Append(Like).Append(" ");
                    break;
                case "EndsWith":
                    mEndWith = true;
                    mBuilder.Append(" ").Append(Like).Append(" ");
                    break;
                case "Equals":
                    mBuilder.Append(" ").Append(Equal).Append(" ");
                    break;
                case "Contains":
                    mContains = true;
                    mBuilder.Append(" ").Append(Like).Append(" ");
                    break;
                default:
                    throw new NotSupportedException();
            }
            if (expr is ConstantExpression || expr is MemberExpression)
                Visit(expr);
            else
                throw new NotSupportedException();
            mBuilder.Append(")");
            return node;
        }

        #region 基类无需实现的重写方法
        protected override Expression VisitBlock(BlockExpression node)
        {
            throw new NotSupportedException();
        }
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitDefault(DefaultExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitDynamic(System.Linq.Expressions.DynamicExpression node)
        {
            throw new NotSupportedException();
        }
        protected override ElementInit VisitElementInit(ElementInit node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitGoto(GotoExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitExtension(Expression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitIndex(IndexExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitLabel(LabelExpression node)
        {
            throw new NotSupportedException();
        }
        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitListInit(ListInitExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitLoop(LoopExpression node)
        {
            throw new NotSupportedException();
        }
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            throw new NotSupportedException();
        }
        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            throw new NotSupportedException();
        }
        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            throw new NotSupportedException();
        }
        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitNew(NewExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            throw new NotSupportedException();
        }
        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitTry(TryExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            throw new NotSupportedException();
        }
        protected override Expression VisitUnary(UnaryExpression node)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region IWhereClauseBuilder接口的实现
        public WhereBuilderResult BuildWhereClause(Expression<Func<TEntity, bool>> expression)
        {
            this.mBuilder.Clear();
            this.mParameterValues.Clear();
            this.Visit(expression.Body);
            WhereBuilderResult result = new WhereBuilderResult(mBuilder.ToString(), mParameterValues);
            return result;
        }
        #endregion
    }
}
