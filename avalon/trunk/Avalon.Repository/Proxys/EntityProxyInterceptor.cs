using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalon.Utility;

namespace Avalon.Framework
{
    internal class EntityProxyInterceptor : IInterceptor
    {
        static MethodInfo getSourceMethod;
        static MethodInfo resetMethod;

        object root;    //根对象
        object source;   //源对象
        object proxy;   //代理对象
        ClassJoinDefineMetadata joinMetadata;
        bool inited;

        static EntityProxyInterceptor()
        {
            getSourceMethod = typeof(IProxy).GetMethod("GetSource");
            resetMethod = typeof(IProxy).GetMethod("Reset");
        }

        public EntityProxyInterceptor(object root, ClassJoinDefineMetadata joinMetadata)
        {
            this.root = root;
            this.joinMetadata = joinMetadata;
        }

        /// <summary>
        /// 是否已经构造完毕
        /// </summary>
        public bool IsConstructed { get; set; }

        public object Source
        {
            get { return source; }
        }

        public object Proxy
        {
            get { return proxy; }
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method == resetMethod)
            {
                inited = false;
                return;
            }

            if (IsConstructed && !inited)
                InitEntity(invocation);

            if (invocation.Method == getSourceMethod)
                invocation.ReturnValue = source;
            else
                invocation.Proceed();
        }

        void InitEntity(IInvocation invocation)
        {
            inited = true;
            proxy = invocation.Proxy;
            source = joinMetadata.DataProcesser.Process(root);
            //如果为null就直接返回null，modify by skypan 2013年3月26日17:30:50
            if (source == null)
            {
                throw new ArgumentException("数据不存在。");
            }

            //get origianl object, can not use nhibernate proxy object
            var original = EntityUtil.OriginalObjectProvider.GetOriginal(source);
            RepositoryFramework.CloneEntity(original, proxy);

            ProxyProvider.ProxyJoins(proxy);
        }
    }
}