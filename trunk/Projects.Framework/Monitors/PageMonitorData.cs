using Projects.Tool.Util;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Projects.Framework
{
    public class PageMonitorData
    {
        ConcurrentDictionary<string, RepositoryMonitorData> repositories = new ConcurrentDictionary<string, RepositoryMonitorData>();

        object syncRoot = new object();
        string pagePath;

        public PageMonitorData(string pagePath)
        {
            this.pagePath = pagePath;
        }
        public string Name
        {
            get { return pagePath; }
            set { pagePath = value; }
        }

        public int Count;

        public long Spans;

        [JsonProperty("Reps")]
        public ICollection Repositories
        {
            get
            {
                lock (syncRoot)
                {
                    return repositories.Values.ToList();
                }
            }
        }

        public void Append(PageMonitor page)
        {
            Interlocked.Increment(ref Count);
            Interlocked.Add(ref Spans, page.ElapsedMilliseconds);

            foreach (var rep in page.Repositories)
            {
                repositories.AddOrUpdate(rep.RepositoryPath, (key) =>
                {
                    return new RepositoryMonitorData()
                    {
                        Name = rep.RepositoryPath,
                        Count = 1,
                        Spans = rep.ElapsedMilliseconds
                    };
                }, (key, repData) =>
                {
                    repData.Count++;
                    repData.Spans += rep.ElapsedMilliseconds;
                    return repData;
                });
            }
        }
    }
}
