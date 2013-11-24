using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal class DefaultRepositoryFrameworkInterceptor : IRepositoryFrameworkInterceptor
    {
        public void PostLoad(object entity)
        {
            var metadata = GetClassDefineMetadata(entity);
            if (metadata != null)
            {
                if (metadata.IsLifecycleImplementor)
                    ((ILifecycle)entity).OnLoaded();
            }
        }

        public void PreUpdate(object entity)
        {
            var metadata = GetClassDefineMetadata(entity);
            if (metadata != null)
            {
                if (metadata.IsLifecycleImplementor)
                    ((ILifecycle)entity).OnSaving(false);

                if (metadata.IsValidatableImplementor)
                    ((IValidatable)entity).Validate();
            }
        }

        public void PreCreate(object entity)
        {
            var metadata = GetClassDefineMetadata(entity);
            if (metadata != null)
            {
                if (metadata.IsLifecycleImplementor)
                    ((ILifecycle)entity).OnSaving(true);

                if (metadata.IsValidatableImplementor)
                    ((IValidatable)entity).Validate();
            }
        }

        public void PreDelete(object entity)
        {
        }

        public void PostUpdate(object entity)
        {
            RaisePersistent(entity);
            var metadata = GetClassDefineMetadata(entity);
            if (metadata != null && metadata.IsLifecycleImplementor)
            {
                ((ILifecycle)entity).OnSaved();
            }
        }

        public void PostCreate(object entity)
        {
            RaisePersistent(entity);
            var metadata = GetClassDefineMetadata(entity);
            if (metadata != null && metadata.IsLifecycleImplementor)
            {
                ((ILifecycle)entity).OnSaved();
            }
        }

        public void PostDelete(object entity)
        {
            RaisePersistent(entity);
        }

        void RaisePersistent(object entity)
        {
            //引发数据变更的操作，通过改操作可以进行缓存及缓存依赖的通知
            RepositoryFramework.RaisePersistent(entity);
        }

        ClassDefineMetadata GetClassDefineMetadata(object entity)
        {
            if (entity != null)
                return RepositoryFramework.GetDefineMetadata(entity.GetType());
            return null;
        }
    }
}
