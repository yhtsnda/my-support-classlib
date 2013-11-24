using Avalon.Framework;
using Avalon.Framework.Shards;
using Avalon.Utility;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.MongoAccess
{
    public class MongoSettingProvider : ISettingProvider
    {
        const string SettingCollectionName = "settings";

        MongoCollection collection;

        public void Init(SettingNode node)
        {
            var id = node.TryGetValue("shardId");
            if (String.IsNullOrEmpty(id))
                throw new AvalonException("需要属性 shardId");
            ShardId shardId = new ShardId(id);

            var manager = (MongoManager)RepositoryFramework.GetSessionFactoryByFactoryType<MongoShardSessionFactory>().GetConnectionManager();
            collection = manager.GetConnection(shardId).GetCollection("settings");
            if (collection == null)
                throw new ArgumentNullException("collection");
        }

        public ISetting Load(string id, Type settingType)
        {
            Arguments.NotNull(id, "id");
            IMongoQuery query = Query.EQ("_id", BsonValue.Create(id));
            if (collection == null)
                throw new ArgumentNullException("collection");
            return (ISetting)collection.FindOneAs(settingType, query);
        }

        public void Save(ISetting setting)
        {
            Arguments.NotNull(setting.Id, "Id");
            collection.Save(setting, SafeMode.True);
        }

        public T Load<T>(string id) where T : ISetting
        {
            Arguments.NotNull(id, "id");
            IMongoQuery query = Query.EQ("_id", BsonValue.Create(id));
            if (collection == null)
                throw new ArgumentNullException("collection");
            return collection.FindOneAs<T>(query);
        }
    }
}
