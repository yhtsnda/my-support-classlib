using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 当T为值类型的时候，如果没有设置特殊的错误值，并正常返回的话，不要设置message属性，一旦设置就表示“出错了”
    /// 取消Success属性原因是，Result属性是否为null等已经可以判断了 2011-03-30
    /// 重新加入Success属性 2011-04-22，原因是可能产生为null的返回结果
    /// </summary>
    /// <typeparam name="T">这个值是一定会有的</typeparam>
    /// <typeparam name="TExtra">这个值不一定会有，比如当T是成功状态时才有这个值</typeparam>
    public class ActionResult<T, TExtra>
    {
        private Exception _exception;

        /// <summary>
        /// 
        /// </summary>
        public bool OK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///
        /// </summary>
        public TExtra ExtraData { get; set; }

        /// <summary>
        /// 设置了异常
        /// </summary>
        public Exception Exception
        {
            get { return _exception; }
            set
            {
                _exception = value;
                OK = false;//2011-04-22

                if (_exception != null)
                {
                    if (string.IsNullOrEmpty(Message))//允许自定义消息
                        Message = value.Message;
                }
            }
        }
    }
}
