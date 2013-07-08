using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Projects.Tool.Reflection;

namespace Projects.Framework
{
    /// <summary>
    /// 类型定义的元数据定义
    /// </summary>
    public class ClassDefineMetadata
    {
        static object syncRoot = new object();

        Type entityType;
        Dictionary<MethodInfo, ClassJoinDefineMetadata> classJoinDefines;
        Dictionary<string, ClassCacheRegionDefineMetadata> cacheRegionDefines;
        List<PropertyInfo> dataProperties;
        List<PropertyInfo> cascadeProperties;
        List<MethodInfo> cascadeMethods;

        List<Action<object, object>> dataCloners;

        public ClassDefineMetadata(Type entityType)
        {
            this.entityType = entityType;
            classJoinDefines = new Dictionary<MethodInfo, ClassJoinDefineMetadata>();
            cacheRegionDefines = new Dictionary<string, ClassCacheRegionDefineMetadata>(StringComparer.CurrentCultureIgnoreCase);

            IsValidatableImplementor = entityType.GetInterface(typeof(IValidatable).FullName) != null;
            IsLifecycleImplementor = entityType.GetInterface(typeof(ILifecycle).FullName) != null;

            dataProperties = new List<PropertyInfo>();
            cascadeProperties = new List<PropertyInfo>();
            cascadeMethods = new List<MethodInfo>();

            IsContextCacheable = true;
        }

        /// <summary>
        /// 定义的对象类型
        /// </summary>
        public Type EntityType
        {
            get { return entityType; }
        }

        public string Table
        {
            get;
            set;
        }

        public IFetchable FetchableObject { get; set; }

        /// <summary>
        /// 对象的主键属性
        /// </summary>
        public PropertyInfo IdMember { get; set; }

        /// <summary>
        /// 当前对象是否可以缓存
        /// </summary>
        public bool IsCacheable { get; set; }

        /// <summary>
        /// 当前对象是否支持上下文缓存
        /// </summary>
        public bool IsContextCacheable { get; set; }

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
        /// 映射的列
        /// </summary>
        public IList<PropertyInfo> DataProperties { get { return dataProperties; } }

        /// <summary>
        /// 级联属性
        /// </summary>
        public IList<PropertyInfo> CascadeProperties { get { return cascadeProperties; } }

        /// <summary>
        /// 级联方法
        /// </summary>
        public IList<MethodInfo> CascadeMethods { get { return cascadeMethods; } }

        /// <summary>
        /// 区域缓存集合
        /// </summary>
        public Dictionary<string, ClassCacheRegionDefineMetadata> CacheRegionDefines
        {
            get { return cacheRegionDefines; }
        }

        /// <summary>
        /// 对象关联定义的元数据
        /// </summary>
        public IDictionary<MethodInfo, ClassJoinDefineMetadata> ClassJoinDefines
        {
            get { return classJoinDefines; }
        }

        /// <summary>
        /// 获取给定对象的相关缓存依赖的键值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<string> GetCacheRegionKeys(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (!EntityType.IsAssignableFrom(entity.GetType()))
                throw new ArgumentException("entity");

            return CacheRegionDefines.Values.Select(o => o.GetRegionCacheKey(entity)).ToList();
        }

        /// <summary>
        /// 获取给定对象的对象缓存键
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetCacheKey(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return GetCacheKeyById(EntityUtil.GetId(entity));
        }

        /// <summary>
        /// 通过标识获取对象缓存键
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCacheKeyById(object id)
        {
            return "#" + entityType.FullName + ":" + id.ToString();
        }

        public ClassJoinDefineMetadata GetClassJoinDefineMetadata(MethodInfo method)
        {
            return classJoinDefines.TryGetValue(method);
        }

        /// <summary>
        /// 进行缓存区域的校验
        /// </summary>
        /// <param name="regions"></param>
        public void CheckCacheRegions(IEnumerable<CacheRegion> regions)
        {
            regions.ForEach(o =>
            {
                if (!CacheRegionDefines.ContainsKey(o.RegionName))
                    throw new PlatformException("类型 {1} 缓存区域 {0} 尚未进行定义。", o.RegionName, EntityType.FullName);
            });
        }

        /// <summary>
        /// 合并数据
        /// </summary>
        internal object CloneEntity(object source)
        {
            if (source == null)
                return null;

            if (dataCloners == null || dataCloners.Count == 0)
            {
                var ta = TypeAccessor.GetAccessor(entityType);
                dataCloners = dataProperties.Select(o => ta.GetPropertyClone(o.Name)).ToList();
            }
            var target = Projects.Tool.Reflection.FastActivator.Create(entityType);
            dataCloners.ForEach(o => o(source, target));
            return target;
        }
    }
}
