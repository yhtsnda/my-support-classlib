using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;

namespace Projects.Mvc
{
    /// <summary>
    /// 业务异常类
    /// </summary>
    public class ProjectBaseException : Exception
    {
        public ProjectBaseException() { Code = ResultCode.InternalServerError; }

        public ProjectBaseException(string message) : base(message)  { Code = ResultCode.InternalServerError; }

        public ProjectBaseException(string message, Exception innerException)
            : base(message, innerException) { Code = ResultCode.InternalServerError; }

        public ProjectBaseException(string format, object arg0)
            : base(String.Format(format, arg0)) { Code = ResultCode.InternalServerError; }

        public ProjectBaseException(string format, object arg0, object arg1)
            : base(String.Format(format, arg0, arg1)) { Code = ResultCode.InternalServerError; }

        public ProjectBaseException(string format, object arg0, object arg1, object arg2)
            : base(String.Format(format, arg0, arg1, arg2)) { Code = ResultCode.InternalServerError; }

        public ProjectBaseException(string format, params object[] args)
            : base(String.Format(format, args)) { Code = ResultCode.InternalServerError; }

        public int Code { get; set; }
    }
}
