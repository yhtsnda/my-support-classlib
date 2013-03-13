using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Framework.OAuth2
{
    public class ApiException : Exception
    {
        public ResultCode ResultCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ApiException()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ApiException(string message, ResultCode code = ResultCode.ServerError)
            : base(message)
        {
            this.ResultCode = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ApiException(string message, Exception innerException, ResultCode code = ResultCode.ServerError)
            : base(message, innerException)
        {
            this.ResultCode = code;
        }
    }
}
