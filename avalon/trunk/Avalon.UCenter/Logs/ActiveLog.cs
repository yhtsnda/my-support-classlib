using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class ActiveLog
    {
        public ActiveLog()
        {
            ActiveDate = NetworkTime.Now;
        }

        public virtual int Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual long UserId { get; set; }


        /// <summary>
        /// 激活产品使用的终端编码（5-8）
        /// </summary>
        public virtual int TerminalCode { get; set; }

        /// <summary>
        /// 激活产品编码（1-4）
        /// </summary>	
        public virtual int ProductCode { get; set; }

        /// <summary>
        /// 激活日期
        /// </summary>
        public virtual DateTime ActiveDate { get; set; }
    }
}
