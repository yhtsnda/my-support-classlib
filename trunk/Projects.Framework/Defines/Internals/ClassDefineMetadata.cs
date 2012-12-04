using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Projects.Framework
{
    /// <summary>
    /// 类型的元数据定义
    /// </summary>
    public class ClassDefineMetadata
    {
        #region 私有字段
        private Type mEntityType;
        private Dictionary<MethodInfo, ClassJoinDefineMetadata> mClassJoinDefines;
        private Dictionary<string, ClassCacheRegionDefineMetadata> mCacheRegionDefines;
        #endregion

        #region 公共属性
       
        public Type EntityType
        {
            get { return mEntityType; }
        }

        public string Table { get; set; }

        public MemberInfo IdMember { get; set; }

        /// <summary>
        /// 当前对象是否可以缓存
        /// </summary>
        public bool IsCacheable { get; set; }

        /// <summary>
        /// 是否禁用通用缓存
        /// </summary>
        public bool DisableCommonCache { get; set; }

        /// <summary>
        /// 是否对象实现了 ILifecycle 接口
        /// </summary>
        public bool IsLifecycleImplementor { get; set; }

        /// <summary>
        /// 是否对象实现了 IValidatable 接口
        /// </summary>
        public bool IsValidatableImplementor { get; set; }

        /// <summary>
        /// 区域缓存集合
        /// </summary>
        public Dictionary<string, ClassCacheRegionDefineMetadata> CacheRegionDefines
        {
            get { return mCacheRegionDefines; }
        }

        /// <summary>
        /// 对象关联定义的元数据
        /// </summary>
        public IDictionary<MethodInfo, ClassJoinDefineMetadata> ClassJoinDefines
        {
            get { return mClassJoinDefines; }
        }
        #endregion

        #region 构造函数
        public ClassDefineMetadata(Type entityType)
        {
            this.mEntityType = entityType;
            mClassJoinDefines = new Dictionary<MethodInfo, ClassJoinDefineMetadata>();
            mCacheRegionDefines = new Dictionary<string, ClassCacheRegionDefineMetadata>
            (StringComparer.CurrentCultureIgnoreCase);

            IsValidatableImplementor = mEntityType.GetInterface(typeof(IValidatable).FullName) != null;
            IsLifecycleImplementor = mEntityType.GetInterface(typeof(ILifecycle).FullName) != null;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取给定对象的相关缓存依赖的键值
        /// </summary>
        public List<string> GetCacheRegionKeys(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (!mEntityType.IsAssignableFrom(entity.GetType()))
                throw new ArgumentException("entity");

            return CacheRegionDefines.Values.Select(item=>item.GetRegionCacheKey(entity)).ToList();
        }

        public string GetCacheKey(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            var pa = PropertyAccessorFactory.GetPropertyAccess(EntityType);
            return GetCacheKeyById(pa.GetGetter(IdMember.Name).Get(entity));
        }

        /// <summary>
        /// 通过标识获取对象缓存键
        /// </summary>
        public string GetCacheKeyById(object id)
        {
            return "#" + mEntityType.FullName + ":" + id.ToString();
        }

        public ClassJoinDefineMetadata GetClassJoinDefineMetadata(MethodInfo method)
        {
            return mClassJoinDefines.TryGetValue(method);
        }

        public void CheckCacheRegions(IEnumerable<CacheRegion> regions)
        {
            regions.ForEach(o =>
            {
                if (!CacheRegionDefines.ContainsKey(o.RegionName))
                    throw new Exception(String.Format("类型 {1} 缓存区域 {0} 尚未进行定义",
                        o.RegionName, EntityType.FullName));
            });
        }
        #endregion
    }
}
