using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.RecordProcessor
{
    /// <summary>
    /// 记录处理器接口
    /// </summary>
    public interface IProcessor
    {
        string ProcessName { get; }
        void SetProcessor(IProcessor processor);
        void ProcessRequest(RecrodData data);
    }
}
