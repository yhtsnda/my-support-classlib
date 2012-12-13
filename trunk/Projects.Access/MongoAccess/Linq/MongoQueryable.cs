using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using MongoDB.Driver;
using Projects.Tool;

namespace Projects.Accesses.MongoAccess
{
    internal class MongoQueryable<T> : QueryableBase<T>
    {
        public MongoQueryable(IQueryExecutor executor)
            : base(new DefaultQueryProvider(typeof(MongoQueryable<>), QueryParser.CreateDefault(), executor))
        {
        }

        public MongoQueryable(IQueryProvider provder, Expression expr)
            : base(provder, expr)
        {
        }
    }
}
