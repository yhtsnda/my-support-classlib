using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 验证结果类
    /// </summary>
    public class ValidationResult
    {
        private IList<string> _result;

        public ValidationResult()
        {
            _result = new List<string>();
        }

        public ValidationResult(ValidationResult result)
            : this()
        {
            if (result == null)
                throw new ArgumentNullException("result");
            Merge(result);
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="message"></param>
        public void AppendFailure(string message)
        {
            _result.Add(message);
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        public void AppendFailureFormat(string format, object arg0)
        {
            _result.Add(string.Format(format, arg0));
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        public void AppendFailureFormat(string format, object arg0, object arg1)
        {
            _result.Add(string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void AppendFailureFormat(string format, object arg0, object arg1, object arg2)
        {
            _result.Add(string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void AppendFailureFormat(string format, params object[] args)
        {
            _result.Add(string.Format(format, args));
        }

        /// <summary>
        /// 合并验证对象
        /// </summary>
        /// <param name="result"></param>
        public void Merge(ValidationResult result)
        {
            if (!result.IsSuccess())
                _result.AddRange(result.GetFailureMessages());
        }

        /// <summary>
        /// 是否所有验证都已经通过
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return _result.Count == 0;
        }

        /// <summary>
        /// 获取所有的错误信息
        /// </summary>
        /// <returns></returns>
        public IList<string> GetFailureMessages()
        {
            return _result;
        }

        public override string ToString()
        {
            //return base.ToString();
            return string.Join(Environment.NewLine, _result);
        }
    }
}
