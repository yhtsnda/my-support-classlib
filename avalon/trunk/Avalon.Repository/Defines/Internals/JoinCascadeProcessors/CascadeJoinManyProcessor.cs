using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal class CascadeJoinManyProcessor<TEntity, TJoin> : IClassJoinCascadeProcessor
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
        /// 获取关联对象集合
        /// </summary>
        public Func<TEntity, IList<TJoin>> JoinFunc { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public Func<TEntity, IList<TJoin>> SourceJoinFunc { get; set; }

        /// <summary>
        /// 关联对象的主键
        /// </summary>
        public Func<TJoin, object> JoinKeyFunc { get; set; }

        /// <summary>
        /// 关联对象的排序(可空)
        /// </summary>
        public Action<TJoin, int> JoinSortAction { get; set; }


        void OnCreate(TEntity entity)
        {
            var repository = DependencyResolver.Resolve<IRepository<TJoin>>();

            var key = KeyFunc(entity);
            var items = JoinFunc(entity);
            int sort = 0;
            foreach (var item in items)
            {
                ForeignKeyAction(item, key);
                if (JoinSortAction != null)
                    JoinSortAction(item, sort);

                var itemKey = JoinKeyFunc(item);
                if (CascadeImpl.IsDefault(itemKey))
                    repository.Create(item);
                else
                    repository.Update(item);
                sort++;
            }
        }

        void OnUpdate(TEntity entity)
        {
            var repository = DependencyResolver.Resolve<IRepository<TJoin>>();

            var key = KeyFunc(entity);
            var items = JoinFunc(entity);

            var sources = SourceJoinFunc(entity);

            var newItems = items.Where(o => CascadeImpl.IsDefault(JoinKeyFunc(o)));
            var updateItems = items.Except(newItems);
            var deleteItems = sources.Except(items, new EntityEqualityComparer<TJoin>(JoinKeyFunc));

            foreach (var newItem in newItems)
            {
                ForeignKeyAction(newItem, key);
                if (JoinSortAction != null)
                    JoinSortAction(newItem, items.IndexOf(newItem));

                repository.Create(newItem);
            }

            foreach (var updateItem in updateItems)
            {
                ForeignKeyAction(updateItem, key);
                if (JoinSortAction != null)
                    JoinSortAction(updateItem, items.IndexOf(updateItem));

                repository.Update(updateItem);
            }
            foreach (var deleteItem in deleteItems)
            {
                repository.Delete(deleteItem);
            }
        }

        void OnDelete(TEntity entity)
        {
            var repository = DependencyResolver.Resolve<IRepository<TJoin>>();
            var items = JoinFunc(entity);
            foreach (var item in items)
            {
                repository.Delete(item);
            }
        }

        void IClassJoinCascadeProcessor.OnCreate(object entity)
        {
            this.OnCreate((TEntity)entity);
        }

        void IClassJoinCascadeProcessor.OnUpdate(object entity)
        {
            this.OnUpdate((TEntity)entity);
        }

        void IClassJoinCascadeProcessor.OnDelete(object entity)
        {
            this.OnDelete((TEntity)entity);
        }

        class EntityEqualityComparer<T> : IEqualityComparer<T>
        {
            Func<T, object> idAccess;
            public EntityEqualityComparer(Func<T, object> idAccess)
            {
                this.idAccess = idAccess;
            }

            public bool Equals(T x, T y)
            {
                return idAccess(x).Equals(idAccess(y));
            }

            public int GetHashCode(T obj)
            {
                return idAccess(obj).GetHashCode();
            }
        }
    }
}
