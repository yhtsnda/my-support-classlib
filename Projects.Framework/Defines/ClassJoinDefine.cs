using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Projects.Framework.Specification;

namespace Projects.Framework
{
    /// <summary>
    /// 定义对象关联的信息
    /// </summary>
    public class ClassJoinDefine<TEntity, TJoin> where TJoin : class
    {
        private ClassDefineMetadata mMetadata;
        private MethodInfo mMethod;
        private ClassJoinType mJoinType;
        private ClassJoinCacheDefine<TEntity, TJoin> mCacheDefine;

        internal ClassJoinDefine(ClassJoinType joinType, MethodInfo method, 
            ClassDefineMetadata metadata)
        {
            this.mJoinType = joinType;
            this.mMetadata = metadata;
            this.mMethod = method;
        }

        /// <summary>
        /// 定义对象加载的规约表达式
        /// </summary>
        /// <param name="specAction"></param>
        public ClassJoinCacheDefine<TEntity, TJoin> On(Func<TEntity, ISpecification<TJoin>, 
            ISpecification<TJoin>> specAction)
        {
            var joinDefine = new ClassJoinDefineMetadata()
            {
                Method = mMethod
            };
            switch (mJoinType)
            {
                case ClassJoinType.HasOne:
                    joinDefine.Processer = 
                        new HasOneClassJoinProcesser<TEntity, TJoin>(specAction, joinDefine);
                    break;
                case ClassJoinType.HasMany:
                    joinDefine.Processer = 
                        new HasManyClassJoinProcesser<TEntity, TJoin>(specAction, joinDefine);
                    break;
            }

            this.mMetadata.ClassJoinDefines.Add(mMethod, joinDefine);
            mCacheDefine = new ClassJoinCacheDefine<TEntity, TJoin>(joinDefine);
            return mCacheDefine;
        }
    }
}
