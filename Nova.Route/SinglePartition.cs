using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Route
{
    internal class SinglePartition<TKey>: AbstractPartition
    {
        private Func<TKey, int> partitionFun;

        public SinglePartition(Func<TKey, int> partitionFun, 
            int[] partitionCount, int[] partitionLength)
        {
            if (partitionFun == null)
                throw new ArgumentNullException("partitionFun");

            this.partitionFun = partitionFun;
        }

        protected override int Calculate()
        {
            throw new NotImplementedException();
        }
    }
}
