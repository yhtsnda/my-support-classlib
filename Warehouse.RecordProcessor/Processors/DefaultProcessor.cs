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
            Console.WriteLine("This output from Login Processor, Do Not Process");
            base.ProcessRequest(data);
        }
    }
}
