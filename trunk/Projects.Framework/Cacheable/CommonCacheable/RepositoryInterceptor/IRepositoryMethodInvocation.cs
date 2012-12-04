using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

namespace Projects.Framework
{
    public interface IRepositoryMethodInvocation
    {
        bool IsMatch(IInvocation invocation);

        void Process(IInvocation invocation, ClassDefineMetadata metadata);
    }
}
