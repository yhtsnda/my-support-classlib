using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal interface IProxyCollection
    {
        void Init(object root, ClassJoinDefineMetadata joinMetadata);
    }

    internal class ProxyCollection<TEntity> : IList<TEntity>, IList, IProxyCollection, IProxy
    {
        //根对象
        object root;
        //源对象
        IList<TEntity> source;

        ClassJoinDefineMetadata joinMetadata;
        bool inited;

        public void Init(object root, ClassJoinDefineMetadata joinMetadata)
        {
            this.root = root;
            this.joinMetadata = joinMetadata;
        }

        public int IndexOf(TEntity item)
        {
            EnsureInited();
            return source.IndexOf(item);
        }

        public void Insert(int index, TEntity item)
        {
            EnsureInited();
            source.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            EnsureInited();
            source.RemoveAt(index);
        }

        public TEntity this[int index]
        {
            get
            {
                EnsureInited();
                return source[index];
            }
            set
            {
                EnsureInited();
                source[index] = value;
            }
        }

        public void Add(TEntity item)
        {
            EnsureInited();
            source.Add(item);
        }

        public void Clear()
        {
            EnsureInited();
            source.Clear();
        }

        public bool Contains(TEntity item)
        {
            EnsureInited();
            return source.Contains(item);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            EnsureInited();
            source.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                EnsureInited();
                return source.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                EnsureInited();
                return source.IsReadOnly;
            }
        }

        public bool Remove(TEntity item)
        {
            EnsureInited();
            return source.Remove(item);
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            EnsureInited();
            return source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            EnsureInited();
            return source.GetEnumerator();
        }

        void EnsureInited()
        {
            if (!inited)
            {
                source = (IList<TEntity>)joinMetadata.DataProcesser.Process(root);
                foreach (var item in (IEnumerable)source)
                    ProxyProvider.ProxyJoins(item);

                inited = true;
            }
        }

        public object GetSource()
        {
            EnsureInited();
            return source;
        }

        int IList.Add(object value)
        {
            EnsureInited();
            return ((IList)source).Add(value);
        }

        void IList.Clear()
        {
            EnsureInited();
            source.Clear();
        }

        bool IList.Contains(object value)
        {
            EnsureInited();
            return source.Contains((TEntity)value);
        }

        int IList.IndexOf(object value)
        {
            EnsureInited();
            return source.IndexOf((TEntity)value);
        }

        void IList.Insert(int index, object value)
        {
            EnsureInited();
            source.Insert(index, (TEntity)value);
        }

        bool IList.IsFixedSize
        {
            get
            {
                EnsureInited();
                return ((IList)source).IsFixedSize;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                EnsureInited();
                return source.IsReadOnly;
            }
        }

        void IList.Remove(object value)
        {
            EnsureInited();
            source.Remove((TEntity)value);
        }

        void IList.RemoveAt(int index)
        {
            EnsureInited();
            source.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                EnsureInited();
                return source[index];
            }
            set
            {
                EnsureInited();
                source[index] = (TEntity)value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            EnsureInited();
            source.CopyTo((TEntity[])array, index);
        }

        int ICollection.Count
        {
            get
            {
                EnsureInited();
                return source.Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                EnsureInited();
                return ((ICollection)source).IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                EnsureInited();
                return ((ICollection)source).SyncRoot;
            }
        }

        #region IProxy 成员


        public void Reset()
        {
            inited = false;
        }

        #endregion
    }
}
