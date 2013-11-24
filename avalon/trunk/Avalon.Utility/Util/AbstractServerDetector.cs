using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Avalon.Utility
{
    public abstract class AbstractServerDetector
    {
        protected static DateTime ServerInitTime;

        static AbstractServerDetector()
        {
            ServerInitTime = DateTime.Now;
        }

        public abstract string Name { get; }

#if DEBUG
        public virtual int DueSecond { get { return 0; } }
#else
        public virtual int DueSecond { get { return 200; } }
#endif

        public string Detect()
        {
            if (DateTime.Now.AddSeconds(-DueSecond) > ServerInitTime)
                return OnDetect();
            return "ignore";
        }

        protected abstract string OnDetect();
    }

    public static class ServerDetectorImpl
    {
        static List<AbstractServerDetector> detectors = new List<AbstractServerDetector>();

        static ServerDetectorImpl()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(o => !o.IsDynamic))
            {
                var types = assembly.GetExportedTypes().Where(o => !o.IsAbstract && o.BaseType == typeof(AbstractServerDetector));
                foreach (var type in types)
                {
                    detectors.Add((AbstractServerDetector)FastActivator.Create(type));
                }
            }
        }

        public static string Detect(string[] names)
        {
            if (names.Length > 0 && names[0] == "none")
                return "";

            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<string> outputs = new List<string>();
            if (names == null || names.Length == 0)
            {
                outputs.AddRange(detectors.Select(o => String.Format("{0}\r\n{1}", o.Name, o.Detect())));
            }
            else
            {
                names.ForEach(o =>
                {
                    var detector = detectors.FirstOrDefault(d => d.Name == o);
                    if (detector != null)
                    {
                        outputs.Add(String.Format("{0}\r\n{1}", o, detector.Detect()));
                    }
                });
            }

            return "span: " + sw.ElapsedMilliseconds + "\r\n" + String.Join("\r\n", outputs);
        }
    }
}
