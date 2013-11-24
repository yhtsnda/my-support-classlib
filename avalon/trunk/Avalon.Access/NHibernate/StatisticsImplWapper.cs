using Avalon.Profiler;
using Avalon.Utility;
using NHibernate.Stat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.NHibernateAccess
{
    public class StatisticsImplWapper : StatisticsImpl, IStatistics, IStatisticsImplementor
    {
        StatisticsImpl impl;

        public StatisticsImplWapper(StatisticsImpl impl)
        {
            this.impl = impl;
        }

        void IStatistics.Clear()
        {
            impl.Clear();
        }

        long IStatistics.CloseStatementCount
        {
            get { return impl.CloseStatementCount; }
        }

        long IStatistics.CollectionFetchCount
        {
            get { return impl.CollectionFetchCount; }
        }

        long IStatistics.CollectionLoadCount
        {
            get { return impl.CollectionLoadCount; }
        }

        long IStatistics.CollectionRecreateCount
        {
            get { return impl.CollectionRecreateCount; }
        }

        long IStatistics.CollectionRemoveCount
        {
            get { return impl.CollectionRemoveCount; }
        }

        string[] IStatistics.CollectionRoleNames
        {
            get { return impl.CollectionRoleNames; }
        }

        long IStatistics.CollectionUpdateCount
        {
            get { return impl.CollectionUpdateCount; }
        }

        long IStatistics.ConnectCount
        {
            get { return impl.ConnectCount; }
        }

        long IStatistics.EntityDeleteCount
        {
            get { return impl.EntityDeleteCount; }
        }

        long IStatistics.EntityFetchCount
        {
            get { return impl.EntityFetchCount; }
        }

        long IStatistics.EntityInsertCount
        {
            get { return impl.EntityInsertCount; }
        }

        long IStatistics.EntityLoadCount
        {
            get { return impl.EntityLoadCount; }
        }

        string[] IStatistics.EntityNames
        {
            get { return impl.EntityNames; }
        }

        long IStatistics.EntityUpdateCount
        {
            get { return impl.EntityUpdateCount; }
        }

        long IStatistics.FlushCount
        {
            get { return impl.FlushCount; }
        }

        CollectionStatistics IStatistics.GetCollectionStatistics(string role)
        {
            return impl.GetCollectionStatistics(role);
        }

        EntityStatistics IStatistics.GetEntityStatistics(string entityName)
        {
            return impl.GetEntityStatistics(entityName);
        }

        QueryStatistics IStatistics.GetQueryStatistics(string queryString)
        {
            return impl.GetQueryStatistics(queryString);
        }

        SecondLevelCacheStatistics IStatistics.GetSecondLevelCacheStatistics(string regionName)
        {
            return impl.GetSecondLevelCacheStatistics(regionName);
        }

        bool IStatistics.IsStatisticsEnabled
        {
            get
            {
                return impl.IsStatisticsEnabled;
            }
            set
            {
                impl.IsStatisticsEnabled = value;
            }
        }

        void IStatistics.LogSummary()
        {
            impl.LogSummary();
        }

        TimeSpan IStatistics.OperationThreshold
        {
            get
            {
                return impl.OperationThreshold;
            }
            set
            {
                impl.OperationThreshold = value;
            }
        }

        long IStatistics.OptimisticFailureCount
        {
            get { return impl.OptimisticFailureCount; }
        }

        long IStatistics.PrepareStatementCount
        {
            get { return impl.PrepareStatementCount; }
        }

        string[] IStatistics.Queries
        {
            get { return impl.Queries; }
        }

        long IStatistics.QueryCacheHitCount
        {
            get { return impl.QueryCacheHitCount; }
        }

        long IStatistics.QueryCacheMissCount
        {
            get { return impl.QueryCacheMissCount; }
        }

        long IStatistics.QueryCachePutCount
        {
            get { return impl.QueryCachePutCount; }
        }

        long IStatistics.QueryExecutionCount
        {
            get { return impl.QueryExecutionCount; }
        }

        TimeSpan IStatistics.QueryExecutionMaxTime
        {
            get { return impl.QueryExecutionMaxTime; }
        }

        string IStatistics.QueryExecutionMaxTimeQueryString
        {
            get { return impl.QueryExecutionMaxTimeQueryString; }
        }

        long IStatistics.SecondLevelCacheHitCount
        {
            get { return impl.SecondLevelCacheHitCount; }
        }

        long IStatistics.SecondLevelCacheMissCount
        {
            get { return impl.SecondLevelCacheMissCount; }
        }

        long IStatistics.SecondLevelCachePutCount
        {
            get { return impl.SecondLevelCachePutCount; }
        }

        string[] IStatistics.SecondLevelCacheRegionNames
        {
            get { return impl.SecondLevelCacheRegionNames; }
        }

        long IStatistics.SessionCloseCount
        {
            get { return impl.SessionCloseCount; }
        }

        long IStatistics.SessionOpenCount
        {
            get { return impl.SessionOpenCount; }
        }

        DateTime IStatistics.StartTime
        {
            get { return impl.StartTime; }
        }

        long IStatistics.SuccessfulTransactionCount
        {
            get { return impl.SuccessfulTransactionCount; }
        }

        long IStatistics.TransactionCount
        {
            get { return impl.TransactionCount; }
        }

        void IStatisticsImplementor.CloseSession()
        {
            //impl.CloseSession();
        }

        void IStatisticsImplementor.CloseStatement()
        {
            //impl.CloseStatement();
        }

        void IStatisticsImplementor.Connect()
        {
            //impl.Connect();
        }

        void IStatisticsImplementor.DeleteEntity(string entityName, TimeSpan time)
        {
            using (ProfilerContext.Watch(String.Format("@{0}ms delete {1}", (int)time.TotalMilliseconds, entityName))) ;
            //impl.DeleteEntity(entityName, time);
        }

        void IStatisticsImplementor.EndTransaction(bool success)
        {
            //impl.EndTransaction(success);
        }

        void IStatisticsImplementor.FetchCollection(string role, TimeSpan time)
        {
            //impl.FetchCollection(role, time);
        }

        void IStatisticsImplementor.FetchEntity(string entityName, TimeSpan time)
        {
            //impl.FetchEntity(entityName, time);
        }

        void IStatisticsImplementor.Flush()
        {
            //impl.Flush();
        }

        void IStatisticsImplementor.InsertEntity(string entityName, TimeSpan time)
        {
            using (ProfilerContext.Watch(String.Format("@{0}ms create {1}", (int)time.TotalMilliseconds, entityName))) ;
            //impl.InsertEntity(entityName, time);
        }

        void IStatisticsImplementor.LoadCollection(string role, TimeSpan time)
        {
            //impl.LoadCollection(role, time);
        }

        void IStatisticsImplementor.LoadEntity(string entityName, TimeSpan time)
        {
            var stack = ProfilerContext.Current.WatchStack;
            if (stack.Count > 0)
            {
                var m = stack.Peek().Message;
                if (m.Contains(".GetList") || m.Contains(".Find"))
                    return;
            }

            using (ProfilerContext.Watch(String.Format("@{0}ms load {1}", (int)time.TotalMilliseconds, entityName))) ;
        }

        void IStatisticsImplementor.OpenSession()
        {
            //impl.OpenSession();
        }

        void IStatisticsImplementor.OptimisticFailure(string entityName)
        {
            //impl.OptimisticFailure(entityName);
        }

        void IStatisticsImplementor.PrepareStatement()
        {
            //impl.PrepareStatement();
        }

        void IStatisticsImplementor.QueryCacheHit(string hql, string regionName)
        {
            //impl.QueryCacheHit(hql, regionName);
        }

        void IStatisticsImplementor.QueryCacheMiss(string hql, string regionName)
        {
            //impl.QueryCacheMiss(hql, regionName);
        }

        void IStatisticsImplementor.QueryCachePut(string hql, string regionName)
        {
            //impl.QueryCachePut(hql, regionName);
        }

        void IStatisticsImplementor.QueryExecuted(string hql, int rows, TimeSpan time)
        {
            using (ProfilerContext.Watch(String.Format("@{0}ms query {1} rows", (int)time.TotalMilliseconds, rows))) ;
            //impl.QueryExecuted(hql, rows, time);
        }

        void IStatisticsImplementor.RecreateCollection(string role, TimeSpan time)
        {
            //impl.RecreateCollection(role, time);
        }

        void IStatisticsImplementor.RemoveCollection(string role, TimeSpan time)
        {
            //impl.RemoveCollection(role, time);
        }

        void IStatisticsImplementor.SecondLevelCacheHit(string regionName)
        {
            //impl.SecondLevelCacheHit(regionName);
        }

        void IStatisticsImplementor.SecondLevelCacheMiss(string regionName)
        {
            //impl.SecondLevelCacheMiss(regionName);
        }

        void IStatisticsImplementor.SecondLevelCachePut(string regionName)
        {
            //impl.SecondLevelCachePut(regionName);
        }

        void IStatisticsImplementor.UpdateCollection(string role, TimeSpan time)
        {
            //impl.UpdateCollection(role, time);
        }

        void IStatisticsImplementor.UpdateEntity(string entityName, TimeSpan time)
        {
            using (ProfilerContext.Watch(String.Format("@{0}ms update {1}", (int)time.TotalMilliseconds, entityName))) ;
            //impl.UpdateEntity(entityName, time);
        }
    }
}
