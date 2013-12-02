using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class LoginLog
    {
        public LoginLog()
        {
            LoginTime = NetworkTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 产品代码（1-4）
        /// </summary>
        public virtual int ProductCode { get; set; }

        /// <summary>
        /// 终端编号（5-8）
        /// </summary>
        public virtual int TerminalCode { get; set; }

        /// <summary>
        /// 操作IP
        /// </summary>
        public virtual string LoginIp { get; set; }

        /// <summary>
        /// 操作IP的Int类型
        /// </summary>
        public virtual long LoginIPInt { get; set; }

        /// <summary>
        /// 浏览器代码
        /// </summary>
        public virtual string BrowserCode { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTime LoginTime { get; set; }

        /// <summary>
        /// 扩展
        /// </summary>
        public virtual string ExtendField { get; set; }
    }
}
