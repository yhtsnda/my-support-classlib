using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class LastLoginLog
    {
        public LastLoginLog()
        {
            LastLoginTime = NetworkTime.Now;
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string LastLoginIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime LastLoginTime { get; set; }
    }
}
