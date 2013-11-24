using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// 解析指定接口类型并获取实现实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// 解析指定接口类型并获取实现实例
        /// </summary>
        /// <param name="type">接口类型</param>
        /// <returns></returns>
        object Resolve(Type interfaceType);

        /// <summary>
        /// 解析指定接口类型并获取实现实例，返回值可能为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ResolveOptional<T>();

        /// <summary>
        /// 解析指定接口类型并获取实现实例，返回值可能为空
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        object ResolveOptional(Type interfaceType);
    }
}
