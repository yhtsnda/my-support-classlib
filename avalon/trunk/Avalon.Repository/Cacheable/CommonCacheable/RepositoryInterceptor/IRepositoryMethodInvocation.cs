using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal interface IRepositoryMethodInvocation
    {
        bool IsMatch(IInvocation invocation);

        void Process(IInvocation invocation, ClassDefineMetadata metadata);
    }
}
