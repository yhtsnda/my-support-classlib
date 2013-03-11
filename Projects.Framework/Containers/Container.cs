using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public static class Container
    {
        public static IDependencyContainer DependencyContainer
        {
            get;
            set;
        }

        /// <summary>
        ///解释指定接口类型并获取实现实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return DependencyContainer.Resolve<T>();
        }

        /// <summary>
        /// 解释指定接口类型并获取实现实例
        /// </summary>
        /// <param name="Type">接口类型.</param>
        /// <returns></returns>
        public static object Resolve(Type type)
        {
            return DependencyContainer.Resolve(type);
        }

        /// <summary>
        ///  注册接口类型及相应的实现类型
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instanceType">实现该接口的类型</param>
        public static void Register(Type interfaceType, Type instanceType)
        {
            DependencyContainer.Register(interfaceType, instanceType);
        }

        /// <summary>
        /// 注册接口类型及相应的实现实例
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instance">实现该接口的实例</param>
        public static void Register(Type interfaceType, object instance)
        {
            DependencyContainer.Register(interfaceType, instance);
        }
    }

    
}
