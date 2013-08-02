﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Route
{
    public abstract class AbstractPartitionFunction
    {
        protected abstract int Partition();

        /// <summary>
        /// 检查分区参数的正确性
        /// </summary>
        /// <param name="count">定义的分区数</param>
        /// <param name="length">对应每个分区的取值长度</param>
        /// <remarks>
        /// 注意：其中count,length两个数组的长度必须是一致的。
        /// 约束：1024 = sum((count[i]*length[i])). count和length两个向量的点积恒等于1024
        /// </remarks>
        /// <returns>正确与否</returns>
        protected virtual bool CheckPartitionArgs(int[] count, int[] length)
        {
            if (count == null || length == null || count.Length != length.Length)
                throw new ArgumentException("error,check your partitionCount & partitionLength definition.");

            int segmentLength = count.Sum();
            
        }
    }
}
