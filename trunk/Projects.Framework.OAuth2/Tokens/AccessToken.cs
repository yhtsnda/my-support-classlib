using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Framework.OAuth2
{
    public class AccessToken
    {
        /// <summary>
        /// 标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// ClientId
        /// </summary>
        public virtual int ClientId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// 发行时间
        /// </summary>
        public virtual DateTime IssueTime { get; set; }

        /// <summary>
        /// 过期时间（单位秒）
        /// </summary>
        public virtual int Expire { get; set; }
    }
}
