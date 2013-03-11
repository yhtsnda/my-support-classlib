using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class CascadeJoinOneProcessor<TEntity, TJoin> : IClassJoinCascadeProcessor
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Func<TEntity, object> KeyFunc { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public Action<TJoin, object> ForeignKeyAction { get; set; }

        /// <summary>
        /// 关联对象
        /// </summary>
        public Func<TEntity, TJoin> JoinFunc { get; set; }

        /// <summary>
        /// 关联对象的主键
        /// </summary>
        public Func<TJoin, object> JoinKeyFunc { get; set; }


        void OnCreate(TEntity entity)
        {
            var repository = DependencyResolver.Resolve<IRepository<TJoin>>();

            var key = KeyFunc(entity);
            var join = JoinFunc(entity);
            if (join != null)
            {
                ForeignKeyAction(join, key);
                var joinKey = JoinKeyFunc(join);
                repository.Create(join);
            }
        }

        void OnUpdate(TEntity entity)
        {
            var repository = DependencyResolver.Resolve<IRepository<TJoin>>();

            var key = KeyFunc(entity);
            var join = JoinFunc(entity);
            if (join != null)
            {
                ForeignKeyAction(join, key);
                var joinKey = JoinKeyFunc(join);
                repository.Update(join);
            }
        }

        void OnDelete(TEntity entity)
        {
            var repository = DependencyResolver.Resolve<IRepository<TJoin>>();
            var join = JoinFunc(entity);
            if (join != null)
            {
                repository.Delete(join);
            }
        }

        void IClassJoinCascadeProcessor.OnCreate(object entity)
        {
            OnCreate((TEntity)entity);
        }

        void IClassJoinCascadeProcessor.OnUpdate(object entity)
        {
            OnUpdate((TEntity)entity);
        }

        void IClassJoinCascadeProcessor.OnDelete(object entity)
        {
            OnDelete((TEntity)entity);
        }
    }
}
