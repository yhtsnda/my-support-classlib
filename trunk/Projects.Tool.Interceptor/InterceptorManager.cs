using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

namespace Projects.Tool.Interceptor
{
    internal class InterceptorManager
    {
        List<Type> mInterceptors;
        static object mSyncObj = new object();

        /// <summary>
        /// 获取可用的拦截器类型
        /// </summary>
        public static List<Type> Interceptors
        {
            get
            {
                if(mInterceptors == null)
                {
                    lock(mSyncObj)
                    {
                        if(mInterceptors == null)
                        {
                            mInterceptors = new List<Type>();
                            var interceptorTypes = ToolSection.Instance.TryGetTypes("interceptors/interceptor","type");
                            foreach (Type interceptor in interceptorTypes)
                            {
                                //如果拦截器有继承至IInterceptor接口
                                if (interceptor.GetInterface("IInterceptor", true) != null)
                                    mInterceptors.Add(interceptor);
                            }
                        }
                    }
                }
                return mInterceptors;
            }
        }
    }
}
