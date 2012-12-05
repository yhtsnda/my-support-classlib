using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    /// <summary>
    /// Map或Reduce任务接口
    /// </summary>
    public interface ITask
    {
    
        void Process();

        void Stop();

        void Pause();

        void SetProcesser(Type processer);

        void SetKeyType();

        void SetValueType();
    }
}
