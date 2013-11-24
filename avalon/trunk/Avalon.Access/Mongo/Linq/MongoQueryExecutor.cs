using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remotion.Linq;
using MongoDB.Driver;
using Avalon.Utility;
using Avalon.Profiler;

namespace Avalon.MongoAccess
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
                var qs = FormatQuery(query);
                Trace<T>("Count", qs);
                using (ProfilerContext.Watch("mongo:" + qs))
                {
                    return (T)Convert.ChangeType(GetCollection<T>().Count(query.Query), typeof(T));
                }
            }
            throw new NotImplementedException();
        }

        public T ExecuteSingle<T>(QueryData query, bool returnDefaultWhenEmpty)
        {
            var qs = FormatQuery(query);
            Trace<T>("FindOne", qs);

            using (ProfilerContext.Watch("mongo:" + qs))
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
            }
            throw new NotSupportedException();
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryData query)
        {
            var qs = FormatQuery(query);
            Trace<T>("FindAll", qs);

            using (ProfilerContext.Watch("mongo:" + qs))
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

        string FormatQuery(QueryData query)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("operate: \"{0}\"", query.Operate);
            if (query.SkipValue.HasValue)
                sb.AppendFormat(", skip: {0}", query.SkipValue.Value);
            if (query.TakeValue.HasValue)
                sb.AppendFormat(", take: {0}", query.TakeValue.Value);
            sb.AppendFormat(", query: {0}", query.Query);
            sb.AppendFormat(", sort: {0}", query.Sorts);
            sb.Append("}");
            return sb.ToString();
        }


        void Trace<T>(string method, string query)
        {
            if (ProfilerContext.Current.Enabled)
                ProfilerContext.Current.Trace("mongo", String.Format("{0}@{1}\r\n{2}", typeof(T).FullName, method, ProfilerUtil.JsonFormat(query)));
        }
    }
}
