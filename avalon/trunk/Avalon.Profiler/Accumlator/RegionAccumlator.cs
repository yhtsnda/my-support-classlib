using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Avalon.Profiler
{
    /// <summary>
    /// 提供具备时间区域的计数器
    /// </summary>
    public class RegionAccumlator
    {
        Stopwatch sw;
        ConcurrentQueue<RegionItem> queue;
        object syncObj = new object();
        RegionItem currRegion = null;

        /// <summary>
        /// 构造新的实例
        /// </summary>
        /// <param name="interval">计数的时间区域，毫秒</param>
        /// <param name="capacity">存留的区间个数</param>
        public RegionAccumlator(int interval, int capacity)
        {
            this.Interval = interval;
            this.Capacity = capacity;

            queue = new ConcurrentQueue<RegionItem>();
            sw = new Stopwatch();
            sw.Start();
        }

        /// <summary>
        /// 获取计数的时间区域，毫秒
        /// </summary>
        public int Interval { get; private set; }

        /// <summary>
        /// 获取存留的区间个数
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// 获取当前的区间索引
        /// </summary>
        public int Index
        {
            get { return (int)(sw.ElapsedMilliseconds / Interval); }
        }

        /// <summary>
        /// 获取完整区间的总值
        /// </summary>
        public int FullSumValue
        {
            get
            {
                int index;
                return GetFullItems(out index).Sum(o => o.Value);
            }
        }

        /// <summary>
        /// 获取区间的总值
        /// </summary>
        public int SumValue
        {
            get
            {
                int index;
                return GetItems(out index).Sum(o => o.Value);
            }
        }

        /// <summary>
        /// 获取上一个区间的值
        /// </summary>
        public int FullValue
        {
            get
            {
                int index;
                var ri = GetFullItems(out index).FirstOrDefault(o => o.Index == index - 1);
                if (ri != null)
                    return ri.Value;
                return 0;
            }
        }

        /// <summary>
        /// 获取当前区间的值
        /// </summary>
        public int Value
        {
            get
            {
                if (currRegion == null || currRegion.Index < Index)
                    return 0;
                return currRegion.Value;
            }
        }

        /// <summary>
        /// 获取完整的区间的各个值
        /// </summary>
        public int[] FullValues
        {
            get
            {
                int index, ci = 0;
                var items = GetFullItems(out index).Reverse().ToList();
                var outputs = new List<int>();
                for (var i = index - 1; i >= Math.Max(0, index - Capacity); i--)
                {
                    if (ci < items.Count && items[ci].Index == i)
                    {
                        outputs.Add(items[ci].Value);
                        ci++;
                    }
                    else
                    {
                        outputs.Add(0);
                    }
                }
                return outputs.ToArray();
            }
        }

        /// <summary>
        /// 获取区间的各个值
        /// </summary>
        public int[] Values
        {
            get
            {
                int index, ci = 0;
                var items = GetItems(out index).Reverse().ToList();
                var outputs = new List<int>();
                for (var i = index; i >= Math.Max(0, index - Capacity + 1); i--)
                {
                    if (ci < items.Count && items[ci].Index == i)
                    {
                        outputs.Add(items[ci].Value);
                        ci++;
                    }
                    else
                    {
                        outputs.Add(0);
                    }
                }
                return outputs.ToArray();
            }
        }

        public void Increase()
        {
            var index = Index;
            if (currRegion == null || index > currRegion.Index)
            {
                lock (syncObj)
                {
                    if (currRegion == null || index > currRegion.Index)
                    {
                        var ri = new RegionItem() { Index = index, Value = 1 };
                        queue.Enqueue(ri);
                        currRegion = ri;

                        while (queue.Count > Capacity + 1)
                        {
                            RegionItem dri;
                            queue.TryDequeue(out dri);
                        }
                        return;
                    }
                }
            }
            Interlocked.Increment(ref currRegion.Value);
        }

        IEnumerable<RegionItem> GetItems(out int index)
        {
            var ci = (int)(sw.ElapsedMilliseconds / Interval);
            index = ci;
            return queue.ToArray().Where(o => o.Index > ci - Capacity && o.Index <= ci);
        }

        IEnumerable<RegionItem> GetFullItems(out int index)
        {
            var ci = (int)(sw.ElapsedMilliseconds / Interval);
            index = ci;
            return queue.ToArray().Where(o => o.Index >= ci - Capacity && o.Index < ci);
        }

        private class RegionItem
        {
            public int Index;

            public int Value;
        }
    }
}
