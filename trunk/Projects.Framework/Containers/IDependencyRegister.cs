using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public interface IDependencyRegister
    {
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        void RegisterType(Type implementationType);
    }
}
