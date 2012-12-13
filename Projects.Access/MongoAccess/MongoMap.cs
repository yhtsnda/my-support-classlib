using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson.Serialization;

namespace Projects.Accesses.MongoAccess
{
    public abstract class MongoMap<TEntity>
    {
        public void RegisterClassMap(Action<BsonClassMap<TEntity>> classMapInitializer)
        {
            BsonClassMap.RegisterClassMap(classMapInitializer);
        }
    }
}
