using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using MongoDB.Driver;
using System.Linq.Expressions;
using Avalon.Utility;

namespace Avalon.MongoAccess
{
    internal class MonogQueryable<T> : QueryableBase<T>
    {
        public MonogQueryable(IQueryExecutor executor)
            : base(new DefaultQueryProvider(typeof(MonogQueryable<>), QueryParser.CreateDefault(), executor))
        {
        }

        public MonogQueryable(IQueryProvider provder, Expression expr)
            : base(provder, expr)
        {
        }
    }
}
