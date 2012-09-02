using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 依赖解析接口
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// 解释指定接口类型并获取实现实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// 解释指定接口类型并获取指定类型的实现实例
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <returns></returns>
        object Resolve(Type interfaceType);

        /// <summary>
        /// 解释指定接口类型并获取所有实现实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// 解释指定接口类型并获取所有实现实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        IEnumerable ResolveAll(Type interfaceType);

        /// <summary>
        /// 释放指定实例
        /// </summary>
        /// <param name="instance"></param>
        void Release(object instance);
    }
}
