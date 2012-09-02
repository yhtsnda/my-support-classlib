using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;

namespace BuildingSiteCheck
{
    /// <summary>
    /// 业务异常类
    /// </summary>
    public class BuildingSiteCheckException : Exception
    {
        public BuildingSiteCheckException() { Code = ResultCode.InternalServerError; }

        public BuildingSiteCheckException(string message) : base(message)  { Code = ResultCode.InternalServerError; }

        public BuildingSiteCheckException(string message, Exception innerException)
            : base(message, innerException) { Code = ResultCode.InternalServerError; }

        public BuildingSiteCheckException(string format, object arg0)
            : base(String.Format(format, arg0)) { Code = ResultCode.InternalServerError; }

        public BuildingSiteCheckException(string format, object arg0, object arg1)
            : base(String.Format(format, arg0, arg1)) { Code = ResultCode.InternalServerError; }

        public BuildingSiteCheckException(string format, object arg0, object arg1, object arg2)
            : base(String.Format(format, arg0, arg1, arg2)) { Code = ResultCode.InternalServerError; }

        public BuildingSiteCheckException(string format, params object[] args)
            : base(String.Format(format, args)) { Code = ResultCode.InternalServerError; }

        public int Code { get; set; }
    }
}
