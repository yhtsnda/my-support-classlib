using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Warehouse.StoragePolicy.Properties;

namespace Warehouse.DataOperator
{
    public class WhereClauseBuilder<TDataObject> : ExpressionVisitor, IWhereClauseBuilder<TDataObject>
    {

        public WhereClauseBuilder(IStorageMappingResolver mappingResolver)
        {
            this.mappingResolver = mappingResolver;
        }

        /// <summary>
        /// 获取一个<c>System.String</c>值,用于表示并操作的关键字
        /// </summary>
        protected virtual string And
        {
            get { return "AND"; }
        }

        /// <summary>
        /// 获取一个<c>System.String</c>值,用于表示或操作的关键字
        /// </summary>
        protected virtual string Or
        {
            get { return "OR"; }
        }

        /// <summary>
        /// 获取一个<c>System.String</c>值,用于表示等于操作的关键字
        /// </summary>
        protected virtual string Equal
        {
            get { return "="; }
        }

        /// <summary>
        /// 获取一个<c>System.String</c>值,用于表示NOT操作的关键字
        /// </summary>
        protected virtual string Not
        {
            get { return "NOT"; }
        }

        /// <summary>
        /// 获取一个<c>System.String</c>值,用于表示不等于操作的关键字
        /// </summary>
        protected virtual string NotEqual
        {
            get { return "<>"; }
        }

        /// <summary>
        /// 获取一个<c>System.String</c>值,用于表示相似查询的关键字
        /// </summary>
        protected virtual string Like
        {
            get { return "LIKE"; }
        }

        /// <summary>
        /// 获取一个<c>System.String</c>值,用于表示相识查询的符合
        /// </summary>
        protected virtual string LikeSymbol
        {
            get { return "%"; }
        }

        /// <summary>
        /// 获取一个<c>System.String</c>值,用于表示参数的符合
        /// </summary>
        protected internal abstract char ParameterChar { get; }

        public WhereClauseBuildResult BuildWhereClause(Expression<Func<TDataObject, bool>> expression)
        {
            throw new NotImplementedException();
        }

        private void Out(string s)
        {
            throw new System.NotImplementedException();
        }

        private void OutMember(Expression expression, MemberInfo memberInfo)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.BinaryExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            string str;
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    str = "+";
                    break;
                case ExpressionType.AddChecked:
                    str = "+";
                    break;
                case ExpressionType.AndAlso:
                    str = this.And;
                    break;
                case ExpressionType.Divide:
                    str = "/";
                    break;
                case ExpressionType.Equal:
                    str = this.Equal;
                    break;
                case ExpressionType.GreaterThan:
                    str = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    str = ">=";
                    break;
                case ExpressionType.LessThan:
                    str = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    str = "<=";
                    break;
                case ExpressionType.Modulo:
                    str = "%";
                    break;
                case ExpressionType.Multiply:
                    str = "*";
                    break;
                case ExpressionType.MultiplyChecked:
                    str = "*";
                    break;
                case ExpressionType.Not:
                    str = this.Not;
                    break;
                case ExpressionType.NotEqual:
                    str = this.NotEqual;
                    break;
                case ExpressionType.OrElse:
                    str = this.Or;
                    break;
                case ExpressionType.Subtract:
                    str = "-";
                    break;
                case ExpressionType.SubtractChecked:
                    str = "-";
                    break;
                default:
                    throw new NotSupportedException(string.Format(Resources.EX_EXPRESSION_NODE_TYPE_NOT_SUPPORT, node.NodeType.ToString()));
            }

            Out("(");
            Visit(node.Left);
            Out(" ");
            Out(str);
            Out(" ");
            Visit(node.Right);
            Out(")");
            return node;
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == typeof(TDataObject) ||
                            typeof(TDataObject).IsSubclassOf(node.Member.DeclaringType))
            {
                string mappedFieldName = mappingResolver.ResolveFieldName<TDataObject>(node.Member.Name);
                Out(mappedFieldName);
            }
            else
            {
                if (node.Member is FieldInfo)
                {
                    ConstantExpression ce = node.Expression as ConstantExpression;
                    FieldInfo fi = node.Member as FieldInfo;
                    object fieldValue = fi.GetValue(ce.Value);
                    Expression constantExpr = Expression.Constant(fieldValue);
                    Visit(constantExpr);
                }
                else
                    throw new NotSupportedException(string.Format(Resources.EX_MEMBER_TYPE_NOT_SUPPORT, node.Member.GetType().FullName));
            }
            return node;
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitDefault(DefaultExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitDynamic(DynamicExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override ElementInit VisitElementInit(ElementInit node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitExtension(Expression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitGoto(GotoExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitIndex(IndexExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitLabel(LabelExpression node)
        {
            throw new System.NotImplementedException();
        }

        protected override Expression VisitLambda(Expression<T> node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitListInit(ListInitExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitLoop(LoopExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitNew(NewExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitTry(TryExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            throw new System.NotImplementedException();
        }
    }
}
