using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public static class CollectionExtend
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> items)
        {
            return items == null || items.Count == 0;
        }

        public static void AddRange<T>(this IList<T> items, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                items.Add(item);
            }
        }
    }
}
