using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Projects.Framework
{
    public class RepositoryMonitor
    {
        static Regex regex = new Regex(@"[\d_-]+", RegexOptions.Compiled);
        Stopwatch sw;
        int queue = 1;

        public RepositoryMonitor(MethodInfo method)
        {
            RepositoryPath = String.Format("{0}.{1}", method.ReflectedType.ToPrettyString(), method.Name);
            sw = new Stopwatch();
            sw.Start();
        }

        public int ElapsedMilliseconds
        {
            get { return (int)sw.ElapsedMilliseconds; }
        }

        public string RepositoryPath { get; set; }

        public void End()
        {
            sw.Stop();
        }

        public void Push()
        {
            queue++;
        }

        public bool Pop()
        {
            queue--;
            return queue <= 0;
        }
    }
}
