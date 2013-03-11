using Projects.Framework;
using Projects.Framework.Shards;
using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public abstract class AbstractShardRepository<TEntity> : AbstractRepository<TEntity>, IShardRepository<TEntity>
        where TEntity : class
    {
        public virtual TEntity Get(object id)
        {
            return Get(ShardParams.Empty, id);
        }

        public virtual IList<TEntity> GetList(System.Collections.IEnumerable ids)
        {
            return GetList(ShardParams.Empty, ids);
        }

        protected override ShardParams GetShardParams(TEntity entity)
        {
            return ShardParams.Empty;
        }
    }

    public abstract class AbstractUnitShardRepository<TEntity> : AbstractRepository<TEntity>, IUnitShardRepository<TEntity>
        where TEntity : class
    {
        public virtual ISpecification<TEntity> CreateSpecification(int unitId)
        {
            return CreateSpecification(ShardParams.Form(unitId));
        }

        public virtual TEntity Get(int unitId, object id)
        {
            return Get(ShardParams.Form(unitId), id);
        }

        public virtual IList<TEntity> GetList(int unitId, System.Collections.IEnumerable ids)
        {
            return GetList(ShardParams.Form(unitId), ids);
        }
    }

    public abstract class AbstractCourseShardRepository<TEntity> : AbstractRepository<TEntity>, ICourseShardRepository<TEntity>
        where TEntity : class
    {
        public virtual ISpecification<TEntity> CreateSpecification(int courseId)
        {
            return CreateSpecification(ShardParams.Form(courseId));
        }

        public virtual TEntity Get(int courseId, object id)
        {
            return Get(ShardParams.Form(courseId), id);
        }

        public virtual IList<TEntity> GetList(int courseId, System.Collections.IEnumerable ids)
        {
            return GetList(ShardParams.Form(courseId), ids);
        }
    }

    public abstract class AbstractUnitUserShardRepository<TEntity> : AbstractRepository<TEntity>, IUnitUserShardRepository<TEntity>
        where TEntity : class
    {
        public virtual ISpecification<TEntity> CreateSpecification(int unitId, long userId)
        {
            return CreateSpecification(ShardParams.Form(unitId, userId));
        }

        public virtual TEntity Get(int unitId, long userId, object id)
        {
            return Get(ShardParams.Form(unitId, userId), id);
        }

        public virtual IList<TEntity> GetList(int unitId, long userId, System.Collections.IEnumerable ids)
        {
            return GetList(ShardParams.Form(unitId, userId), ids);
        }
    }

    public abstract class AbstractUserShardRepository<TEntity> : AbstractRepository<TEntity>, IUserShardRepository<TEntity>
        where TEntity : class
    {
        public virtual ISpecification<TEntity> CreateSpecification(long userId)
        {
            return CreateSpecification(ShardParams.Form(userId));
        }

        public virtual TEntity Get(long userId, object id)
        {
            return Get(ShardParams.Form(userId), id);
        }

        public virtual IList<TEntity> GetList(long userId, System.Collections.IEnumerable ids)
        {
            return GetList(ShardParams.Form(userId), ids);
        }
    }
}
