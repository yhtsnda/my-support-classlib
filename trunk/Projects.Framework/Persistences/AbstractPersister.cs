using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal abstract class AbstractPersister : IPersister
    {
        IPersister innerPersister;
        public readonly IEnumerable EmptyEnumerable = new ArrayList();

        public AbstractPersister(IPersister innerPersister)
        {
            this.innerPersister = innerPersister;
        }

        public virtual object Get(string cacheKey)
        {
            object entity = GetInner(cacheKey);
            if (IsEmpty(entity) && innerPersister != null)
            {
                entity = innerPersister.Get(cacheKey);
                if (!IsEmpty(entity))
                    Set(cacheKey, entity);
            }
            return entity;
        }

        public IEnumerable<DataItem> GetList(DataWrapper wrapper)
        {
            List<DataItem> items = new List<DataItem>();

            if (wrapper.HasMiss)
            {
                var entities = GetListInner(wrapper);
                items.AddRange(wrapper.Update(entities));

                if (wrapper.HasMiss && innerPersister != null)
                {
                    var newItems = innerPersister.GetList(wrapper);
                    SetBatch(newItems);
                    items.AddRange(newItems);
                }
            }

            return items;
        }

        protected abstract object GetInner(string cacheKey);

        protected abstract IEnumerable GetListInner(DataWrapper wrapper);

        public abstract void Set(string cacheKey, object entity);

        public abstract void SetBatch(IEnumerable<DataItem> items);

        protected virtual bool IsEmpty(object entity)
        {
            return entity == null;
        }
    }
}
