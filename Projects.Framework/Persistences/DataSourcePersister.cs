using Castle.DynamicProxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class DataSourcePersister : AbstractPersister
    {
        IInvocation invocation;

        public DataSourcePersister(IInvocation invocation, IPersister innerPersister)
            : base(innerPersister)
        {
            this.invocation = invocation;
        }

        protected override object GetInner(string cacheKey)
        {
            invocation.Proceed();
            return invocation.ReturnValue;
        }

        protected override IEnumerable GetListInner(DataWrapper wrapper)
        {
            if (wrapper.HasMiss)
            {
                var ids = wrapper.GetMissIds();
                invocation.SetArgumentValue(1, ids);
                invocation.Proceed();
                return (IEnumerable)invocation.ReturnValue;
            }
            return new ArrayList();
        }

        public override void Set(string cacheKey, object entity)
        {
        }

        public override void SetBatch(IEnumerable<DataItem> items)
        {
        }
    }
}
