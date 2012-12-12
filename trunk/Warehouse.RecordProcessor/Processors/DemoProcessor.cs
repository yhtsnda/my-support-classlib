using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.RecordProcessor
{
    internal class DemoProcessor : BaseProcessor
    {
        public DemoProcessor(string processName)
            : base(processName)
        {
        }

        public override void ProcessRequest(RecrodData data)
        {
            if (data.Header.Get("flag") == this.ProcessName)
                Console.WriteLine("This output from Login Processor, Do Process");
            else
                Console.WriteLine("This output from Login Processor, Do Not Process");

            base.ProcessRequest(data);
        }
    }
}
