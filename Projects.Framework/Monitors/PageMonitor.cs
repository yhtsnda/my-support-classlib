using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace Projects.Framework
{
    public class PageMonitor
    {
        static Regex regex = new Regex(@"[\d]{2,}", RegexOptions.Compiled);
        Stopwatch sw;

        public PageMonitor(Uri url)
        {
            PagePath = GetPagePath(url);
            sw = new Stopwatch();
            sw.Start();
            Repositories = new List<RepositoryMonitor>();
        }

        public long ElapsedMilliseconds
        {
            get { return sw.ElapsedMilliseconds; }
        }

        public string PagePath { get; set; }

        public List<RepositoryMonitor> Repositories
        {
            get;
            private set;
        }

        public void End()
        {
            sw.Stop();
        }

        public static string GetPagePath(Uri uri)
        {
            var path = uri.AbsolutePath.ToLower();
            path = regex.Replace(path, "*");
            return path;
        }

    }
}
