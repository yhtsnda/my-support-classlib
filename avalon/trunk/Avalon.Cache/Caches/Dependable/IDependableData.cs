using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public interface IDependableData : IQueryTimestamp
    {
        object Data { get; set; }
    }

    public class DependableData<T> : IDependableData
    {
        public DependableData()
        {
            Timestamp = NetworkTime.Now.Ticks;
        }

        public T Data { get; set; }

        public long Timestamp
        {
            get;
            private set;
        }

        object IDependableData.Data
        {
            get { return Data; }
            set { Data = (T)value; }
        }
    }
}
