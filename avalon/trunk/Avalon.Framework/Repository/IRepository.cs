using Avalon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public interface IRepository
    {
    }

    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository
    {
        ISpecification<TEntity> CreateSpecification();

        ISpecification<TEntity> CreateSpecification(ShardParams shardParams);

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Create(TEntity entity);

        /// <summary>
        /// 创建对象(联合主键用)
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="id">The id.</param>
        void Create(TEntity entity, object id);

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 从当前会话中移除对象
        /// </summary>
        /// <param name="entity">The entity.</param>
        void SessionEvict(TEntity entity);

        /// <summary>
        /// 判断当前会话时候包含指定的对象。
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        bool SessionContains(TEntity entity);

        /// <summary>
        /// 通过主键获取对象
        /// </summary>
        /// <param name="shardParams">The shard params.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(ShardParams shardParams, object id);

        /// <summary>
        /// 通过主键批量获取对象
        /// </summary>
        /// <param name="shardParams">The shard params.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(ShardParams shardParams, IEnumerable ids);

        /// <summary>
        /// 通过规约查找并返回一个对象
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        TEntity FindOne(ISpecification<TEntity> spec);

        /// <summary>
        /// 通过规约查找对象
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        IList<TEntity> FindAll(ISpecification<TEntity> spec);

        /// <summary>
        /// 通过规约查找对象并进行分页处理
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        PagingResult<TEntity> FindPaging(ISpecification<TEntity> spec);

        /// <summary>
        /// 通过规约查找对象的数量
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        int Count(ISpecification<TEntity> spec);
    }
}
