using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 不具有分区能力的仓储
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IShardRepository<TEntity> : IRepository<TEntity> where TEntity : class
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
    /// 通过单元标识进行分区的仓储
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IUnitShardRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 创建一个规约对象
        /// </summary>
        /// <param name="unitId">The unit id.</param>
        /// <returns></returns>
        ISpecification<TEntity> CreateSpecification(int unitId);

        /// <summary>
        /// 通过对象标识获取对象
        /// </summary>
        /// <param name="unitId">The unit id.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(int unitId, object id);

        /// <summary>
        /// 通过对象标识批量获取对象
        /// </summary>
        /// <param name="unitId">The unit id.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(int unitId, IEnumerable ids);
    }

    public interface ICourseShardRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 创建一个规约对象
        /// </summary>
        /// <param name="unitId">The course id.</param>
        /// <returns></returns>
        ISpecification<TEntity> CreateSpecification(int courseId);


        /// <summary>
        /// 通过对象标识获取对象
        /// </summary>
        /// <param name="unitId">The course id.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(int courseId, object id);

        /// <summary>
        /// 通过对象标识批量获取对象
        /// </summary>
        /// <param name="unitId">The course id.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(int courseId, IEnumerable ids);
    }

    /// <summary>
    /// 通过单元标识及用户标识进行分区的仓储
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IUnitUserShardRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 创建一个规约对象
        /// </summary>
        /// <param name="unitId">The unit id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        ISpecification<TEntity> CreateSpecification(int unitId, long userId);

        /// <summary>
        /// 通过对象标识获取对象
        /// </summary>
        /// <param name="unitId">The unit id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(int unitId, long userId, object id);

        /// <summary>
        /// 通过对象标识批量获取对象
        /// </summary>
        /// <param name="unitId">The unit id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(int unitId, long userId, IEnumerable ids);
    }

    /// <summary>
    /// 通过用户标识进行分区的仓储
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IUserShardRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 创建一个规约对象
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        ISpecification<TEntity> CreateSpecification(long userId);

        /// <summary>
        /// 通过对象标识获取对象
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity Get(long userId, object id);

        /// <summary>
        /// 通过对象标识批量获取对象
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IList<TEntity> GetList(long userId, IEnumerable ids);
    }
}
