using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.Framework.Querys
{
    public class ODataQueryData
    {
        public ODataQueryData(CommonTree root)
        {
            Arguments.NotNull(root, "root");
            if (root.Type == 0)
            {
                var children = root.Children;
                FilterNode = children.OfType<FilterExpressionNode>().FirstOrDefault();
                TopNode = children.OfType<TopExpressionNode>().FirstOrDefault();
                SkipNode = children.OfType<SkipExpressionNode>().FirstOrDefault();
                SelectNode = children.OfType<SelectExpressionNode>().FirstOrDefault();
                OrderbyNode = children.OfType<OrderbyExpressionNode>().FirstOrDefault();
                InlineCountNode = children.OfType<InlineCountExpressionNode>().FirstOrDefault();
                CountNode = children.OfType<CountExpressionNode>().FirstOrDefault();
            }
            else
            {
                if (root is FilterExpressionNode)
                    FilterNode = (FilterExpressionNode)root;
                else if (root is TopExpressionNode)
                    TopNode = (TopExpressionNode)root;
                else if (root is SkipExpressionNode)
                    SkipNode = (SkipExpressionNode)root;
                else if (root is SelectExpressionNode)
                    SelectNode = (SelectExpressionNode)root;
                else if (root is OrderbyExpressionNode)
                    OrderbyNode = (OrderbyExpressionNode)root;
                else if (root is InlineCountExpressionNode)
                    InlineCountNode = (InlineCountExpressionNode)root;
                else if (root is CountExpressionNode)
                    CountNode = (CountExpressionNode)root;
            }
            if (TopNode != null && TopNode.Value < 0)
                throw new ArgumentException("$top 值不能小于 0");

            if (SkipNode != null && SkipNode.Value < 0)
                throw new ArgumentException("$skip 值不能小于 0");
        }

        public FilterExpressionNode FilterNode { get; set; }

        public TopExpressionNode TopNode { get; set; }

        public SkipExpressionNode SkipNode { get; set; }

        public SelectExpressionNode SelectNode { get; set; }

        public OrderbyExpressionNode OrderbyNode { get; set; }

        public InlineCountExpressionNode InlineCountNode { get; set; }

        public CountExpressionNode CountNode { get; set; }

        public int? Skip
        {
            get
            {
                if (SkipNode == null)
                    return null;
                return SkipNode.Value;
            }
        }

        public int? Top
        {
            get
            {
                if (TopNode == null)
                    return null;
                return TopNode.Value;
            }
        }

        public bool InlineCount
        {
            get { return InlineCountNode != null && InlineCountNode.InlinCountType == "allpages"; }
        }

        public bool Count
        {
            get { return CountNode != null && CountNode.Value; }
        }

        public IList<ExpressionNode> DescendantExpressions()
        {
            List<ExpressionNode> exps = new List<ExpressionNode>();
            if (FilterNode != null)
            {
                exps.Add(FilterNode);
                exps.AddRange(FilterNode.DescendantExpressions());
            }
            if (SelectNode != null)
            {
                exps.Add(SelectNode);
                exps.AddRange(SelectNode.DescendantExpressions());
            }
            if (OrderbyNode != null)
            {
                exps.Add(OrderbyNode);
                exps.AddRange(OrderbyNode.DescendantExpressions());
            }
            if (TopNode != null)
            {
                exps.Add(TopNode);
                exps.AddRange(TopNode.DescendantExpressions());
            }
            if (SkipNode != null)
            {
                exps.Add(SkipNode);
                exps.AddRange(SkipNode.DescendantExpressions());
            }
            if (CountNode != null)
            {
                exps.Add(CountNode);
                exps.AddRange(CountNode.DescendantExpressions());
            }
            return exps;
        }

        public static ODataQueryData Parse(NameValueCollection datas)
        {
            Arguments.NotNull(datas, "datas");
            var queryString = GetODataQueryString(datas);

            var input = new ANTLRReaderStream(new StringReader(queryString));
            var lexer = new AvalonQueryLexer(input);
            var tokStream = new CommonTokenStream(lexer);

            var parser = new AvalonQueryParser(tokStream) { TreeAdaptor = new QueryTreeAdaptor() };

            var result = parser.prog();
            var tree = result.Tree;

            var query = new ODataQueryData(tree);
            return query;
        }

        static string GetODataQueryString(NameValueCollection datas)
        {
            List<string> odataQuerys = new List<string>();
            foreach (string key in datas)
            {
                if (key.StartsWith("$"))
                    odataQuerys.Add(key + "=" + datas[key]);
            }
            return String.Join("&", odataQuerys);
        }
    }
}
