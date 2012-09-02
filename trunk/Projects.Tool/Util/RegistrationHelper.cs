using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Projects.Tool.Util
{
    public static class RegistrationHelper
    {
        /// <summary>
        ///  返回调用当前正在执行的方法的方法的 Assembly关于T的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesImplementing<T>()
        {
            return GetTypesImplementing<T>(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// 通过给定程序集的长格式名称加载程序集关于T的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesImplementing<T>(string assemblyName)
        {
            return GetTypesImplementing<T>(Assembly.Load(assemblyName));
        }

        /// <summary>
        /// 通过给定程序集加载程序集关于T的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesImplementing<T>(Assembly assembly)
        {
            return assembly.GetExportedTypes()
                .Where(t => t.IsPublic && !t.IsAbstract && typeof(T).IsAssignableFrom(t))
                .ToList();
        }

        /// <summary>
        /// 获取已加载到此应用程序域的执行上下文中的程序集
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();
        }

        /// <summary>
        /// 获取已加载到此应用程序域的执行上下文中的程序集中关于T的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetInstancesOfTypesImplementing<T>()
        {
            var instances = new List<T>();
            GetAssemblies()
                .ForEach(a =>
                {
                    GetTypesImplementing<T>(a).ForEach(t =>
                    {
                        instances.Add((T)Activator.CreateInstance(t));
                    });
                });
            return instances.AsReadOnly();
        }
    }
}
