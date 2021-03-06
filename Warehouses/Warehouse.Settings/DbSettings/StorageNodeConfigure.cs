﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Accesses.MongoAccess;

namespace Warehouse.Settings
{
    /// <summary>
    /// 存储节点的配置基类
    /// </summary>
    public abstract class StorageNodeConfigure : IConfigure
    {
        public string ConfigKey
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public StorageMediaType StorageMedia { get; protected set; }

        /// <summary>
        /// 读取存储媒介的配置
        /// </summary>
        public void Load()
        {
            
        }

        /// <summary>
        /// 保存存储媒介的配置
        /// </summary>
        public void Save()
        {
            if (this.StorageMedia == StorageMediaType.MySQL 
                || this.StorageMedia == StorageMediaType.SQLServer)
            {
                var config = (RelationDatabaseConfigure)this;
                MongoAccessor.Insert<RelationDatabaseConfigure>(config);
            }
        }

        /// <summary>
        /// 将配置转化为数据库连接字符串
        /// </summary>
        public abstract string ToConnectionString();
    }
}
