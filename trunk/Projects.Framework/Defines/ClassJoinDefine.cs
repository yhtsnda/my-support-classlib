using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Framework
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
                    throw new PlatformException("类型 {0} 的级联属性 {1} 必须可读。", member.DeclaringType, member.Name);

                joinMetadata.Method = getMethod;
                joinMetadata.JoinType = MethodJoinType.PropertyGet;

                // set
                var setMethod = property.GetSetMethod(true);
                if (setMethod == null)
                    throw new PlatformException("类型 {0} 的级联属性 {1} 必须可写。", member.DeclaringType, member.Name);

                var setDefine = new ClassJoinDefineMetadata();
                setDefine.Method = setMethod;
                setDefine.JoinType = MethodJoinType.PropertySet;
                setDefine.JoinName = member.Name;

                AddClassJoinDefine(setDefine.Method, setDefine);
                //metadata.ClassJoinDefines.Add(setDefine.Method, setDefine);
            }
            else
            {
                joinMetadata.JoinType = MethodJoinType.Method;
                joinMetadata.Method = (MethodInfo)member;
            }
            AddClassJoinDefine(joinMetadata.Method, joinMetadata);
            //metadata.ClassJoinDefines.Add(joinMetadata.Method, joinMetadata);
        }

        internal ClassJoinDefine(ClassDefineMetadata metadata, ClassJoinType joinType, MemberInfo member, Action<TEntity, HasOneByForeignKeyDefine> foreignKeyDefine)
            : this(metadata, joinType, member)
        {
            switch (joinType)
            {
                case ClassJoinType.HasOneByForeignKey:
                    joinMetadata.DataProcesser = new HasOneByForeignKeyClassJoinProcesser<TEntity, TJoin>(foreignKeyDefine, joinMetadata);
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
                    joinMetadata.DataProcesser = new HasOneClassJoinProcesser<TEntity, TJoin>(joinExpr, joinMetadata);
                    break;
                case ClassJoinType.HasMany:
                    joinMetadata.DataProcesser = new HasManyClassJoinProcesser<TEntity, TJoin>(joinExpr, joinMetadata);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        void AddClassJoinDefine(MethodInfo method, ClassJoinDefineMetadata joinMetadata)
        {
            InnerAddClassJoinDefine(metadata.EntityType, method, joinMetadata);
        }

        void InnerAddClassJoinDefine(Type type, MethodInfo method, ClassJoinDefineMetadata joinMetadata)
        {
            var innerMethod = type.GetMethod(method.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, method.GetParameters().Select(o => o.ParameterType).ToArray(), null);
            if (innerMethod != null)
                metadata.ClassJoinDefines[innerMethod] = joinMetadata;
            if (type.BaseType != null)
                InnerAddClassJoinDefine(type.BaseType, method, joinMetadata);

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
            joinMetadata.JoinCascade.CascataType = CascadeType.All;
            if (member is PropertyInfo)
                metadata.CascadeProperties.Add((PropertyInfo)member);
            if (member is MethodInfo)
                metadata.CascadeMethods.Add((MethodInfo)member);
        }

        protected void OnCache()
        {
            joinMetadata.JoinCache.IsCacheable = true;
        }

    }
}
