using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Tool.Util
{
    public class WorkbenchModule : IHttpModule
    {
        static List<IWorkbenchModule> modules = new List<IWorkbenchModule>();
        static bool inited = false;

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += OnEndRequest;

            if (!inited)
            {
                foreach (var module in modules)
                    module.Init();
            }
            inited = true;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            for (var i = 0; i < modules.Count; i++)
            {
                try
                {
                    modules[i].BeginRequest((HttpApplication)sender);
                }
                catch { }
            }
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            for (var i = modules.Count - 1; i >= 0; i--)
            {
                try
                {
                    modules[i].EndRequest((HttpApplication)sender);
                }
                catch { }
            }
            Workbench.Dispose();
        }

        public static void Register(Type moduleType)
        {
            var module = (IWorkbenchModule)Projects.Tool.Reflection.FastActivator.Create(moduleType);
            modules.Add(module);
            if (inited)
                module.Init();
        }
    }
}
