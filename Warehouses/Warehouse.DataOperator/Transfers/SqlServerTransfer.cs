using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

using Warehouse.Settings;
using Warehouse.Utility;

namespace Warehouse.DataOperator
{
    /// <summary>
    /// SQL Server数据库传输类
    /// </summary>
    public class SqlServerTransfer : IServerTransfer
    {
        /// <summary>
        /// 复制数据库结构,将源库中的所有数据表都复制到目标库中
        /// 方法只复制数据数据库的结构,而不会拷贝数据
        /// </summary>
        /// <param name="source">需要复制的源数据库</param>
        /// <param name="target">需要复制到的目标数据库</param>
        public ResultKey Clone(StorageNodeConfigure source, StorageNodeConfigure target)
        {
            return Clone(source, target, null);
        }

        /// <summary>
        /// 复制数据库结构,只复制指定的表
        /// 方法只复制数据数据库的结构,而不会拷贝数据
        /// </summary>
        /// <param name="source">需要复制的源数据库</param>
        /// <param name="target">需要复制到的目标数据库</param>
        /// <param name="tables">需要克隆的数据表</param>
        public ResultKey Clone(StorageNodeConfigure source, StorageNodeConfigure target, string[] tables)
        {
            if (!(source is RelationDatabaseConfigure))
                throw new ArgumentException("源数据库节点配置不是有效的关系数据库存储节点");
            if (!(target is RelationDatabaseConfigure))
                throw new ArgumentException("目标数据库节点配置不是有效的关系数据库存储节点");

            var srcConfig = (RelationDatabaseConfigure)source;
            var targetConfig = (RelationDatabaseConfigure)target;
            //服务连接信息
            var srcConnectInfo =
                new SqlConnectionInfo(srcConfig.Host, srcConfig.DBUser, srcConfig.DBPassword);
            var targetConnectInfo =
                new SqlConnectionInfo(targetConfig.Host, targetConfig.DBUser, targetConfig.DBPassword);
            //连接到服务器
            Server srcServer = new Server(new ServerConnection(srcConnectInfo));
            Server targetServer = new Server(new ServerConnection(targetConnectInfo));
            //获取目标数据库
            Database srcDb, targetDb;
            if (!srcServer.Databases.Contains(srcConfig.DBName))
                throw new Exception("源服务器中不包含指点的数据库实例");
            srcDb = srcServer.Databases[srcConfig.DBName];
            //如果目标服务器中已经存在指定的数据库
            if (targetServer.Databases.Contains(targetConfig.DBName))
            {
                //获取目标数据库,并删除之
                targetDb = targetServer.Databases[targetConfig.DBName];
                targetDb.Drop();
            }
            //开始传输数据库结构
            targetDb = new Database(targetServer, targetConfig.DBName);
            targetDb.Create();
            Transfer transfer = new Transfer(srcDb);
            if (tables == null)
                transfer.CopyAllTables = true;
            else
                transfer.ObjectList = new ArrayList(tables);
            transfer.CopySchema = true;
            transfer.Options.WithDependencies = true;
            transfer.Options.ContinueScriptingOnError = true;
            transfer.DestinationDatabase = targetConfig.DBName;
            transfer.DestinationServer = targetServer.Name;
            transfer.DestinationLogin = targetConfig.DBUser;
            transfer.DestinationPassword = targetConfig.DBPassword;
            var transferScript = transfer.ScriptTransfer();

            targetDb.ExecuteNonQuery(transferScript);
            return ResultKey.OK;
        }

        /// <summary>
        /// 根据数据存储策略创建数据表
        /// </summary>
        public ResultKey Generate(IPolicy policy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 传输数据表中的数据
        /// </summary>
        public ResultKey Transmission(string source, string target, IPolicy policy)
        {
            throw new NotImplementedException();
        }

        #region 内部方法

        #endregion 
    }
}
