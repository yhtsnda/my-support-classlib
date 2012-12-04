using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

using Projects.Tool;

namespace Projects.Framework
{
    public class RepositoryInterceptor : IInterceptor
    {
        private ILog mLog = LogManager.GetLogger<RepositoryInterceptor>();
        private static List<IRepositoryMethodInvocation> mInvocations;

        static RepositoryInterceptor()
        {
            mInvocations = new List<IRepositoryMethodInvocation>
            {
                new RepositoryPersistentMethodInvocation(),
                new RepositoryGetMethodInvocation(),
                new RepositoryGetListMethodInvocation()
            };
        }

        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
