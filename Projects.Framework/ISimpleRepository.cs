using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Pager;

namespace Projects.Framework
{
    /// <summary>
    /// 简单仓储接口
    /// </summary>
    /// <remarks>
    /// 在简单仓储接口中,不需要实现规约
    /// </remarks>
    public interface ISimpleRepository<TEntity, TSearch>
        where TEntity : class
        where TSearch : class
    {
        ActionResult<ResultKey, TEntity> Create(TEntity entity);

        ResultKey Update(TEntity entity);

        ResultKey Delete(TEntity entity);

        TEntity Get(object key);

        TEntity FindOne(TSearch condition);

        IList<TEntity> FindAll(TSearch condition);

        PagedList<TEntity> FindPaging(TSearch condition, int pageIndex = 1, int pageSize = 20);
    }
}
