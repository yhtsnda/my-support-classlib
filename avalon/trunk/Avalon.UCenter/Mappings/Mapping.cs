using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class Mapping
    {
        public Mapping()
        {
            CreateTime = NetworkTime.Now;
        }


        public virtual int Id { get; set; }

        /// <summary>
        /// 非91UP用户与91UP映射的用户键
        /// </summary>
        public virtual string UserKey { get; set; }

        /// <summary>
        /// 用户的来源
        /// </summary>
        public virtual MappingType MappingType { get; set; }

        /// <summary>
        /// 映射的91UP用户的ID
        /// </summary>
        public virtual long LocalUserId { get; set; }

        /// <summary>
        /// 映射的91U用户名
        /// </summary>
        public virtual string LocalUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 用户来源地的标识
        /// </summary>
        public virtual long PassportId { get; set; }
    }
}
