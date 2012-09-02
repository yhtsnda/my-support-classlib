using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public interface IDependencyRegister
    {
        /// <summary>
        /// 注册接口类型及相应的实现类型
        /// </summary>
        /// <typeparam name="I">接口类型</typeparam>
        /// <typeparam name="T">实现该接口的类型</typeparam>
        void Register<I, T>() where T : I, new();

        /// <summary>
        /// 注册接口类型及相应的实现实例
        /// </summary>
        /// <typeparam name="I">接口类型</typeparam>
        /// <param name="instance">实现该接口的实例</param>
        void Register<I>(I instance);

        /// <summary>
        ///  注册接口类型及相应的实现类型
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instanceType">实现该接口的类型</param>
        void Register(Type interfaceType, Type instanceType);

        /// <summary>
        /// 注册接口类型及相应的实现实例
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instance">实现该接口的实例</param>
        void Register(Type interfaceType, object instance);
    }
}
