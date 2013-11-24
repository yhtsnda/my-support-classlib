using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.MongoAccess
{
    public abstract class MongoMap<TEntity>
    {
        public void RegisterClassMap(Action<BsonClassMap<TEntity>> classMapInitializer)
        {
            BsonClassMap.RegisterClassMap(classMapInitializer);
        }
    }
}
