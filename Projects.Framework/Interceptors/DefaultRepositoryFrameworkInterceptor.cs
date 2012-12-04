using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class DefaultRepositoryFrameworkInterceptor : IRepositoryFrameworkInterceptor
    {
        public void PreUpdate(object entity)
        {
            throw new NotImplementedException();
        }

        public void PreCreate(object entity)
        {
            throw new NotImplementedException();
        }

        public void PreDelete(object entity)
        {
            throw new NotImplementedException();
        }

        public void PostUpdate(object entity)
        {
            throw new NotImplementedException();
        }

        public void PostCreate(object entity)
        {
            throw new NotImplementedException();
        }

        public void PostDelete(object entity)
        {
            throw new NotImplementedException();
        }

        public void PostLoad(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
