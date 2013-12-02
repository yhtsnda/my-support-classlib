using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class UserExtend
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string RegIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsEmailChecked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string RealName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string IdCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Mobile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // public virtual Sex Sex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string QQ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string BirthDay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Blood { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Marry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string BirthProvince { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string BirthCity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ContactEmail { get; set; }

        public virtual string ImageLogo { get; set; }

        public virtual string ImageUpdateTime { get; set; }
    }
}
