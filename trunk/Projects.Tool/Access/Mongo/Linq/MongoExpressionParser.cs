using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections;

namespace Projects.Tool.MongoAccess
{
    internal partial class MongoExpressionParser : ExpressionVisitor
    {
        private Expression mExpression;
        private QueryParserResult mResult;
        Stack<QueryComplete> mQueryStack;
        SortByBuilder mSort = new SortByBuilder();
        UpdateBuilder mUpdate = new UpdateBuilder();

        public MongoExpressionParser(Expression expression)
        {
            this.mExpression = expression;
        }

        public object Execute()
        {
            ParseExpression();
            return ExecuteResult();
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.NodeType == ExpressionType.Constant && node.Type.IsGenericType)
            {
                var v = node.Type.GetGenericArguments()[0];
                mResult.EntityType = v;
            }
            return node;
        }

        private void ParseExpression()
        {
            if (mResult == null)
            {
                mResult = new QueryParserResult();
                mQueryStack = new Stack<QueryComplete>();

                Visit(mExpression);
                QueryComplete query = null;
                if (mQueryStack.Count > 0)
                {
                    query = mQueryStack.Pop();
                    while (mQueryStack.Count > 0)
                    {
                        query = Query.And(query, mQueryStack.Pop());
                    }
                }
                mResult.Query = query;
                mResult.SortBy = mSort;
                mResult.Update = mUpdate;
                if (mResult.MethodCall == MethodCall.NoSet)
                    mResult.MethodCall = MethodCall.Query;
            }
        }

        private object ExecuteResult()
        {
            MongoCollection coll = MongoManager.GetCollection(mResult.EntityType);
            if (coll == null)
            {
                throw new ArgumentNullException("coll");
            }

            switch (mResult.MethodCall)
            {
                case MethodCall.NoSet:
                    break;
                case MethodCall.Query:
                case MethodCall.First:
                case MethodCall.FirstOrDefault:
                    if (mResult.Select != null)
                    {
                        mResult.EntityType = mResult.Select.Type;
                        BsonSerializer.RegisterSerializer(mResult.Select.Type, new NewSerializer());
                    }
                    MongoCursor cursor = null;
                    if (mResult.Query == null)
                        cursor = coll.FindAllAs(mResult.EntityType);
                    else
                        cursor = coll.FindAs(mResult.EntityType, mResult.Query);
                    cursor.SetSortOrder(mResult.SortBy);

                    if (mResult.Skip != 0)
                        cursor.SetSkip(mResult.Skip);
                    if (mResult.Take != 0)
                        cursor.SetLimit(mResult.Take);
                    if (mResult.Select != null)
                        cursor.SetFields(mResult.Select.Members.Select(o => o.Name == "Id" ? "_id" : o.Name).ToArray());
                    if (mResult.MethodCall == MethodCall.First || mResult.MethodCall == MethodCall.FirstOrDefault)
                    {
                        foreach (var item in cursor)
                        {
                            return item;
                        }
                        if (mResult.MethodCall == MethodCall.First)
                            throw new InvalidOperationException("未找到指定的项目");
                        return null;
                    }
                    return cursor;
                case MethodCall.Count:
                    return (int)coll.Count(mResult.Query);
                case MethodCall.Update:
                    coll.Update(mResult.Query, mResult.Update, UpdateFlags.Multi);
                    return true;
                case MethodCall.Delete:
                    coll.Remove(mResult.Query);
                    return true;
                default:
                    break;
            }
            throw new NotSupportedException();
        }

        private void ProcessWhere(MethodCallExpression node)
        {
            Visit(((LambdaExpression)StripQuotes(node.Arguments[1])).Body);
        }

        private void ProcessCondition(ExpressionType type, string name, BsonValue value)
        {
            QueryComplete query = null;
            switch (type)
            {
                case ExpressionType.Equal:
                    query = Query.EQ(name, value);
                    break;
                case ExpressionType.NotEqual:
                    query = Query.NE(name, value);
                    break;
                case ExpressionType.GreaterThan:
                    query = Query.GT(name, value);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    query = Query.GTE(name, value);
                    break;
                case ExpressionType.LessThan:
                    query = Query.LT(name, value);
                    break;
                case ExpressionType.LessThanOrEqual:
                    query = Query.LTE(name, value);
                    break;
            }
            mQueryStack.Push(query);
        }

        private void ProcessCondition(ExpressionType type)
        {
            QueryComplete left = mQueryStack.Pop();
            QueryComplete right = mQueryStack.Pop();
            QueryComplete query = null;
            switch (type)
            {
                case ExpressionType.AndAlso:
                    query = Query.And(left, right);
                    break;
                case ExpressionType.OrElse:
                    query = Query.Or(left, right);
                    break;
            }
            mQueryStack.Push(query);
        }

        private void ProcessContains(MethodCallExpression node, bool not)
        {
            IList<BsonValue> values = new List<BsonValue>();
            IEnumerable array = null;
            if (node.Object == null)
                array = (IEnumerable)GetExpressionValue(node.Arguments[0]);
            else
                array = (IEnumerable)GetMemberValue((MemberExpression)node.Object);

            foreach (object item in array)
            {
                values.Add(BsonValue.Create(item));
            }
            string name = node.Object == null ? GetMemberName(node.Arguments[1]) : GetMemberName(node.Arguments[0]);
            BsonArray value = BsonArray.Create(values);

            QueryComplete query = null;
            if (not)
                query = Query.NotIn(name, value);
            else
                query = Query.In(name, value);
            mQueryStack.Push(query);
        }

        private string GetMemberName(Expression node)
        {
            var expr = node;
            if (expr.NodeType == ExpressionType.Quote)
                expr = ((LambdaExpression)StripQuotes(expr)).Body;

            if (expr.NodeType == ExpressionType.Convert)
                expr = ((UnaryExpression)expr).Operand;

            if (expr.NodeType != ExpressionType.MemberAccess)
                throw new ParserException("当前的必须为成员访问表达式", expr);
            string name = ((MemberExpression)expr).Member.Name;

            var type = (((MemberExpression)expr).Expression).Type;
            var classMap = BsonClassMap.LookupClassMap(type);
            var mm = classMap.GetMemberMap(name);
            if (mm != null)
                name = mm.ElementName;

            if (name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                name = "_id";
            return name;
        }

        private object GetExpressionValue(Expression node)
        {
            if (node.NodeType == ExpressionType.Constant)
                return GetConstantValue(node);
            if (node.NodeType == ExpressionType.MemberAccess)
                return GetMemberValue((MemberExpression)node);
            throw new ParserException("无法获取值的表达式", node);
        }

        private object GetConstantValue(Expression node)
        {
            if (node.NodeType != ExpressionType.Constant)
                throw new ParserException("当前必须为常量表达式", node);
            return ((ConstantExpression)node).Value;
        }

        private Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        private object GetMemberValue(MemberExpression node)
        {
            var convertExpr = Expression.Convert(node, typeof(object));
            var getterExpr = Expression.Lambda<Func<object>>(convertExpr);
            var getter = getterExpr.Compile();
            return getter();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable))
            {
                switch (node.Method.Name)
                {
                    case "Where":
                        ProcessWhere(node);
                        break;
                    case "OrderBy":
                        mSort.Ascending(GetMemberName(node.Arguments[1]));
                        break;
                    case "OrderByDescending":
                        mSort.Descending(GetMemberName(node.Arguments[1]));
                        break;
                    case "Skip":
                        mResult.Skip = (int)GetConstantValue(node.Arguments[1]);
                        break;
                    case "Take":
                        mResult.Take = (int)GetConstantValue(node.Arguments[1]);
                        return Visit(node.Arguments[0]);
                    case "First":
                        mResult.MethodCall = MethodCall.First;
                        if (node.Arguments.Count == 2)
                            ProcessWhere(node);
                        break;
                    case "FirstOrDefault":
                        mResult.MethodCall = MethodCall.FirstOrDefault;
                        if (node.Arguments.Count == 2)
                            ProcessWhere(node);
                        break;
                    case "Count":
                        mResult.MethodCall = MethodCall.Count;
                        if (node.Arguments.Count == 2)
                            ProcessWhere(node);
                        break;
                    case "Select":
                        var expr = ((LambdaExpression)StripQuotes(node.Arguments[1])).Body;
                        if (expr.NodeType != ExpressionType.New)
                            throw new ParserException("仅支持 Select 内必须为 new 表达式", expr);
                        mResult.Select = (NewExpression)expr;
                        break;
                    default:
                        throw new ParserException("不支持 " + node.Method.Name + " 方法", node);
                }
            }
            else if(node.Method.DeclaringType == typeof(MongoQueryable))
            {
                switch (node.Method.Name)
                {
                    case "Set":
                        string name = GetMemberName(node.Arguments[1]);
                        object value = GetConstantValue(node.Arguments[2]);
                        mUpdate.Set(name, BsonValue.Create(value));
                        break;
                    case "Update":
                        mResult.MethodCall = MethodCall.Update;
                        break;
                    case "Delete":
                        mResult.MethodCall = MethodCall.Delete;
                        if (node.Arguments.Count == 2)
                            ProcessWhere(node);
                        break;
                }
            }
            else if (node.Method.Name == "Contains")
            {
                ProcessContains(node, false);
            }
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Not:
                    if (!(node.Operand is MethodCallExpression))
                        throw new ParserException("Not 仅能用在指定 Contains 函数之前", node);
                    ProcessContains((MethodCallExpression)node.Operand, true);
                    break;
            }
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    Visit(node.Right);
                    Visit(node.Left);
                    ProcessCondition(node.NodeType);
                    break;

                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.LessThan:
                    string name = GetMemberName(node.Left);
                    BsonValue value = BsonValue.Create(GetExpressionValue(node.Right));
                    ProcessCondition(node.NodeType, name, value);
                    break;
                default:
                    throw new ParserException("不支持的运算符", node);
            }
            return node;
        }
    }
}
