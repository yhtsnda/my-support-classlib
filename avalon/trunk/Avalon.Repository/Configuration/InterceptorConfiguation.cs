using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Configurations
{
    internal class InterceptorConfiguation
    {
        ToolSection section;
        Dictionary<Type, List<IRepositoryFrameworkInterceptor>> typeInterceptors = new Dictionary<Type, List<IRepositoryFrameworkInterceptor>>();
        List<IRepositoryFrameworkInterceptor> interceptors = new List<IRepositoryFrameworkInterceptor>();

        public InterceptorConfiguation(ToolSection section)
        {
            this.section = section;
            interceptors.Add(new DefaultRepositoryFrameworkInterceptor());
        }

        public void Load()
        {
            var interceptorNodes = section.TryGetNodes("interceptors/interceptor");
            foreach (var node in interceptorNodes)
            {
                var entityAttr = node.Attributes.TryGetValue("entity");
                var typeAttr = node.Attributes.TryGetValue("type");
                try
                {

                    if (String.IsNullOrEmpty(entityAttr))
                        RegisterInterceptor((IRepositoryFrameworkInterceptor)FastActivator.Create(typeAttr));
                    else
                        RegisterInterceptor(Type.GetType(entityAttr), (IRepositoryFrameworkInterceptor)FastActivator.Create(typeAttr));
                }
                catch (Exception ex)
                {
                    throw new AvalonException("加载拦截器错误。entity:{0}, type:{1}。\r\n详细错误:\r\n{2} ", entityAttr, typeAttr, ex);
                }
            }
        }

        /// <summary>
        /// 获取指定类型的的仓储拦截器
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IEnumerable<IRepositoryFrameworkInterceptor> GetInterceptors(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            var items = typeInterceptors.TryGetValue(entityType);
            if (items == null)
                items = new List<IRepositoryFrameworkInterceptor>();

            if (items.Count > 0)
                items = items.ToList();
            items.AddRange(interceptors);

            return items;
        }

        /// <summary>
        /// 注册指定类型的仓储拦截器
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="interceptor"></param>
        public void RegisterInterceptor(Type entityType, IRepositoryFrameworkInterceptor interceptor)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");
            if (interceptor == null)
                throw new ArgumentNullException("interceptor");


            var items = typeInterceptors.TryGetValue(entityType);
            if (items == null)
            {
                items = new List<IRepositoryFrameworkInterceptor>();
                typeInterceptors.Add(entityType, items);
            }
            items.Add(interceptor);
        }

        /// <summary>
        /// 注册仓储拦截器
        /// </summary>
        /// <param name="interceptor"></param>
        public void RegisterInterceptor(IRepositoryFrameworkInterceptor interceptor)
        {
            if (interceptor == null)
                throw new ArgumentNullException("interceptor");

            interceptors.Add(interceptor);
        }

    }
}
