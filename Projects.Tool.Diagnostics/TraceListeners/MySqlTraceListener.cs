using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Projects.Tool.Diagnostics
{
    public class MySqlTraceListener : TraceListener
    {
        const int MaxCount = 1000;
        static ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        public static ConcurrentQueue<string> Queue
        {
            get { return queue; }
        }

        public static string GetContent()
        {
            return String.Concat(queue.ToArray());
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            base.TraceEvent(eventCache, AppendTime(source), eventType, id);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            base.TraceEvent(eventCache, AppendTime(source), eventType, id, format, args);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            base.TraceEvent(eventCache, AppendTime(source), eventType, id, message);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            base.TraceData(eventCache, AppendTime(source), eventType, id, data);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            base.TraceData(eventCache, AppendTime(source), eventType, id, data);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            base.TraceTransfer(eventCache, AppendTime(source), id, message, relatedActivityId);
        }

        string AppendTime(string source)
        {
            return NetworkTime.Now.ToString("HH:mm:ss fff");
        }

        public override void Write(string message)
        {
            EnsureCount();
            queue.Enqueue(message);
        }

        public override void WriteLine(string message)
        {
            EnsureCount();
            var mi = message.IndexOf("connection string", StringComparison.CurrentCultureIgnoreCase);
            if (mi > -1)
                message = message.Substring(0, mi) + "connection string ...";
            queue.Enqueue(message + "\r\n");
        }

        void EnsureCount()
        {
            while (queue.Count > MaxCount)
            {
                string v;
                queue.TryDequeue(out v);
            }
        }
    }
}
