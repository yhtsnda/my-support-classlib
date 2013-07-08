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
            var preUr = new UniqueRaise(GetRaiseType(invocation, true), metadata, entity, false);
            var postUr = new UniqueRaise(GetRaiseType(invocation, false), metadata, entity, false);

            //invoke cascade delete or update
            var preRaiseType = GetRaiseType(invocation, true);
            if (preRaiseType == RaiseType.PreDelete || preRaiseType == RaiseType.PreUpdate)
                Cascade(invocation, entity);

            RepositoryFramework.Raise(entity, preRaiseType, preUr);

            using (MonitorImpl.Repository(invocation.Method))
                invocation.Proceed();

            //invoke post event
            var postRaiseType = GetRaiseType(invocation, false);
            RepositoryFramework.Raise(entity, postRaiseType, postUr);

            //invoke cascade create
            if (postRaiseType == RaiseType.PostCreate)
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

        RaiseType GetRaiseType(IInvocation invocation, bool pre)
        {
            switch (invocation.Method.Name)
            {
                case "Create":
                    return pre ? RaiseType.PreCreate : RaiseType.PostCreate;
                case "Update":
                    return pre ? RaiseType.PreUpdate : RaiseType.PostUpdate;
                case "Delete":
                    return pre ? RaiseType.PreDelete : RaiseType.PostDelete;
            }
            return RaiseType.Unknown;
        }
    }
}
