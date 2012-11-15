using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Mvc
{
    /// <summary>
    /// 自定义HTTP模块的基础类
    /// </summary>
    public abstract class BaseHttpModules : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) => OnBeginRequest(new HttpContextWrapper(((HttpApplication)sender).Context));
            context.Error += (sender, e) => OnError(new HttpContextWrapper(((HttpApplication)sender).Context));
            context.EndRequest += (sender, e) => OnEndRequest(new HttpContextWrapper(((HttpApplication)sender).Context));
            OnInitialize(context);
        }

        public virtual void OnInitialize(HttpApplication context)
        {
        }

        public virtual void OnBeginRequest(HttpContextBase context)
        {
        }

        public virtual void OnError(HttpContextBase context)
        {
        }

        public virtual void OnEndRequest(HttpContextBase context)
        {
        }

        ~BaseHttpModules()
        {
            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
