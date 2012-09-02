using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static class ArrayExtend
    {
        /// <summary>
        /// 判断当期数组是否为 null 或长度为0
        /// </summary>
        public static bool IsNullOrEmpty(this Array source)
        {
            return source != null ? source.Length <= 0 : false;
        }

        /// <summary>
        /// 判断给定的索引是否在数组有效的范围内
        /// </summary>
        public static bool WithinIndex(this Array source, int index)
        {
            return source != null && index >= 0 && index < source.Length;
        }
    }
}
