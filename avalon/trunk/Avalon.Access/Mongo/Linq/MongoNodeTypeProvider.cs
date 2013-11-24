using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remotion.Linq.Parsing.Structure;
using System.Reflection;

namespace Avalon.MongoAccess
{
    internal class MongoNodeTypeProvider : INodeTypeProvider
    {
        private INodeTypeProvider defaultNodeTypeProvider;

        public MongoNodeTypeProvider()
        {
            defaultNodeTypeProvider = ExpressionTreeParser.CreateDefaultNodeTypeProvider();
        }

        public bool IsRegistered(MethodInfo method)
        {
            return defaultNodeTypeProvider.IsRegistered(method);
        }

        public Type GetNodeType(MethodInfo method)
        {
            return defaultNodeTypeProvider.GetNodeType(method);
        }
    }
}
