using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    /// <summary>
    /// 数据节点配置
    /// </summary>
    public class DataNodeConfig
    {
        private const int DEFAULT_POOL_SIZE = 128;
        private const long DEFAULT_WAIT_TIMEOUT = 10 * 1000L;
        private const long DEFAULT_IDLE_TIMEOUT = 10 * 60 * 1000L;
        private const long DEFAULT_HEARTBEAT_TIMEOUT = 30 * 1000L;
        private const int DEFAULT_HEARTEBAT_RETRY = 10;

        private int poolSize = DEFAULT_POOL_SIZE;
        private long waitTimeout = DEFAULT_WAIT_TIMEOUT;
        private long idleTimeout = DEFAULT_IDLE_TIMEOUT;
        private int heartbeatRetry = DEFAULT_HEARTEBAT_RETRY;
        private long heartbeatTimeount = DEFAULT_HEARTBEAT_TIMEOUT;

        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }
        
        /// <summary>
        /// 节点对应的主数据源名称
        /// </summary>
        public string MasterSource { get; set; }
        
        /// <summary>
        /// 节点对应的副数据源名称
        /// </summary>
        public string SlaveSource { get; set; }

        /// <summary>
        /// 心跳SQL
        /// </summary>
        public string HeartbeatSQL { get; set; }

        /// <summary>
        /// 保持后端数据通道的默认最大值
        /// </summary>
        public int PoolSize { get { return poolSize; } set { poolSize = value; } }

        /// <summary>
        /// 取得新连接的等待超时时间
        /// </summary>
        public long WaitTimeout { get { return waitTimeout; } set { waitTimeout = value; } }

        /// <summary>
        /// 连接池中连接空闲超时时间
        /// </summary>
        public long IdleTimeout { get { return idleTimeout; } set { idleTimeout = value; } }

        /// <summary>
        /// 检查连接发生异常到切换，重试次数
        /// </summary>
        public int HeartbeatRetry { get { return heartbeatRetry; } set { heartbeatRetry = value; } }

        /// <summary>
        /// 心跳超时时间
        /// </summary>
        public long HeartbeatTimeount { get { return heartbeatTimeount; } set { heartbeatTimeount = value; } }
    }
}
