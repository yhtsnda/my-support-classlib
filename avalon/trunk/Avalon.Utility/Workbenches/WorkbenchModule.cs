using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.Utility
{
    public class WorkbenchModule : IHttpModule
    {
        static List<IWorkbenchModule> modules = new List<IWorkbenchModule>();
        static bool inited = false;
        static object syncRoot = new object();
        static ILog log = LogManager.GetLogger("workbench");

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += OnEndRequest;

            if (inited)
                return;

            lock (syncRoot)
            {
                try
                {
                    if (!inited)
                    {
                        foreach (var module in modules)
                            module.Init();
                    }
                }
                finally
                {
                    inited = true;
                }
            }
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            for (var i = 0; i < modules.Count; i++)
            {
                modules[i].BeginRequest((HttpApplication)sender);
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
                catch (Exception ex)
                {
                    log.Error("endrequest error", ex);
                }
            }
            Workbench.Dispose();
        }

        public static void Register(Type moduleType)
        {
            lock (syncRoot)
            {
                var module = (IWorkbenchModule)FastActivator.Create(moduleType);
                modules.Add(module);
                if (inited)
                    module.Init();
            }
        }
    }
}
