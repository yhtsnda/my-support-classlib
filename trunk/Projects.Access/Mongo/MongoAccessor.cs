using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace Projects.Accesses.MongoAccess
{
    public class MongoAccessor
    {
        public static IQueryable<T> GetQuery<T>()
        {
            return new MongoQueryable<T>();
        }

        public static MongoCollection GetCollection<T>()
        {
            return MongoManager.GetCollection<T>();
        }

        public static void Delete<T>(T entry)
        {
            var getter = BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.Getter;
            var id = getter(entry);
            GetCollection<T>().Remove(Query.EQ("_id", BsonValue.Create(id)), SafeMode.True);
        }

        public static void Insert<T>(T entry)
        {
            GetCollection<T>().Insert<T>(entry, SafeMode.True);
        }

        public static void InsertBatch<T>(IEnumerable<T> list)
        {
            GetCollection<T>().InsertBatch<T>(list, SafeMode.True);
        }

        public static void Update<T>(T entry)
        {
            GetCollection<T>().Save<T>(entry, SafeMode.True);
        }

        public static void AddToSet<TEntry, TSet>(TEntry entry, Expression<Func<TEntry,object>> expr,
            IEnumerable<TSet> setIds)
        {
            var classMap = BsonClassMap.LookupClassMap(entry.GetType());
            var query = Query.EQ(classMap.IdMemberMap.ElementName, 
                BsonValue.Create(classMap.IdMemberMap.Getter(entry)));
            var setName = classMap.GetMemberMap((expr.Body as MemberExpression).Member.Name).ElementName;
            var update = MongoDB.Driver.Builders.Update.AddToSetEachWrapped<TSet>(setName, setIds);
            GetCollection<TEntry>().Update(query, update, SafeMode.True);
        }

        public static bool UpdateForOptimisticLock<T>(T entity, Expression<Func<T, byte>> expr, Func<T, bool> newFunc)
        {
            if (newFunc(entity))
            {
                Insert(entity);
                return true;
            }
            else
            {
                var classMap = BsonClassMap.LookupClassMap(entity.GetType());
                var lockVersionMap = classMap.GetMemberMap((expr.Body as MemberExpression).Member.Name);
                var oldLockValue = Convert.ToByte(lockVersionMap.Getter(entity));
                var newLockValue = oldLockValue == byte.MaxValue ? 1 : oldLockValue + 1;
                //query
                var mongoQuery = new List<IMongoQuery> { };
                mongoQuery.Add(Query.EQ(classMap.IdMemberMap.ElementName, BsonValue.Create(classMap.IdMemberMap.Getter(entity))));
                if (oldLockValue != 0)
                    mongoQuery.Add(Query.EQ(lockVersionMap.ElementName, (byte)lockVersionMap.Getter(entity)));
                var query = Query.And(mongoQuery.ToArray());
                //update
                UpdateBuilder update = MongoDB.Driver.Builders.Update.Set(lockVersionMap.ElementName, newLockValue);
                foreach (var memberMap in classMap.MemberMaps)
                {
                    if (memberMap.ElementName != classMap.IdMemberMap.ElementName && memberMap.ElementName != lockVersionMap.ElementName)
                        update = update.Set(memberMap.ElementName, BsonValue.Create((memberMap.Getter(entity))));
                }

                var result = GetCollection<T>().FindAndModify(query, SortBy.Null, update, false, false);
                return !(result.Response.GetValue(0) is BsonNull);
            }
        }
    }
}
