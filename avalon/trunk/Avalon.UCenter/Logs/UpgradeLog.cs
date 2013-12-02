using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class UpgradeLog
    {
        public UpgradeLog()
        {
            CreateTime = NetworkTime.Now;
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
        /// 
        /// </summary>
        public virtual string OriginalUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }
}
