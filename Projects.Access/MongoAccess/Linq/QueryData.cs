using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remotion.Linq.Clauses;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using Projects.Tool;

namespace Projects.Framework.MongoAccess
{
    internal class QueryData
    {
        public QueryData()
        {
            Sorts = new SortByBuilder();
        }

        public Type EntityType { get; set; }

        public IMongoQuery Query { get; set; }

        public OperateType Operate { get; set; }

        public SortByBuilder Sorts { get; private set; }

        public int? SkipValue { get; set; }

        public int? TakeValue { get; set; }
    }

    public enum OperateType
    {
        None,
        Count,
        First,
        Single
    }
}
