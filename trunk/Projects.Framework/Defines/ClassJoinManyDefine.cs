using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    public class ClassJoinManyDefine<TEntity, TJoin> : ClassJoinDefine<TEntity, TJoin> where TJoin : class
    {
        ClassJoinManyCascadeDefine<TEntity, TJoin> cascadeDefine;
        ClassJoinCacheDefine<TEntity, TJoin> cacheDefine;

        protected internal ClassJoinManyDefine(ClassDefineMetadata metadata, MemberInfo member, Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> joinonExpr) :
            base(metadata, ClassJoinType.HasMany, member, joinonExpr)
        {
            cascadeDefine = new ClassJoinManyCascadeDefine<TEntity, TJoin>(JoinMetadata);
            cacheDefine = new ClassJoinCacheDefine<TEntity, TJoin>(JoinMetadata.JoinCache);
        }

        public ClassJoinManyDefine<TEntity, TJoin> Cache(Action<ClassJoinCacheDefine<TEntity, TJoin>> cd)
        {
            OnCache();
            cd(cacheDefine);
            return this;
        }

        public ClassJoinManyDefine<TEntity, TJoin> Cascade(Action<ClassJoinManyCascadeDefine<TEntity, TJoin>> cd)
        {
            OnCascade();
            cd(cascadeDefine);
            return this;
        }
    }
}
