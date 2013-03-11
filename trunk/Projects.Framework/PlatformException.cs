using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Util;
using Projects.Tool;

namespace Projects.Framework
{
    /// <summary>
    /// 平台业务异常类
    /// 所有业务判断的异常都使用本类
    /// </summary>
    public class PlatformException : Exception
    {
       
        /// <summary>
        /// 构造函数
        /// </summary>
        public PlatformException()
        {
            Code = ResultCode.InternalServerError;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public PlatformException(string message)
            : base(message)
        {
            Code = ResultCode.InternalServerError;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public PlatformException(string message, Exception innerException)
            : base(message, innerException)
        {
            Code = ResultCode.InternalServerError;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="format">复合格式字符串</param>
        /// <param name="arg0">要设置格式的对象</param>
        public PlatformException(string format, object arg0)
            : base(string.Format(format, arg0))
        {
            Code = ResultCode.InternalServerError;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="format">复合格式字符串</param>
        /// <param name="arg0">要设置格式的第一个对象</param>
        /// <param name="arg1">要设置格式的第二个对象</param>
        public PlatformException(string format, object arg0, object arg1)
            : base(string.Format(format, arg0, arg1))
        {
            Code = ResultCode.InternalServerError;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="format">复合格式字符串</param>
        /// <param name="arg0">要设置格式的第一个对象</param>
        /// <param name="arg1">要设置格式的第二个对象</param>
        /// <param name="arg2">要设置格式的第三个对象</param>
        public PlatformException(string format, object arg0, object arg1, object arg2)
            : base(string.Format(format, arg0, arg1, arg2))
        {
            Code = ResultCode.InternalServerError;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="format">复合格式字符串</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public PlatformException(string format, params object[] args)
            : base(string.Format(format, args))
        {
            Code = ResultCode.InternalServerError;
        }

        public int Code
        {
            get;
            set;
        }
    }
}
