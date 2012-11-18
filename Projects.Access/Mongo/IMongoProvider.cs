using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

namespace Projects.Accesses.MongoAccess
{
    public interface IMongoProvider
    {
        MongoCollection GetCollection(string database, string collection);
        MongoDatabase GetDatabase(string database);
    }
}
