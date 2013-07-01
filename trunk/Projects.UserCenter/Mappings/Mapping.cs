using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    /// <summary>
    /// 账号映射关系
    /// </summary>
    public class Mapping
    {
        public Mapping(string mappingKey, int localUserId, MappingType type)
        {
            this.LocalUserId = localUserId;
            this.MappingType = type;
            this.CreateTime = DateTime.Now;
            this.MappingKey = mappingKey;
        }

        /// <summary>
        /// 自增ID
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// 与第三方映射的键(需确保在第三方唯一)
        /// </summary>
        public virtual string MappingKey { get; protected set; }

        /// <summary>
        /// 与第三方账号映射的本地账号ID
        /// </summary>
        public virtual int LocalUserId { get; protected set; }

        /// <summary>
        /// 映射类型
        /// </summary>
        public virtual MappingType MappingType { get; protected set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; protected set; }
    }
}
