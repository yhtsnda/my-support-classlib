using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

namespace Projects.Framework
{
    internal class RepositoryGetListMethodInvocation : IRepositoryMethodInvocation
    {
        public bool IsMatch(IInvocation invocation)
        {
            throw new NotImplementedException();
        }

        public void Process(IInvocation invocation, ClassDefineMetadata metadata)
        {
            throw new NotImplementedException();
        }
    }
}
