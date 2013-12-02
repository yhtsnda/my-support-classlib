using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class ResultWrapper<TResultCode> where TResultCode : struct
    {
        public ResultWrapper(TResultCode code, string message = null)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public TResultCode Code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return Convert.ChangeType(Code, typeof(int)).ToString() + ":" + Message;
        }
    }

    public class ResultWrapper<TResultCode, TResultData> where TResultCode : struct
    {
        public ResultWrapper(TResultCode code, TResultData data, string message = null)
        {
            Code = code;
            Data = data;
            Message = message;
        }

        public ResultWrapper(TResultCode code, string message = null)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public TResultCode Code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 结果数据
        /// </summary>
        public TResultData Data { get; set; }

        public override string ToString()
        {
            return Convert.ChangeType(Code, typeof(int)).ToString() + ":" + Message;
        }
    }
}
