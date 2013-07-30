using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Reflection;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 提供基于线程的工作台，用于先存储线程关联的数据。
    /// </summary>
    public class Workbench : IDisposable
    {
        const string WorkbenchKey = "Projects.Tool.Workbench";

        Hashtable items = new Hashtable();
        Dictionary<MethodInfo, Action<Workbench>> disposeHandlers = new Dictionary<MethodInfo, Action<Workbench>>();
        Dictionary<MethodInfo, Action<Workbench>> flushHandlers = new Dictionary<MethodInfo, Action<Workbench>>();
        bool disposed = false;


        /// <summary>
        /// 获取当前的工作台对象。
        /// </summary>
        public static Workbench Current
        {
            get
            {
                Workbench current = (Workbench)CallContext.GetData(WorkbenchKey);
                if (current == null)
                {
                    current = new Workbench();
                    CallContext.SetData(WorkbenchKey, current);
                }
                current.CheckDisposed();
                return current;
            }
        }

        private Workbench()
        {
        }

        public static void Reset()
        {
            Workbench current = (Workbench)CallContext.GetData(WorkbenchKey);
            if (current != null)
                CallContext.SetData(WorkbenchKey, null);
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public static void Dispose()
        {
            Workbench current = (Workbench)CallContext.GetData(WorkbenchKey);
            if (current != null)
                ((IDisposable)current).Dispose();

            //current = new Workbench();
            //CallContext.SetData(WorkbenchKey, current);
        }

        public static void Flush()
        {
            Workbench current = (Workbench)CallContext.GetData(WorkbenchKey);
            if (current != null)
            {
                current.InnerFlush();
            }
        }

        /// <summary>
        /// 用于存储工作台的共享数据
        /// </summary>
        public IDictionary Items
        {
            get
            {
                CheckDisposed();
                return items;
            }
        }

        /// <summary>
        /// 附加一个销毁工作台前的回调委托，同一个方法仅回调一次。
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void AttachDisposeHandler(Action<Workbench> handler)
        {
            CheckDisposed();

            disposeHandlers[handler.Method] = handler;
        }

        /// <summary>
        /// 附加一个刷出作台前的回调委托，同一个方法仅回调一次。
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void AttachFlushHandler(Action<Workbench> handler)
        {
            CheckDisposed();
            flushHandlers[handler.Method] = handler;
        }

        /// <summary>
        /// 移除一个销毁工作台前的回调委托
        /// </summary>
        /// <param name="handler"></param>
        public void DetachDisposeHandler(Action<Workbench> handler)
        {
            disposeHandlers.Remove(handler.Method);
        }

        /// <summary>
        /// 移除一个刷出工作台前的回调委托
        /// </summary>
        /// <param name="handler"></param>
        public void DetachFlusheHandler(Action<Workbench> handler)
        {
            flushHandlers.Remove(handler.Method);
        }

        void InnerFlush()
        {
            var items = flushHandlers.Values.ToList();
            foreach (Action<Workbench> handler in items)
                handler(this);
        }

        void CheckDisposed()
        {
            if (disposed)
                throw new InvalidOperationException("当前的工作台已经被销毁。");
        }

        void IDisposable.Dispose()
        {
            while (disposeHandlers.Count > 0)
            {
                var items = disposeHandlers.Values.ToList();
                foreach (Action<Workbench> handler in items)
                {
                    handler(this);
                    disposeHandlers.Remove(handler.Method);
                }
            }

            disposed = true;
        }
    }
}
