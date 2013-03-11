using Castle.DynamicProxy;
using Projects.Tool;
using Projects.Tool.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework
{
    internal class RepositoryPersistentMethodInvocation : IRepositoryMethodInvocation
    {
        const string LoopDetectKey = "_LoopDetectKey_";

        public bool IsMatch(IInvocation invocation)
        {
            var method = invocation.Method.Name;
            return method == "Create" || method == "Update" || method == "Delete";
        }

        public void Process(IInvocation invocation, ClassDefineMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            var entity = invocation.Arguments[0];
            if (entity == null)
                throw new ArgumentNullException("entity");

            var interceptors = RepositoryFramework.GetInterceptors(metadata.EntityType);

            //invoke pre event
            var preUr = new Projects.Framework.RepositoryFramework.UniqueRaise(GetRaiseType(invocation, true), metadata, entity, false);
            var postUr = new Projects.Framework.RepositoryFramework.UniqueRaise(GetRaiseType(invocation, false), metadata, entity, false);

            //invoke cascade delete or update
            var preRaiseType = GetRaiseType(invocation, true);
            if (preRaiseType == RepositoryFramework.RaiseType.PreDelete || preRaiseType == RepositoryFramework.RaiseType.PreUpdate)
                Cascade(invocation, entity);

            var newEntity = RepositoryFramework.Raise(entity, preRaiseType, preUr);
            EntityUtil.MergeObject(newEntity, entity);

            invocation.Proceed();

            //invoke post event
            var postRaiseType = GetRaiseType(invocation, false);
            newEntity = RepositoryFramework.Raise(entity, postRaiseType, postUr);
            EntityUtil.MergeObject(newEntity, entity);

            //invoke cascade create
            if (postRaiseType == RepositoryFramework.RaiseType.PostCreate)
                Cascade(invocation, entity);

            preUr.Dispose();
            postUr.Dispose();
        }

        void Cascade(IInvocation invocation, object entity)
        {
            switch (invocation.Method.Name)
            {
                case "Create":
                    CascadeImpl.OnCreate(entity);
                    break;
                case "Update":
                    CascadeImpl.OnUpdate(entity);
                    break;
                case "Delete":
                    CascadeImpl.OnDelete(entity);
                    break;
            }
        }

        Projects.Framework.RepositoryFramework.RaiseType GetRaiseType(IInvocation invocation, bool pre)
        {
            switch (invocation.Method.Name)
            {
                case "Create":
                    return pre ? RepositoryFramework.RaiseType.PreCreate : RepositoryFramework.RaiseType.PostCreate;
                case "Update":
                    return pre ? RepositoryFramework.RaiseType.PreUpdate : RepositoryFramework.RaiseType.PostUpdate;
                case "Delete":
                    return pre ? RepositoryFramework.RaiseType.PreDelete : RepositoryFramework.RaiseType.PostDelete;
            }
            return RepositoryFramework.RaiseType.Unknown;
        }
    }
}
