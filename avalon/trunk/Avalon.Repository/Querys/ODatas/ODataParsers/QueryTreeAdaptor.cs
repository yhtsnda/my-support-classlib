using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class QueryTreeAdaptor : CommonTreeAdaptor
    {
        public override object Create(IToken token)
        {
            if (token != null)
            {
                ExpressionNode node = null;
                switch (token.Type)
                {
                    case AvalonQueryLexer.FILTER:
                        node = new FilterExpressionNode();
                        break;
                    case AvalonQueryLexer.TOP:
                        node = new TopExpressionNode();
                        break;
                    case AvalonQueryLexer.SKIP:
                        node = new SkipExpressionNode();
                        break;
                    case AvalonQueryLexer.SELECT:
                        node = new SelectExpressionNode();
                        break;
                    case AvalonQueryLexer.ORDERBY:
                        node = new OrderbyExpressionNode();
                        break;
                    case AvalonQueryLexer.INLINECOUNT:
                        node = new InlineCountExpressionNode();
                        break;
                    case AvalonQueryLexer.ALLPAGES:
                    case AvalonQueryLexer.NONE:
                        node = new InlineCountTypeExpressionNode() { InlinCountType = token.Text };
                        break;
                    case AvalonQueryLexer.ASC:
                    case AvalonQueryLexer.DESC:
                        node = new OrderbyItemExpresionNode(token);
                        break;
                    case AvalonQueryLexer.SUBSTRINGOF:
                        node = new FunctionExpressionNode("substringof");
                        break;
                    case AvalonQueryLexer.STARTSWITH:
                        node = new FunctionExpressionNode("startswith");
                        break;
                    case AvalonQueryLexer.ENDSWITH:
                        node = new FunctionExpressionNode("endswith");
                        break;
                    case AvalonQueryLexer.AND:
                    case AvalonQueryLexer.OR:
                    case AvalonQueryLexer.EQUALS:
                    case AvalonQueryLexer.NOTEQUALS:
                    case AvalonQueryLexer.LESSTHAN:
                    case AvalonQueryLexer.LESSTHANOREQUAL:
                    case AvalonQueryLexer.GREATERTHAN:
                    case AvalonQueryLexer.GREATERTHANOREQUAL:
                        node = new BinaryExpressionNode();
                        break;
                    case AvalonQueryLexer.NOT:
                        node = new NotExpressionNode();
                        break;
                    case AvalonQueryLexer.STRING:
                    case AvalonQueryLexer.INT:
                    case AvalonQueryLexer.LONG:
                    case AvalonQueryLexer.DOUBLE:
                    case AvalonQueryLexer.SINGLE:
                    case AvalonQueryLexer.BOOL:
                    case AvalonQueryLexer.NULL:
                    case AvalonQueryLexer.DATETIME:
                    case AvalonQueryLexer.GUID:
                    case AvalonQueryLexer.BYTE:
                        node = new ConstantExpressionNode(token);
                        break;
                    case AvalonQueryLexer.IDENTIFIER:
                        node = new PropertyExpressionNode(token.Text);
                        break;
                    case AvalonQueryLexer.IN:
                    case AvalonQueryLexer.NOTIN:
                        node = new InExpressionNode(token.Type == AvalonQueryLexer.NOTIN);
                        break;
                }
                if (node != null)
                {
                    node.Token = token;
                    node.TreeAdaptor = this;
                    node.NodeType = GetType(token.Type);
                    return node;
                }
            }
            return base.Create(token);
        }

        ExpressionNodeType GetType(int type)
        {
            switch (type)
            {
                case AvalonQueryLexer.FILTER:
                    return ExpressionNodeType.Filter;
                case AvalonQueryLexer.SKIP:
                    return ExpressionNodeType.Skip;
                case AvalonQueryLexer.TOP:
                    return ExpressionNodeType.Top;
                case AvalonQueryLexer.SELECT:
                    return ExpressionNodeType.Select;
                case AvalonQueryLexer.ORDERBY:
                    return ExpressionNodeType.Orderby;

                case AvalonQueryLexer.ASC:
                case AvalonQueryLexer.DESC:
                    return ExpressionNodeType.OrderbyItem;
                case AvalonQueryLexer.INLINECOUNT:
                    return ExpressionNodeType.InlineCount;

                case AvalonQueryLexer.SUBSTRINGOF:
                case AvalonQueryLexer.STARTSWITH:
                case AvalonQueryLexer.ENDSWITH:
                    return ExpressionNodeType.Function;

                case AvalonQueryLexer.AND:
                    return ExpressionNodeType.And;
                case AvalonQueryLexer.OR:
                    return ExpressionNodeType.Or;
                case AvalonQueryLexer.NOT:
                    return ExpressionNodeType.Not;

                case AvalonQueryLexer.EQUALS:
                    return ExpressionNodeType.Equal;
                case AvalonQueryLexer.NOTEQUALS:
                    return ExpressionNodeType.NotEqual;
                case AvalonQueryLexer.LESSTHAN:
                    return ExpressionNodeType.LessThan;
                case AvalonQueryLexer.LESSTHANOREQUAL:
                    return ExpressionNodeType.LessThanOrEqual;
                case AvalonQueryLexer.GREATERTHAN:
                    return ExpressionNodeType.GreaterThan;
                case AvalonQueryLexer.GREATERTHANOREQUAL:
                    return ExpressionNodeType.GreaterThanOrEqual;
                case AvalonQueryLexer.STRING:
                case AvalonQueryLexer.INT:
                    return ExpressionNodeType.Constant;
                case AvalonQueryLexer.IDENTIFIER:
                    return ExpressionNodeType.Property;
                case AvalonQueryLexer.BOOL:
                    return ExpressionNodeType.Constant;
                case AvalonQueryLexer.IN:
                case AvalonQueryLexer.NOTIN:
                    return ExpressionNodeType.In;
            }
            return ExpressionNodeType.Unkonwn;
        }
    }
}
