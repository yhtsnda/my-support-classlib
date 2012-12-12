using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.RecordProcessor
{
    public static class ProcessorFacde
    {
        static ProcessorFacde()
        {
            RecorderFactory.Instance.Init();
        }

        public static void Process(RecrodData data)
        {
            if (String.IsNullOrEmpty(data.RecorderFlag))
                throw new ArgumentException("需要处理数据的Flag属性为空");
            var processors = RecorderFactory.Instance.GetProcessors(data.RecorderFlag);
            if(processors == null)
                throw new NullReferenceException("记录器配置中不包含" + data.RecorderFlag + "的配置"); 
            //进入处理
            processors[0].ProcessRequest(data);
        }

        /// <summary>
        /// 传入序列化后的RecordData
        /// </summary>
        /// <param name="data"></param>
        public static void Process(string data)
        {
            RecrodData recordData;
            try
            {
                recordData = new RecrodData(data);
            }
            catch
            {
                throw new ArgumentException("无法将传入的字符串反序列化为RecordData类型");
            }
            Process(recordData);
        }
    }
}
