using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class RegisterLog
    {
        public RegisterLog()
        {
            RegTime = NetworkTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 产品编号（1-4）
        /// </summary>
        public virtual int ProductCode { get; set; }

        /// <summary>
        /// 终端编号（5-8）
        /// </summary>
        public virtual int TerminalCode { get; set; }

        /// <summary>
        /// 来源标号（9-12）
        /// </summary>
        public virtual int OriginCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string RegIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int RegIpInt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime RegTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string BrowserCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsAutoReg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string FromUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ExtendField { get; set; }
    }
}
