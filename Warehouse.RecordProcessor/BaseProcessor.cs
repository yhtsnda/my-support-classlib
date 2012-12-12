using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.RecordProcessor
{
    public class BaseProcessor : IProcessor
    {
        private string mProcessorName = String.Empty;
        protected IProcessor mProcessor;

        protected BaseProcessor()
        {
        }

        public BaseProcessor(string processName)
        {
            this.mProcessorName = processName;
        }

        public string ProcessName
        {
            get { return mProcessorName; }
        }

        internal void SetProcessor(IProcessor processor)
        {
            SetProcessor(processor);
        }

        /// <summary>
        /// 将需要处理的数据传递给下一个处理器
        /// </summary>
        /// <param name="data">处理数据</param>
        public virtual void ProcessRequest(RecrodData data)
        {
            if(this.mProcessor != null)
                this.mProcessor.ProcessRequest(data);
        }

        void IProcessor.SetProcessor(IProcessor processor)
        {
            this.mProcessor = processor;
        }
    }
}
