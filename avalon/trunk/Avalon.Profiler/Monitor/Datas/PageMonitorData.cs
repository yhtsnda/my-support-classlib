using Avalon.Utility;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Avalon.Profiler
{
    public class PageMonitorData
    {
        ConcurrentDictionary<string, RepositoryMonitorData> repositories = new ConcurrentDictionary<string, RepositoryMonitorData>();

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
                return repositories.Values.ToList();
            }
        }

        public void Append(MonitorContext context)
        {
            Interlocked.Increment(ref Count);
            Interlocked.Add(ref Spans, context.Elapsed);

            foreach (var rep in context.Repositories)
            {
                repositories.AddOrUpdate(rep.RepositoryPath, (key) =>
                {
                    return new RepositoryMonitorData()
                    {
                        Name = rep.RepositoryPath,
                        Count = 1,
                        CacheHits = rep.CacheHit ? 1 : 0,
                        Spans = rep.Elapsed
                    };
                }, (key, repData) =>
                {
                    Interlocked.Increment(ref repData.Count);
                    Interlocked.Add(ref repData.Spans, rep.Elapsed);
                    Interlocked.Add(ref repData.CacheHits, rep.CacheHit ? 1 : 0);
                    return repData;
                });
            }
        }
    }
}
