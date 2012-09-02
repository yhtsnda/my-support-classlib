using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

using Projects.Tool;
using Projects.Tool.Pager;
using Projects.Framework;

namespace Projects.Repositories
{
    /// <summary>
    /// 简单仓储接口的抽象实现
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    /// <typeparam name="TSearch">查询对象</typeparam>
    public class AbstractSimpleRepository<TEntity, TSearch> : ISimpleRepository<TEntity, TSearch>
        where TEntity : class
        where TSearch : class
    {
        public readonly string MappingKey = ConfigurationManager.AppSettings.AllKeys.Contains("prj_conn") ?
            ConfigurationManager.AppSettings["prj_conn"] : "default";

        /// <summary>
        /// 创建数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual ActionResult<ResultKey, TEntity> Create(TEntity entity)
        {
            string entityType = typeof(TEntity).Name;
            SqlMapperManager.Instance[MappingKey].Insert("Create_" + entityType, entity);
            return new ActionResult<ResultKey, TEntity> { Result = ResultKey.OK, ExtraData = entity };
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual ResultKey Update(TEntity entity)
        {
            string entityType = typeof(TEntity).Name;
            int result = (int)SqlMapperManager.Instance[MappingKey].Update("Update_" + entityType, entity);
            return result >= 0 ? ResultKey.OK : ResultKey.Failure;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultKey Delete(TEntity entity)
        {
            string entityType = typeof(TEntity).Name;
            int result = (int)SqlMapperManager.Instance[MappingKey].Delete("Delete_" + entityType, entity);
            return result >= 0 ? ResultKey.OK : ResultKey.Failure;
        }

        public TEntity Get(object key)
        {
            string entityType = typeof(TEntity).Name;
            return SqlMapperManager.Instance[MappingKey].QueryForObject<TEntity>("QueryOneByKey_" + entityType, key);
        }
        
        /// <summary>
        /// 根据条件查询一条记录
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public TEntity FindOne(TSearch condition)
        {
            string entityType = typeof(TEntity).Name;
            IList<TEntity> result = SqlMapperManager.Instance[MappingKey]
                .QueryForList<TEntity>("Query_" + entityType, condition);

            if (result.Any())
                return result[0];
            return null;
        }

        /// <summary>
        /// 根据条件查询所有记录
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IList<TEntity> FindAll(TSearch condition)
        {
            string entityType = typeof(TEntity).Name;
            IList<TEntity> result =
                SqlMapperManager.Instance[MappingKey].QueryForList<TEntity>("Query_" + entityType, condition);
            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public PagedList<TEntity> FindPaging(TSearch condition, int pageIndex = 1, int pageSize = 20)
        {
            IList<TEntity> result;

            Type enType = typeof(TSearch);
            var method = enType.GetMethod("ToHashtable");
            if (method != null)
            {
                var realCnd = (Hashtable)method.Invoke(condition, null);
                realCnd.Add("Skip", (pageIndex - 1) * pageSize);
                realCnd.Add("Limit", pageSize);
                result = SqlMapperManager.Instance[MappingKey]
                    .QueryForList<TEntity>("Query_" + enType.Name + "_Paging", realCnd);
            }
            else
            {
                result = SqlMapperManager.Instance[MappingKey]
                    .QueryForList<TEntity>("Query_" + enType.Name + "_Paging", condition, pageIndex * pageSize, pageSize);
            }
            int recCount = SqlMapperManager.Instance[MappingKey]
                .QueryForObject<int>("Query_" + enType.Name + "_Count", condition);

            PagedList<TEntity> pagedResult = new PagedList<TEntity>(result.AsQueryable(), pageIndex, pageSize, recCount);
            return pagedResult;
            
        }
    }
}
