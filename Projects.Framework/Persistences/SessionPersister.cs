using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class SessionPersister : AbstractPersister
    {
        SessionCache sessionCache;

        public SessionPersister(SessionCache sessionCache, IPersister innerPersister)
            : base(innerPersister)
        {
            this.sessionCache = sessionCache;
        }

        public override void Set(string cacheKey, object entity)
        {
            sessionCache.Set(cacheKey, entity);
        }

        public override void SetBatch(IEnumerable<DataItem> items)
        {
            foreach (var item in items)
                sessionCache.Set(item.Key, item.Data);
        }

        protected override object GetInner(string cacheKey)
        {
            return sessionCache.Get(cacheKey);
        }

        protected override IEnumerable GetListInner(DataWrapper wrapper)
        {
            var keys = wrapper.GetMissKeys();
            return sessionCache.GetList(keys);
        }
    }
}
