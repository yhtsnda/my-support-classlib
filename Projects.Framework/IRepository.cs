using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool;
using Projects.Tool.Util;
using Projects.Tool.Pager;
using Projects.Framework.Shards;

namespace Projects.Framework
{
    public interface IRepository
    {
    }

    /// <summary>
    /// 仓储的接口
    /// </summary>
    public interface IRepository<TEntity> : IRepository
        where TEntity : class
    {
        /// <summary>
        /// 创建仓储的规约
        /// </summary>
        /// <returns></returns>
        ISpecification<TEntity> CreateSpecification(object searchObj, int? pageIndex = -1, int? pageSize = 12);
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="entity">The entity.</param>
        ActionResult<ResultKey, TEntity> Create(TEntity entity);

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">The entity.</param>
        ActionResult<ResultKey, object> Update(TEntity entity);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity">The entity.</param>
        ActionResult<ResultKey, object> Delete(TEntity entity);

        /// <summary>
        /// 通过主键获取对象
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(object id);

        /// <summary>
        /// 通过主键获取对象
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(ShardParams shardParams, object id);

        /// <summary>
        /// 通过主键批量获取对象
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(IEnumerable<int> ids);

        /// <summary>
        /// 通过主键批量获取对象
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(ShardParams shardParams, IEnumerable<int> ids);

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
        PagedList<TEntity> FindPaging(ISpecification<TEntity> spec);

        /// <summary>
        /// 通过规约获取到的记录数量
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        int GetCount(ISpecification<TEntity> spec);
    }
}
