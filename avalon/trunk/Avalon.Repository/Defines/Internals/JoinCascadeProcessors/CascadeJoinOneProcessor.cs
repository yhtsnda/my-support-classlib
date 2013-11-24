using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal class CascadeJoinOneProcessor<TEntity, TJoin> : IClassJoinCascadeProcessor
    {
        /// <summary>
        /// 获取主键值
        /// </summary>
        public Func<TEntity, object> KeyFunc { get; set; }

        /// <summary>
        /// 设置关联对象的外键值
        /// </summary>
        public Action<TJoin, object> ForeignKeyAction { get; set; }

        /// <summary>
        /// 获取关联对象
        /// </summary>
        public Func<TEntity, TJoin> JoinFunc { get; set; }

        /// <summary>
        /// 获取关联对象的主键
        /// </summary>
        public Func<TJoin, object> JoinKeyFunc { get; set; }

        void ValidProperties()
        {
            if (KeyFunc == null)
                throw new ArgumentNullException("KeyFunc", String.Format("TEntity:{0}, TJoin:{1}", typeof(TEntity).FullName, typeof(TJoin).FullName));

            if (JoinFunc == null)
                throw new ArgumentNullException("JoinFunc", String.Format("TEntity:{0}, TJoin:{1}", typeof(TEntity).FullName, typeof(TJoin).FullName));

            if (ForeignKeyAction == null)
                throw new ArgumentNullException("ForeignKeyAction", String.Format("TEntity:{0}, TJoin:{1}", typeof(TEntity).FullName, typeof(TJoin).FullName));

            if (JoinKeyFunc == null)
                throw new ArgumentNullException("JoinKeyFunc", String.Format("TEntity:{0}, TJoin:{1}", typeof(TEntity).FullName, typeof(TJoin).FullName));
        }

        void OnCreate(TEntity entity)
        {
            ValidProperties();

            var repository = DependencyResolver.Resolve<IRepository<TJoin>>();

            var key = KeyFunc(entity);
            var join = JoinFunc(entity);
            if (join != null)
            {
                var joinKey1 = JoinKeyFunc(join);
                ForeignKeyAction(join, key);
                var joinKey2 = JoinKeyFunc(join);

                if (!joinKey1.Equals(joinKey2))
                    repository.Create(join);
                else
                    repository.Update(join);
            }
        }

        void OnUpdate(TEntity entity)
        {
            ValidProperties();

            var repository = DependencyResolver.Resolve<IRepository<TJoin>>();

            var key = KeyFunc(entity);
            var join = JoinFunc(entity);
            if (join != null)
            {
                var joinKey1 = JoinKeyFunc(join);
                ForeignKeyAction(join, key);
                var joinKey2 = JoinKeyFunc(join);

                if (!joinKey1.Equals(joinKey2))
                    repository.Create(join);
                else
                    repository.Update(join);
            }
        }

        void OnDelete(TEntity entity)
        {
            ValidProperties();

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
