using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 领域对象验证接口
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// 验证对象的状态
        /// 验证不通过请抛 <see cref="ValidationException" />异常
        /// </summary>
        void Validate();
    }

    /// <summary>
    /// 验证异常对象
    /// </summary>
    [Serializable]
    public class ValidationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ValidationException()
            : base("A validation failure occurred")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerException"></param>
        public ValidationException(Exception innerException)
            : base("A validation failure occurred", innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
