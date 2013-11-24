using Avalon.Framework.Shards;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public abstract class AbstractConnectionManager<T> where T : class
    {
        object syncRoot = new object();
        Dictionary<string, T> nameDic = new Dictionary<string, T>();
        Dictionary<ShardId, T> shardDic = new Dictionary<ShardId, T>();

        public T GetConnection(string connName)
        {
            T conn = nameDic.TryGetValue(connName);
            if (conn == null)
            {
                lock (syncRoot)
                {
                    conn = nameDic.TryGetValue(connName);
                    if (conn == null)
                    {
                        var connSetting = ConfigurationManager.ConnectionStrings[connName];
                        if (connSetting == null)
                            throw new ConfigurationErrorsException(String.Format("缺少命名为“{0}”的连接字符串", connName));

                        conn = CreateConnection(connSetting.ConnectionString);
                        nameDic.Add(connName, conn);
                    }
                }
            }
            return conn;
        }

        public T GetConnection(ShardId shardId)
        {
            T conn = shardDic.TryGetValue(shardId);
            if (conn == null)
            {
                lock (syncRoot)
                {
                    conn = shardDic.TryGetValue(shardId);
                    if (conn == null)
                    {
                        var shardNodes = ToolSection.Instance.TryGetNodes("shard/shardIds/shardId");
                        var shardNode = shardNodes.FirstOrDefault(o => o.Attributes.TryGetValue("id") == shardId.Id);
                        if (shardNode == null)
                            throw new MissConfigurationException(ToolSection.Instance.RootNode, "shard/shardIds/shardId");

                        var connName = shardNode.Attributes.TryGetValue("connectionName");
                        if (String.IsNullOrEmpty(connName))
                            throw new ArgumentNullException("必须为分区 " + shardId.Id + " 指定数据源链接的名称路径 shard/shardIds/shardId/connectionName ");

                        conn = GetConnection(connName);
                        shardDic.Add(shardId, conn);
                    }
                }
            }
            return conn;
        }

        protected abstract T CreateConnection(string connStr);
    }
}
