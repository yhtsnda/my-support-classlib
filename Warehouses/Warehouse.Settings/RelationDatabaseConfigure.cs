using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.Settings
{
    /// <summary>
    /// 关系性数据库配置
    /// </summary>
    public class RelationDatabaseConfigure : StorageNodeConfigure
    {

        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; protected set; }

        private int mPort = 3306;
        /// <summary>
        /// 主机端口
        /// </summary>
        public int Port { get { return mPort; } protected set { mPort = value; } }

        private int mTimeout = 15;
        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get { return mTimeout; } protected set { mTimeout = value; } }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DBName { get; protected set; }

        /// <summary>
        /// 数据库用户
        /// </summary>
        public string DBUser { get; protected set; }

        /// <summary>
        /// 数据库用户密码
        /// </summary>
        public string DBPassword { get; protected set; }

        /// <summary>
        /// 数据驱动
        /// </summary>
        public int Driver { get; protected set; }

        /// <summary>
        /// 关系型数据库连接配置
        /// </summary>
        /// <param name="type">关系型数据库的类型</param>
        public RelationDatabaseConfigure(StorageMediaType type)
        {
            if (type == StorageMediaType.File)
                base.StorageMedia = StorageMediaType.SQLServer;
            else
                base.StorageMedia = type;
        }

        /// <summary>
        /// 关系型数据库连接配置
        /// </summary>
        public RelationDatabaseConfigure()
        {
            base.StorageMedia = StorageMediaType.SQLServer;
        }

        /// <summary>
        /// 获取数据库的连接字符串
        /// </summary>
        public override string ToConnectionString()
        {
            if (base.StorageMedia == StorageMediaType.SQLServer)
            {
                return String.Format("Data Source={0};Initial Catalog= {1};UserId={2};Password={3};Connect Timeout={4}", 
                    Host, DBName, DBUser, DBPassword, Timeout);
            }
            else if (base.StorageMedia == StorageMediaType.MySQL)
            {
                return String.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};",
                    Host, Port, DBName, DBUser, DBPassword);
            }
            return String.Empty;
        }
    }
}
