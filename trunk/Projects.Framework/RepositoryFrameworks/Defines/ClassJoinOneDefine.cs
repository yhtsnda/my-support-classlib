using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    public class ClassJoinOneDefine<TEntity, TJoin> : ClassJoinDefine<TEntity, TJoin> where TJoin : class
    {
        ClassJoinOneCascadeDefine<TEntity, TJoin> cascadeDefine;
        ClassJoinCacheDefine<TEntity, TJoin> cacheDefine;

        protected internal ClassJoinOneDefine(ClassDefineMetadata metadata, MemberInfo member, Action<TEntity, HasOneByForeignKeyDefine> foreignKeyDefine)
            : base(metadata, ClassJoinType.HasOneByForeignKey, member, foreignKeyDefine)
        {
            cascadeDefine = new ClassJoinOneCascadeDefine<TEntity, TJoin>(JoinMetadata);
            cacheDefine = new ClassJoinCacheDefine<TEntity, TJoin>(JoinMetadata.JoinCache);
        }

        protected internal ClassJoinOneDefine(ClassDefineMetadata metadata, MemberInfo member, Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> joinExpr) :
            base(metadata, ClassJoinType.HasOne, member, joinExpr)
        {
            cascadeDefine = new ClassJoinOneCascadeDefine<TEntity, TJoin>(JoinMetadata);
            cacheDefine = new ClassJoinCacheDefine<TEntity, TJoin>(JoinMetadata.JoinCache);
        }

        public ClassJoinOneDefine<TEntity, TJoin> Cache(Action<ClassJoinCacheDefine<TEntity, TJoin>> cd)
        {
            OnCache();
            cd(cacheDefine);
            return this;
        }

        public ClassJoinOneDefine<TEntity, TJoin> Cascade(Action<ClassJoinOneCascadeDefine<TEntity, TJoin>> cd)
        {
            OnCascade();
            cd(cascadeDefine);
            return this;
        }
    }
}
