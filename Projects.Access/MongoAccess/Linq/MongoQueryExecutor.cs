using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remotion.Linq;
using MongoDB.Driver;
using Projects.Tool;

namespace Projects.Framework.MongoAccess
{
    internal class MongoQueryExecutor : IQueryExecutor
    {
        MongoSession session;
        ShardParams shardParams;

        public MongoQueryExecutor(MongoSession session, ShardParams shardParams)
        {
            this.session = session;
            this.shardParams = shardParams;
        }

        MongoCollection GetCollection<T>()
        {
            return session.GetCollection<T>(shardParams);
        }

        QueryData GetQueryData<T>(QueryModel queryModel)
        {
            var query = MongoQueryModelVistor.GetQueryData(queryModel);
            if (query.EntityType != typeof(T))
                throw new ArgumentException();

            return query;
        }

        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            return ExecuteScalar<T>(GetQueryData<T>(queryModel));
        }

        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            return ExecuteSingle<T>(GetQueryData<T>(queryModel), returnDefaultWhenEmpty);
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            return ExecuteCollection<T>(GetQueryData<T>(queryModel));
        }

        public T ExecuteScalar<T>(QueryData query)
        {
            if (query.Operate == OperateType.Count)
            {
                return (T)Convert.ChangeType(GetCollection<T>().Count(query.Query), typeof(T));
            }
            throw new NotImplementedException();
        }

        public T ExecuteSingle<T>(QueryData query, bool returnDefaultWhenEmpty)
        {
            var cursor = GetCollection<T>().FindAs<T>(query.Query);

            cursor.SetSortOrder(query.Sorts);

            if (query.SkipValue.HasValue)
                cursor.SetSkip(query.SkipValue.Value);

            if (query.Operate == OperateType.First)
            {
                if (returnDefaultWhenEmpty)
                    return cursor.FirstOrDefault();

                return cursor.First();
            }

            if (query.Operate == OperateType.Single)
            {
                if (returnDefaultWhenEmpty)
                    return cursor.SingleOrDefault();

                return cursor.Single();
            }
            throw new NotSupportedException();
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryData query)
        {
            var cursor = GetCollection<T>().FindAs<T>(query.Query);
            cursor.SetSortOrder(query.Sorts);

            if (query.SkipValue.HasValue)
                cursor.SetSkip(query.SkipValue.Value);
            if (query.TakeValue.HasValue)
                cursor.SetLimit(query.TakeValue.Value);

            return cursor;
        }
    }
}
