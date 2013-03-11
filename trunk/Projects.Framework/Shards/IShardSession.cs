using Projects.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Shards
{
    /// <summary>
    /// 分区后的回话接口
    /// </summary>
    public interface IShardSession<TEntity> : IDisposable
    {
        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void SessionEvict(TEntity entity);

        bool SessionContains(TEntity entity);

        TEntity Get(object id);

        IList<TEntity> GetList(IEnumerable ids);

        TEntity FindOne(ISpecification<TEntity> spec);

        IList<TEntity> FindAll(ISpecification<TEntity> spec);

        PagingResult<TEntity> FindPaging(ISpecification<TEntity> spec);

        int Count(ISpecification<TEntity> spec);
    }
}
