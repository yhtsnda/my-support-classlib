using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remotion.Linq;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;

namespace Avalon.MongoAccess
{
    internal static class MongoQueryParser
    {
        static readonly QueryParser queryParser;

        static MongoQueryParser()
        {
            var nodeTypeProvider = new MongoNodeTypeProvider();

            var transformerRegistry = ExpressionTransformerRegistry.CreateDefault();
            var processor = ExpressionTreeParser.CreateDefaultProcessor(transformerRegistry);

            var expressionTreeParser = new ExpressionTreeParser(nodeTypeProvider, processor);
            queryParser = new QueryParser(expressionTreeParser);
        }

        public static QueryParser QueryParser
        {
            get { return queryParser; }
        }

        public static QueryModel Parse(Expression expression)
        {
            return queryParser.GetParsedQuery(expression);
        }
    }
}
