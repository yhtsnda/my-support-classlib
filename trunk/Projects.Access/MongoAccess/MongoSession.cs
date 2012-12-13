﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using Projects.Tool;
using Projects.Tool.Pager;
using Projects.Framework.Shards;
using Projects.Framework.Specification;

namespace Projects.Accesses.MongoAccess
{
    public class MongoSession
    {
        SafeMode safeMode = SafeMode.True;
        MongoManager manager;

        internal MongoSession(MongoManager manager)
        {
            this.manager = manager;
        }

        public MongoCollection GetCollection<T>(ShardParams shardParams)
        {
            return manager.GetCollection<T>(shardParams);
        }

        public IQueryable<T> CreateQuery<T>(ShardParams shardParams)
        {
            return new MongoQueryable<T>(new MongoQueryExecutor(this, shardParams));
        }

        public void Create<T>(ShardParams shardParams, T entity)
        {
            var getter = BsonClassMap.LookupClassMap(entity.GetType()).IdMemberMap.Getter;
            GetCollection<T>(shardParams).Insert(entity, safeMode);
        }

        public void Update<T>(ShardParams shardParams, T entity)
        {
            var getter = BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.Getter;
            GetCollection<T>(shardParams).Save<T>(entity, safeMode);
        }

        public void Delete<T>(ShardParams shardParams, T entity)
        {
            var getter = BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.Getter;
            var id = getter(entity);
            GetCollection<T>(shardParams).Remove(Query.EQ("_id", BsonValue.Create(id)), safeMode);
        }

        public T Get<T>(ShardParams shardParams, object id)
        {
            return GetCollection<T>(shardParams).FindOneByIdAs<T>(BsonValue.Create(id));
        }

        public virtual IList<T> GetList<T>(ShardParams shardParams, IEnumerable ids)
        {
            IMongoQuery query = Query.In("_id", BsonArray.Create(ids));
            return GetCollection<T>(shardParams).FindAs<T>(query).ToList();
        }

        public virtual T FindOne<T>(ISpecification<T> spec)
        {
            return ((MongoSpecification<T>)spec).Query.FirstOrDefault();
        }

        public virtual IList<T> FindAll<T>(ISpecification<T> spec)
        {
            return ((MongoSpecification<T>)spec).Query.ToList();
        }

        public virtual PagedList<T> FindPaging<T>(ISpecification<T> spec)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            int count = 0;
            if (mongoSpec.Take > 0)
            {
                var cq = mongoSpec.GetQuery();
                if (mongoSpec.CriteriaExpression != null)
                    cq = cq.Where(mongoSpec.CriteriaExpression);
                count = cq.Count();
            }
            PagedList<T> result = new PagedList<T>(count);
            result.AddRange(mongoSpec.Query.ToList());
            return result;
        }

        public virtual int Count<T>(ISpecification<T> spec)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            var cq = mongoSpec.GetQuery();
            if (mongoSpec.CriteriaExpression != null)
                cq = cq.Where(mongoSpec.CriteriaExpression);

            return cq.Count();
        }

        public void AddToSet<TEntity, TSet>(ShardParams shardParams, TEntity entity, Expression<Func<TEntity, object>> expr, IEnumerable<TSet> setIds)
        {
            var classMap = BsonClassMap.LookupClassMap(entity.GetType());
            //查询条件
            var query = Query.EQ(classMap.IdMemberMap.ElementName, BsonValue.Create(classMap.IdMemberMap.Getter(entity)));
            //需要批量更新的字段名
            var setName = classMap.GetMemberMap((expr.Body as MemberExpression).Member.Name).ElementName;
            //执行的更新语句
            var update = MongoDB.Driver.Builders.Update.AddToSetEachWrapped<TSet>(setName, setIds);
            //执行语句
            GetCollection<TEntity>(shardParams).Update(query, update, SafeMode.True);
        }

        /// <summary>
        /// expr为获取 lockVersion的表达式
        /// LockValue从1开始。0用来兼容旧的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public bool UpdateForOptimisticLock<T>(ShardParams shardParams, T entity, Expression<Func<T, byte>> expr, Func<T, bool> newFunc)
        {
            if (newFunc(entity))
            {
                Create(shardParams, entity);
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

                var result = GetCollection<T>(shardParams).FindAndModify(query, SortBy.Null, update, false, false);
                return !(result.Response.GetValue(0) is BsonNull);
            }
        }
    }
}
