using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class ODataQueryValidator
    {
        public static ODataQueryValidator NotLimit;
        public static ODataQueryValidator PagingNeed;


        static ODataQueryValidator()
        {
            NotLimit = new ODataQueryValidator(0);
            PagingNeed = new ODataQueryValidator(100, QueryType.Limit);
        }

        public ODataQueryValidator(int maxTop = 100, QueryType needType = QueryType.None, QueryType enabledType = QueryType.All, QueryType disabledType = QueryType.None)
        {
            MaxTop = maxTop;
            NeedType = needType;
            EnabledType = enabledType;
            DisabledType = disabledType;
        }

        public int MaxTop { get; set; }

        public QueryType EnabledType { get; set; }

        public QueryType DisabledType { get; set; }

        public QueryType NeedType { get; set; }

        public Action<ODataQueryData> CustomHandler { get; set; }

        public void Valid(ODataQueryData queryData)
        {
            if (MaxTop > 0)
            {
                if (!queryData.Top.HasValue)
                    throw new AvalonException("未指定 $top 参数") { Code = 400000 };

                if (queryData.Top.Value > MaxTop)
                    throw new AvalonException(String.Format("给定 $top 参数不能大于 {0}", MaxTop)) { Code = 400000 };
            }
            var queryTypes = Process(queryData);

            if (NeedType != QueryType.None)
            {
                var needs = ToHash(NeedType);
                needs.ExceptWith(queryTypes);
                if (needs.Count > 0)
                    throw new AvalonException("查询缺少必须的查询信息 " + String.Join(",", needs)) { Code = 400000 };
            }
            if (DisabledType != QueryType.None)
            {
                var disables = ToHash(DisabledType);
                disables.IntersectWith(queryTypes);
                if (disables.Count > 0)
                    throw new AvalonException("查询包含了禁用的类型 " + String.Join(",", disables)) { Code = 400000 };
            }

            if (EnabledType != QueryType.All)
            {
                var enables = ToHash(EnabledType);
                var dis = queryTypes.Except(enables);
                if (dis.Count() > 0)
                    throw new AvalonException("查询包含了不可用的类型 " + String.Join(",", dis)) { Code = 400000 };
            }
            if (CustomHandler != null)
                CustomHandler(queryData);
        }

        HashSet<QueryType> ToHash(QueryType queryType)
        {
            HashSet<QueryType> hash = new HashSet<QueryType>();
            var values = Enum.GetValues(typeof(QueryType)).Cast<QueryType>();
            foreach (var value in values)
            {
                if ((queryType & value) == value)
                    hash.Add(value);
            }
            hash.Remove(QueryType.None);
            hash.Remove(QueryType.All);
            return hash;
        }

        HashSet<QueryType> Process(ODataQueryData queryData)
        {
            HashSet<QueryType> hash = new HashSet<QueryType>();

            if (queryData.FilterNode != null)
            {
                hash.Add(QueryType.Filter);
                var vistor = new ODataExpressionNodeVisitor();
                vistor.Visit(queryData.FilterNode.Body);
                hash.UnionWith(vistor.QueryTypes);
            }
            if (queryData.OrderbyNode != null)
                hash.Add(QueryType.Order);
            if (queryData.Skip.HasValue || queryData.Top.HasValue)
                hash.Add(QueryType.Limit);
            if (queryData.InlineCount)
                hash.Add(QueryType.InlineCount);
            if (queryData.SelectNode != null)
                hash.Add(QueryType.Select);

            return hash;
        }

        class ODataExpressionNodeVisitor
        {
            HashSet<QueryType> hash = new HashSet<QueryType>();

            public HashSet<QueryType> QueryTypes
            {
                get { return hash; }
            }

            public void Visit(ExpressionNode exp)
            {
                switch (exp.NodeType)
                {
                    case ExpressionNodeType.And:
                    case ExpressionNodeType.Or:
                    case ExpressionNodeType.Equal:
                    case ExpressionNodeType.LessThan:
                    case ExpressionNodeType.LessThanOrEqual:
                    case ExpressionNodeType.GreaterThan:
                    case ExpressionNodeType.GreaterThanOrEqual:
                    case ExpressionNodeType.NotEqual:
                        VisitBinary((BinaryExpressionNode)exp);
                        break;
                    case ExpressionNodeType.Constant:
                        break;
                    case ExpressionNodeType.Property:
                        break;
                    case ExpressionNodeType.Function:
                        VisitFunction((FunctionExpressionNode)exp);
                        break;
                    case ExpressionNodeType.In:
                        VisitIn((InExpressionNode)exp);
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
            void VisitFunction(FunctionExpressionNode exp)
            {
                switch (exp.FunctionName)
                {
                    case "substringof":
                    case "startswith":
                    case "endswith":
                        hash.Add(QueryType.IsLike);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            void VisitIn(InExpressionNode exp)
            {
                hash.Add(exp.Mode == InMode.In ? QueryType.IsIn : QueryType.IsNotIn);
            }

            void VisitBinary(BinaryExpressionNode exp)
            {
                Visit(exp.Left);
                Visit(exp.Right);
            }
        }
    }

    /// <summary>
    /// 查询类型
    /// </summary>
    public enum QueryType
    {
        None = 0,
        /// <summary>
        /// 过滤器
        /// </summary>
        Filter = 1,
        /// <summary>
        /// 限制
        /// </summary>
        Limit = 2,
        /// <summary>
        /// 排序
        /// </summary>
        Order = 4,
        /// <summary>
        /// 总数
        /// </summary>
        InlineCount = 8,
        /// <summary>
        /// 投影
        /// </summary>
        Select = 16,
        /// <summary>
        /// 包含
        /// </summary>
        IsIn = 32,
        /// <summary>
        /// 不包含
        /// </summary>
        IsNotIn = 64,
        /// <summary>
        /// 字符串匹配
        /// </summary>
        IsLike = 128,
        All = 1023
    }
}
