using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MongoDB.Driver;

namespace Projects.Accesses.MongoAccess
{
    internal class QueryParserResult
    {
        public Type EntityType { get; set; }
        public NewExpression Select { get; set; }
        public IMongoQuery Query { get; set; }
        public IMongoSortBy SortBy { get; set; }
        public IMongoUpdate Update { get; set; }

        public MethodCall MethodCall { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    internal enum MethodCall
    {
        NoSet,
        Query,
        First,
        FirstOrDefault,
        Count,
        Update,
        Delete
    }
}
