using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Warehouse.Settings
{
    public class ConfigurePool
    {
        /// <summary>
        /// 内置配置池
        /// </summary>
        static ConfigurePool()
        {
            mPolicyConfigPool = new Dictionary<string, PolicyConfigureBase>();
            mStorageConfigPool = new Dictionary<string, StorageNodeConfigure>();
        }

        /// <summary>
        /// 存储策略配置池
        /// </summary>
        internal static Dictionary<string, PolicyConfigureBase> mPolicyConfigPool;

        /// <summary>
        /// 存储节点配置池
        /// </summary>
        internal static Dictionary<string, StorageNodeConfigure> mStorageConfigPool;

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置的类型</typeparam>
        /// <param name="key">配置的键</param>
        /// <returns>配置信息</returns>
        public static T Get<T>(string key) where T : class, IConfigure
        {
            Type type = typeof(T);
            if (type.BaseType.Equals(typeof(PolicyConfigureBase)))
            {
                //如果不存在这样的配置
                if (!mPolicyConfigPool.ContainsKey(key))
                    return Create<T>(key);
                return mPolicyConfigPool[key] as T;
            }
            else if (type.BaseType.Equals(typeof(StorageNodeConfigure)))
            {
                //如果不存在这样的配置
                if (!mStorageConfigPool.ContainsKey(key))
                    return Create<T>(key);
                return mStorageConfigPool[key] as T;
            }
            return default(T);
        }

        /// <summary>
        /// 实例化并读取配置
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">配置键</param>
        /// <returns>配置实例</returns>
        protected static T Create<T>(string key) where T : class, IConfigure
        {
            Type type = typeof(T);
            //查找其中的Load方法,并调用之
            var loadMethod = type.GetMethod("Load");
            var instance = Activator.CreateInstance(type);
            loadMethod.Invoke(instance, null);

            //根据结果类型写入到各自的缓冲中
            if (type.BaseType.Equals(typeof(PolicyConfigureBase)))
                mPolicyConfigPool.Add(key, (PolicyConfigureBase)instance);
            else if (type.BaseType.Equals(typeof(StorageNodeConfigure)))
                mStorageConfigPool.Add(key, (StorageNodeConfigure)instance);

            return instance as T;
        }

        /// <summary>
        /// 刷新指定键值的配置
        /// </summary>
        /// <param name="key">配置键</param>
        protected void Refresh(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
