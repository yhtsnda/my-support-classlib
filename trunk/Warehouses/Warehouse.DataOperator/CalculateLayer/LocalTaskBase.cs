using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Warehouse.DataOperator
{
    public abstract class LocalTaskBase : ITask
    {
        public event EventHandler OnException;

        public event EventHandler OnProcess;

        public event EventHandler OnProcessOver;

        public abstract void Process();

        public abstract void Stop();

        public abstract void Pause();

        /// <summary>
        /// 计算器的实例
        /// </summary>
        protected object mCalculator = null;

        /// <summary>
        /// 设置执行Map操作的类
        /// </summary>
        /// <param name="processer">完成Map和Reduce操作的类的类型</param>
        public void SetProcesser(Type processer)
        {
            try
            {
                mCalculator = LocalTaskManager.CheckAndCreate(processer);
            }
            catch (ArgumentException)
            {
                //TODO:这里需要终止操作,并记录日志
            }
        }

        public void SetKeyType()
        {
            throw new NotImplementedException();
        }

        public void SetValueType()
        {
            throw new NotImplementedException();
        }
    }
}
