using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

using Projects.Tool.Reflection;

namespace Projects.Tool.MongoAccess
{
    internal class MongoManager
    {
        static MongoDatabase mDatabase;
        static object mSyncObj = new object();
        static IMongoProvider mProvider;

        static MongoManager()
        {
            var type = ToolSection.Instance.TryGetType("mongoAccess/provider");
            if (type != null)
            {
                mProvider = (IMongoProvider)FastActivator.Create(type);
            }
            else
            {
                mProvider = new DefaultMongoProvider();
            }
        }

        public static MongoCollection GetCollection(Type entityType)
        {
            CollectionNameAttribute attr = (CollectionNameAttribute)Attribute
                .GetCustomAttribute(entityType, typeof(CollectionNameAttribute));

            if (attr != null)
            {
                return mProvider.GetCollection(attr.Database, attr.Name);
            }
            return mProvider.GetCollection(null, entityType.Name);
        }

        public static MongoCollection GetCollection<T>()
        {
            return GetCollection(typeof(T));
        }
    }
}
