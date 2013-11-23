using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public static class CacheTuple
    {
        public static CacheTuple<T1> Create<T1>(T1 t1)
        {
            return new CacheTuple<T1>() { Item1 = t1 };
        }

        public static CacheTuple<T1, T2> Create<T1, T2>(T1 t1, T2 t2)
        {
            return new CacheTuple<T1, T2>() { Item1 = t1, Item2 = t2 };
        }

        public static CacheTuple<T1, T2, T3> Create<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
        {
            return new CacheTuple<T1, T2, T3>() { Item1 = t1, Item2 = t2, Item3 = t3 };
        }
    }
    public class CacheTuple<T1>
    {
        public T1 Item1 { get; set; }
    }

    public class CacheTuple<T1, T2>
    {
        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }
    }

    public class CacheTuple<T1, T2, T3>
    {
        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }

        public T3 Item3 { get; set; }
    }
}
