using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 定义对象关联的信息
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TJoin"></typeparam>
    public abstract class ClassJoinDefine<TEntity, TJoin> where TJoin : class
    {
        ClassDefineMetadata metadata;
        MemberInfo member;
        ClassJoinType joinType;
        ClassJoinDefineMetadata joinMetadata;

        private ClassJoinDefine(ClassDefineMetadata metadata, ClassJoinType joinType, MemberInfo member)
        {
            this.metadata = metadata;
            this.joinType = joinType;
            this.member = member;

            joinMetadata = new ClassJoinDefineMetadata();
            joinMetadata.JoinName = member.Name;

            var property = member as PropertyInfo;
            if (property != null)
            {
                var getMethod = property.GetGetMethod(true);
                if (getMethod == null)
                    throw new AvalonException("类型 {0} 的级联属性 {1} 必须可读。", member.DeclaringType, member.Name);

                joinMetadata.Method = getMethod;
                joinMetadata.JoinType = MethodJoinType.PropertyGet;

                // set
                var setMethod = property.GetSetMethod(true);
                if (setMethod == null)
                    throw new AvalonException("类型 {0} 的级联属性 {1} 必须可写。", member.DeclaringType, member.Name);

                var setDefine = new ClassJoinDefineMetadata();
                setDefine.Method = setMethod;
                setDefine.JoinType = MethodJoinType.PropertySet;
                setDefine.JoinName = member.Name;

                metadata.ClassJoinDefines.Add(setDefine.Method, setDefine);
            }
            else
            {
                joinMetadata.JoinType = MethodJoinType.Method;
                joinMetadata.Method = (MethodInfo)member;
            }
            metadata.ClassJoinDefines.Add(joinMetadata.Method, joinMetadata);
        }

        internal ClassJoinDefine(ClassDefineMetadata metadata, ClassJoinType joinType, MemberInfo member, Action<TEntity, HasOneByForeignKeyDefine> foreignKeyDefine)
            : this(metadata, joinType, member)
        {
            switch (joinType)
            {
                case ClassJoinType.HasOneByForeignKey:
                    joinMetadata.DataProcesser = new HasOneByForeignKeyClassJoinProcessor<TEntity, TJoin>(foreignKeyDefine, joinMetadata);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        internal ClassJoinDefine(ClassDefineMetadata metadata, ClassJoinType joinType, MemberInfo member, Action<TEntity, HasManyByForeignKeyDefine> foreignKeyDefine)
            : this(metadata, joinType, member)
        {
            switch (joinType)
            {
                case ClassJoinType.HasManyByForeignKey:
                    joinMetadata.DataProcesser = new HasManyByForeignKeyClassJoinProcessor<TEntity, TJoin>(foreignKeyDefine, joinMetadata);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        internal ClassJoinDefine(ClassDefineMetadata metadata, ClassJoinType joinType, MemberInfo member, Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> joinExpr)
            : this(metadata, joinType, member)
        {
            switch (joinType)
            {
                case ClassJoinType.HasOne:
                    joinMetadata.DataProcesser = new HasOneClassJoinProcessor<TEntity, TJoin>(joinExpr, joinMetadata);
                    break;
                case ClassJoinType.HasMany:
                    joinMetadata.DataProcesser = new HasManyClassJoinProcessor<TEntity, TJoin>(joinExpr, joinMetadata);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        protected MemberInfo Member
        {
            get { return member; }
        }

        protected ClassJoinDefineMetadata JoinMetadata
        {
            get { return joinMetadata; }
        }

        protected ClassDefineMetadata Metadata
        {
            get { return metadata; }
        }

        protected void OnCascade()
        {
            if (member is PropertyInfo)
                metadata.CascadeProperties.Add((PropertyInfo)member);
            if (member is MethodInfo)
                metadata.CascadeMethods.Add((MethodInfo)member);

            //启动级联更新
            joinMetadata.JoinCascade.CascataType = CascadeType.All;
        }

        protected void OnCache<TEntity>(Func<TEntity, string> nameFunc)
        {
            //启用缓存
            joinMetadata.JoinCache.IsCacheable = true;
            joinMetadata.JoinCache.NameFunc = (entity) => nameFunc((TEntity)entity);
        }

    }
}
