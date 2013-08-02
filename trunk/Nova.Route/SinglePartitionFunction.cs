using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Route
{
    public class SinglePartitionFunction<TKey>: AbstractPartitionFunction
    {
        private Func<TKey, int> partitionFun;

        public SinglePartitionFunction(Func<TKey, int> partitionFun, 
            int[] partitionCount, int[] partitionLength)
        {
            if (partitionFun == null)
                throw new ArgumentNullException("partitionFun");

            this.partitionFun = partitionFun;
        }

        protected override int Partition()
        {
            throw new NotImplementedException();
        }
    }
}
