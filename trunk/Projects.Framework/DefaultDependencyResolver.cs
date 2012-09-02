using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Reflection;

namespace Projects.Framework
{
    public class DefaultDependencyResolver : IDependencyRegister, IDependencyResolver
    {
        ConcurrentDictionary<Type, List<object>> mInstances = new ConcurrentDictionary<Type, List<object>>();

        public  DefaultDependencyResolver()
        {
            InitResolver();
        }

        /// <summary>
        /// 注册接口类型及相应的实现类型
        /// </summary>
        /// <typeparam name="I">接口类型</typeparam>
        /// <typeparam name="T">实现该接口的类型</typeparam>
        public void Register<I, T>() where T : I, new()
        {
            Register<I>(new T());
        }

        /// <summary>
        /// 注册接口类型及相应的实现实例
        /// </summary>
        /// <typeparam name="I">接口类型</typeparam>
        /// <param name="instance">实现该接口的实例</param>
        public void Register<I>(I instance)
        {
            Register(typeof(I), instance);
            
        }

        /// <summary>
        ///  注册接口类型及相应的实现类型
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instanceType">实现该接口的类型</param>
        public void Register(Type interfaceType, Type instanceType)
        {
            Register(interfaceType, FastActivator.Create(instanceType));
        }

        /// <summary>
        /// 注册接口类型及相应的实现实例
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instance">实现该接口的实例</param>
        public void Register(Type interfaceType, object instance)
        {
            List<object> items = mInstances.GetOrAdd(interfaceType, type => new List<object>());
            items.Add(instance);
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type interfaceType)
        {
            var items = mInstances.GetOrAdd(interfaceType, type => new List<object>());
            return items.LastOrDefault();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return ResolveAll(typeof(T)).Cast<T>();
        }

        public IEnumerable ResolveAll(Type interfaceType)
        {
            return mInstances.GetOrAdd(interfaceType, type => new List<object>());
        }

        public void Release(object instance)
        {
            throw new NotImplementedException();
        }

        protected virtual void InitResolver()
        {

        }
    }
}
