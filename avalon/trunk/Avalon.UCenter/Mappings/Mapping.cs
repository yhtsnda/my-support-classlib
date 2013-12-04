using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class Mapping
    {
        public Mapping(int mappingId, string mappingKey, int sourceId, int localUserId)
        {
            this.MappingUserId = mappingId;
            this.MappingUserKey = mappingKey;
            this.MappingSourceId = sourceId;
            this.LocalUserId = localUserId;
            CreateTime = NetworkTime.Now;
        }

        /// <summary>
        /// 唯一键
        /// </summary>
        public virtual int Id { get; protected set; }
        /// <summary>
        /// 与用户中心映射的用户Id
        /// </summary>
        public virtual int MappingUserId { get; protected set; }
        /// <summary>
        /// 与用户中心映射的用户键
        /// </summary>
        public virtual string MappingUserKey { get; protected set; }
        /// <summary>
        /// 映射来源ID
        /// </summary>
        public virtual int MappingSourceId { get; protected set; }
        /// <summary>
        /// 映射来源类型
        /// </summary>
        internal virtual MappingApp MappingSource { get; set; }
        /// <summary>
        /// 外部用户映射到本地的用户ID
        /// </summary>
        public virtual int LocalUserId { get; protected set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; protected set; }
    }
}
