using Avalon.Framework;
using Avalon.Framework.Shards;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.NHibernateAccess
{
    internal class ShardInterceptor : EmptyInterceptor
    {
        List<PartitionId> partitionIds = new List<PartitionId>();

        public ShardInterceptor(PartitionId partitionId)
        {
            if (partitionId != null)
                partitionIds.Add(partitionId);
        }

        public bool Register(PartitionId partitionId)
        {
            //没有分区
            if (partitionId == null)
                return true;

            if (partitionIds.Any(o => o.TableName == partitionId.TableName && o.RealTableName != partitionId.RealTableName))
                return false;

            if (!partitionIds.Any(o => o.TableName == partitionId.TableName && o.RealTableName == partitionId.RealTableName))
                partitionIds.Add(partitionId);
            return true;
        }

        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            foreach (var partitionId in partitionIds)
            {
                sql = sql.Replace(partitionId.TableName, partitionId.RealTableName);
            }

            return base.OnPrepareStatement(sql);
        }

        public override string GetEntityName(object entity)
        {
            if (ProxyProvider.IsProxy(entity))
                return entity.GetType().BaseType.FullName;

            return base.GetEntityName(entity);
        }
    }
}
