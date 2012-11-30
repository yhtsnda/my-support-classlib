using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;
using Projects.Tool;
using Projects.Tool.Pager;
using Projects.Accesses;

namespace Projects.Repository
{
    /// <summary>
    /// 基于IBatis的仓储类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AbstractIBatisRepository<TEntity> 
        : IRepository<TEntity> where TEntity : class
    {
        protected abstract ShardParams GetShardParams(TEntity entity);

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual ActionResult<ResultKey, TEntity> Create(TEntity entity)
        {
            //获取对象的类型的字符串描述
            string entityType = typeof (TEntity).Name;
            SqlMapperManager.Instance.DefaultMapper.Insert("Create_" + entityType, entity);
            return new ActionResult<ResultKey, TEntity>()
                       {Result = ResultKey.OK, Message = "数据库操作成功"};
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual ActionResult<ResultKey, object> Update(TEntity entity)
        {
            //获取对象的类型的字符串描述
            string entityType = typeof(TEntity).Name;
            SqlMapperManager.Instance.DefaultMapper.Update("Update_" + entityType, entity);
            return new ActionResult<ResultKey, object>()
                       {Result = ResultKey.OK, Message = "数据库操作成功"};
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual ActionResult<ResultKey, object> Delete(TEntity entity)
        {
            //获取对象的类型的字符串描述
            string entityType = typeof(TEntity).Name;
            SqlMapperManager.Instance.DefaultMapper.Delete("Delete_" + entityType, entity);
            return new ActionResult<ResultKey, object>() { Result = ResultKey.OK, Message = "数据库操作成功" };
        }

        /// <summary>
        /// 根据主键ID获取对象
        /// </summary>
        /// <param name="id">主键对象</param>
        /// <returns></returns>
        public virtual TEntity Get(object id)
        {
            //获取对象的类型的字符串描述
            string entityType = typeof(TEntity).Name;
            return SqlMapperManager.Instance.DefaultMapper.QueryForObject<TEntity>("QueryOneByKey_" + entityType, id); 
        }

        /// <summary>
        /// 根据一组主键值获取对象的列表
        /// </summary>
        /// <param name="ids">主键对象</param>
        /// <returns></returns>
        public virtual IList<TEntity> GetList(IEnumerable<int> ids)
        {
            List<TEntity> output = new List<TEntity>();
            string strIds = String.Empty;
            List<int> lstIds = ids.ToList();
            strIds = lstIds.Aggregate(strIds, (current, lstId) => current + (lstId.ToString() + ","));

            if (!String.IsNullOrEmpty(strIds))
            {
                strIds = strIds.TrimEnd(',');
                output = (List<TEntity>)SqlMapperManager.Instance.DefaultMapper
                    .QueryForList<TEntity>("QueryListByKeys_" + typeof(TEntity).Name, strIds);
            }
            return output;
        }

        /// <summary>
        /// 根据条件规约查找一条记录
        /// </summary>
        /// <param name="spec">条件规约</param>
        /// <returns></returns>
        public virtual TEntity FindOne(ISpecification<TEntity> spec)
        {
            if (String.IsNullOrEmpty(spec.QueryStatement))
                spec.QueryStatement = "Query_" + typeof (TEntity).Name;
            return ((IBatisSpecification<TEntity>)spec).Query.FirstOrDefault();
        }

        /// <summary>
        /// 根据条件规约查找所有记录
        /// </summary>
        /// <param name="spec">条件规约</param>
        /// <returns></returns>
        public virtual IList<TEntity> FindAll(ISpecification<TEntity> spec)
        {
            string entityType = typeof(TEntity).Name;
            if(String.IsNullOrEmpty(spec.QueryStatement)) 
                spec.QueryStatement = "Query_All" + entityType;
            return ((IBatisSpecification<TEntity>) spec).Query.ToList();
        }

        /// <summary>
        /// 根据条件规约查找分页记录
        /// </summary>
        /// <param name="spec">条件规约</param>
        /// <returns></returns>
        public virtual PagedList<TEntity> FindPaging(ISpecification<TEntity> spec)
        {
            IBatisSpecification<TEntity> ibatisSpec = (IBatisSpecification<TEntity>) spec;
            if(String.IsNullOrWhiteSpace(spec.QueryStatement))
                spec.QueryStatement = "Query_" + typeof (TEntity).Name + "_Paging";
            if(String.IsNullOrWhiteSpace(spec.QueryNumberStatement))
                spec.QueryNumberStatement = "Query_" + typeof (TEntity).Name + "_Paging_Record_Number";
            var cq = ibatisSpec.Query;
            var cn = ibatisSpec.QueryRecordCount;

            PagedList<TEntity> result = new PagedList<TEntity>(cq, spec.PageIndex ?? 1, spec.PageSize ?? 12, cn);
            return result;
        }
        
        /// <summary>
        /// 根据条件规约获取记录的数量
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public int GetCount(ISpecification<TEntity> spec)
        {
            string entityType = typeof(TEntity).Name;
            if (String.IsNullOrEmpty(spec.QueryNumberStatement))
                spec.QueryNumberStatement = "Query_" + entityType + "_Count";
            return ((IBatisSpecification<TEntity>) spec).QueryRecordCount;
        }

        /// <summary>
        /// 创建仓储规约
        /// </summary>
        /// <returns></returns>
        public ISpecification<TEntity> CreateSpecification(object searchObj, int? pageIndex = -1, int? pageSize = -1)
        {
            ISpecification<TEntity> specification = SpecificationFactory.Create<TEntity>();
            if(pageIndex != -1 && pageSize != -1)
            {
                specification.PageIndex = pageIndex;
                specification.PageSize = pageSize;
            }
            specification.SearchObject = searchObj;
            return specification;
        }
    }
}
