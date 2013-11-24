using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 规约对象的工厂
    /// </summary>
    public class SpecificationFactory
    {
        /// <summary>
        /// 创建一个规约对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ISpecification<T> Create<T>()
        {
            return RepositoryFramework.CreateSpecification<T>();
            //return DependencyResolver.Resolve<ISpecificationProvider>().CreateSpecification<T>();
        }
    }
}
