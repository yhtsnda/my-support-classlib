using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Linq.Expressions;

namespace Warehouse.RecordProcessor
{
    internal class RecorderFactory
    {
        private static RecorderFactory mFactory;
        private Dictionary<string, IProcessor[]> mRecordProcessors;

        protected RecorderFactory()
        {
            mRecordProcessors = new Dictionary<string, IProcessor[]>();
        }

        public static RecorderFactory Instance
        {
            get
            {
                if (mFactory == null)
                    mFactory = new RecorderFactory();
                return mFactory;
            }
        }

        /// <summary>
        /// 获取处理器序列
        /// </summary>
        /// <param name="recorderName">记录器名称</param>
        /// <returns>处理器序列</returns>
        /// <exception cref="NullReferenceException">
        /// 当不存在请求的记录器名称时
        /// </exception>
        public IProcessor[] GetProcessors(string recorderName)
        {
            if (!mRecordProcessors.ContainsKey(recorderName))
                return null;
            return mRecordProcessors[recorderName];
        }

        /// <summary>
        /// 根据配置初始化处理器
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// 当配置文件中不存在任何'recorders/recorder'配置节点时
        /// </exception>
        /// <exception cref="ConfigurationException">
        /// 当配置节点中不能同时读取到name和type时
        /// </exception>
        internal void Init()
        {
            var nodes = RecorderSection.Instance.TryGetNodes("recorders/recorder");
            if (nodes == null && nodes.Count() == 0)
                throw new NullReferenceException("记录器配置文件中不包含任何有效的配置");

            string recorderName = String.Empty;
            
            foreach (var node in nodes)
            {
                var processors = node.TryGetNodes("processor");
                List<IProcessor> listProcessor = new List<IProcessor>();
                foreach (var item in processors)
                {
                    if (item.Attributes["name"] == null || item.Attributes["type"] == null)
                        throw new ConfigurationErrorsException("节点中有不正确的name和type配置,请修正!");

                    object processorInstance = FastActivator.Create(item.Attributes["type"],
                        Expression.Constant(item.Attributes["name"]));
                    //如果对象为继承至IRecordProcessor的
                    if (processorInstance.GetType().BaseType.GetInterfaces().Contains(typeof(IProcessor)) ||
                        processorInstance.GetType().GetInterfaces().Contains(typeof(IProcessor)))
                        listProcessor.Add((IProcessor)processorInstance);
                }
                if (listProcessor.Count != 0 && !this.mRecordProcessors.ContainsKey(node.Attributes["name"]))
                {
                    //建立一个职责链
                    if (listProcessor.Count >= 2)
                    {
                        for (int i = 0; i < listProcessor.Count - 1; i++)
                            listProcessor[i].SetProcessor(listProcessor[i + 1]);
                    }
                    this.mRecordProcessors.Add(node.Attributes["name"], listProcessor.ToArray());
                }
                    
            }
        }

        /// <summary>
        /// 重新构建处理器
        /// </summary>
        /// <param name="recorderName"></param>
        protected void ReBuild(string recorderName)
        {
            throw new NotImplementedException();
        }
    }
}
