using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.RecordProcessor
{
    internal class DefaultProcessor : BaseProcessor
    {
        public DefaultProcessor(string processName)
            : base(processName)
        {
        }

        public override void ProcessRequest(RecrodData data)
        {
            //TODO:特征值的判断
            //if 非特征值
            base.ProcessRequest(data);
            //else 实际处理代码
            Console.WriteLine("This output from Login Processor, Do Not Process");
            
        }
    }
}
