using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Remotion.Linq;
using IBatisNet.DataMapper;

using Projects.Framework.Shards;

namespace Projects.Accesses.IBatisNetAccess
{
    internal class IBatisNetQueryExecutor : IQueryExecutor
    {
        private ISqlMapSession mSession;
        private ShardParams mShardParams;

        public IBatisNetQueryExecutor(ISqlMapSession session, ShardParams shardParams)
        {
            this.mSession = session;
            this.mShardParams = shardParams;
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            throw new NotImplementedException();
        }

        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            throw new NotImplementedException();
        }

        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            throw new NotImplementedException();
        }
    }
}
