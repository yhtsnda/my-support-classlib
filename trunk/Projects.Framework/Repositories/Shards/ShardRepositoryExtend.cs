using Projects.Framework.Shards;
using Projects.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 不具备分区的仓储
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface INoShardRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 通过对象标识获取对象
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(object id);

        /// <summary>
        /// 通过对象标识批量获取对象
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(IEnumerable ids);
    }

    /// <summary>
    /// 使用一个参数进行分区的仓储
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TParam">The type of the param.</typeparam>
    public interface IShardedRepositoy<TEntity, TParam> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 创建一个规约对象
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns></returns>
        ISpecification<TEntity> CreateSpecification(TParam param);

        /// <summary>
        /// 通过对象标识获取对象
        /// </summary>
        /// <param name="param">The param.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(TParam param, object id);

        /// <summary>
        /// 通过对象标识批量获取对象
        /// </summary>
        /// <param name="param">The param.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(TParam param, IEnumerable ids);

        /// <summary>
        /// Gets the shard params.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns></returns>
        ShardParams GetShardParams(TParam param);
    }

    /// <summary>
    /// 使用两个参数进行分区的仓储
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TParam1">The type of the param1.</typeparam>
    /// <typeparam name="TParam2">The type of the param2.</typeparam>
    public interface IShardedRepositoy<TEntity, TParam1, TParam2> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 创建一个规约对象
        /// </summary>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <returns></returns>
        ISpecification<TEntity> CreateSpecification(TParam1 param1, TParam2 param2);

        /// <summary>
        /// 通过对象标识获取对象
        /// </summary>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(TParam1 param1, TParam2 param2, object id);

        /// <summary>
        /// 通过对象标识批量获取对象
        /// </summary>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(TParam1 param1, TParam2 param2, IEnumerable ids);

        /// <summary>
        /// Gets the shard params.
        /// </summary>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <returns></returns>
        ShardParams GetShardParams(TParam1 param1, TParam2 param2);
    }

    public abstract class AbstractNoShardRepository<TEntity> : AbstractRepository<TEntity>, INoShardRepository<TEntity>
      where TEntity : class
    {
        public virtual TEntity Get(object id)
        {
            return Get(ShardParams.Empty, id);
        }

        public virtual IList<TEntity> GetList(IEnumerable ids)
        {
            return GetList(ShardParams.Empty, ids);
        }

        protected override ShardParams GetShardParams(TEntity entity)
        {
            return ShardParams.Empty;
        }
    }

    public abstract class AbstractShardedRepository<TEntity, TParam> : AbstractRepository<TEntity>, IShardedRepositoy<TEntity, TParam> where TEntity : class
    {
        public virtual ISpecification<TEntity> CreateSpecification(TParam param)
        {
            return CreateSpecification(GetShardParams(param));
        }

        public virtual TEntity Get(TParam param, object id)
        {
            return Get(GetShardParams(param), id);
        }

        public virtual IList<TEntity> GetList(TParam param, IEnumerable ids)
        {
            return GetList(GetShardParams(param), ids);
        }

        public abstract ShardParams GetShardParams(TParam param);
    }

    public abstract class AbstractShardedRepository<TEntity, TParam1, TParam2> : AbstractRepository<TEntity>, IShardedRepositoy<TEntity, TParam1, TParam2> where TEntity : class
    {
        public virtual ISpecification<TEntity> CreateSpecification(TParam1 param1, TParam2 param2)
        {
            return CreateSpecification(GetShardParams(param1, param2));
        }

        public virtual TEntity Get(TParam1 param1, TParam2 param2, object id)
        {
            return Get(GetShardParams(param1, param2), id);
        }

        public virtual IList<TEntity> GetList(TParam1 param1, TParam2 param2, IEnumerable ids)
        {
            return GetList(GetShardParams(param1, param2), ids);
        }

        public abstract ShardParams GetShardParams(TParam1 param1, TParam2 param2);
    }
}
