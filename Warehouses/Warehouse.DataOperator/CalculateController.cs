using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Warehouse.DataOperator
{
    /// <summary>
    /// 计算的主控程序
    /// </summary>
    public class CalculateController
    {
        /// <summary>
        /// 执行Map函数处理器的数量
        /// </summary>
        public int MapProcesserCount { get; set; }

        /// <summary>
        /// 执行Reduction函数处理器的数量
        /// </summary>
        public int ReduceProcesserCount { get; set; }

        /// <summary>
        /// 处理器类型
        /// </summary>
        public ProcesserType ProcesserType
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        

        /// <summary>
        /// 分配Map任务
        /// </summary>
        public void AllotMapTask()
        {

        }

        /// <summary>
        /// 分配Reduce任务
        /// </summary>
        public void AllotReduceTask()
        {

        }

        /// <summary>
        /// 分配本地运行的Map任务
        /// </summary>
        protected void AllotLocalMapTask()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 分配本地运行的Reduce任务
        /// </summary>
        protected void AllotLocalReduceTask()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 分配远程运行的Map任务
        /// </summary>
        protected void AllotRemoteMapTask()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 分配远程运行的Reduce任务
        /// </summary>
        protected void AllotRemoteReduceTask()
        {
            throw new System.NotImplementedException();
        }
    }
}
