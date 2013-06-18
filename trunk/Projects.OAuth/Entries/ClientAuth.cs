using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 客户端的授权信息
    /// </summary>
    public class ClientAuth
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        public virtual int ClientId { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 客户端描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        public virtual string Secret { get; set; }

        /// <summary>
        /// 授权范围
        /// </summary>
        public virtual IList<string> Scopes { get; set; }

        /// <summary>
        /// 授权状态
        /// </summary>
        public virtual ClientAuthStatus Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 客户端的回调地址
        /// </summary>
        public virtual string CallbackPath { get; set; }
    }
}
