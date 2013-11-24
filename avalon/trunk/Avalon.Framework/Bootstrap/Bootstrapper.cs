using System.Collections.Generic;
using System.Linq;
using System;

namespace Avalon.Framework
{
    /// <summary>
    /// 引导器
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// 执行所有引导
        /// </summary>
        public static void Run()
        {
            //DependencyResolver.ResolveAll<IBootstrapperTask>()
            //    .ForEach(t => t.Run());
        }

        /// <summary>
        /// 执行所有重置
        /// </summary>
        public static void Reset()
        {
            //DependencyResolver.ResolveAll<IBootstrapperTask>()
            //    .ForEach(t => t.Reset());
        }
    }
}
