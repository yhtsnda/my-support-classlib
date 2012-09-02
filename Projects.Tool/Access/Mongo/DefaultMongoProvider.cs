using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using MongoDB.Driver;

namespace Projects.Tool.MongoAccess
{
    public class DefaultMongoProvider : IMongoProvider
    {
        private Dictionary<string, MongoDatabase> mDatabases;
        private string mDefaultDbName = "mongo";
        private object mSyncObj;

        public DefaultMongoProvider()
        {
            mDatabases = new Dictionary<string, MongoDatabase>(StringComparer.CurrentCultureIgnoreCase);
            mSyncObj = new object();
            string dn = ToolSection.Instance.TryGetValue("mongoAccess/default");
            if (!String.IsNullOrEmpty(dn))
            {
                mDefaultDbName = dn;
            }
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="collection">集合名称</param>
        public virtual MongoCollection GetCollection(string database, string collection)
        {
            if (String.IsNullOrEmpty(database))
                database = mDefaultDbName;
            MongoDatabase mongoDb = GetDatabase(database);
            return mongoDb.GetCollection(collection);
        }

        /// <summary>
        /// 获取Mongo数据库
        /// </summary>
        /// <param name="database">数据库名称</param>
        public MongoDatabase GetDatabase(string database)
        {
            if (String.IsNullOrEmpty(database))
                database = mDefaultDbName;

            MongoDatabase mongoDb;
            if (!mDatabases.TryGetValue(database, out mongoDb))
            {
                lock (mSyncObj)
                {
                    if (!mDatabases.TryGetValue(database, out mongoDb))
                    {
                        mongoDb = MongoDatabase.Create(ConfigurationManager.ConnectionStrings[database].ConnectionString);
                        mDatabases.Add(database, mongoDb);
                    }
                }
            }
            return mongoDb;
        }
    }
}
