using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 面向个体用户的授权码
    /// </summary>
    public class AuthorizationCode
    {
        public AuthorizationCode(int appid, int userId)
            : this()
        {
            this.AppId = appid;
            this.UserId = userId;
        }

        public AuthorizationCode()
        {
            CreateTime = DateTime.Now;
            Code = Guid.NewGuid().ToString("N");
        }

        public string Code { get; set; }

        public int AppId { get; set; }

        public int UserId { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual DateTime ExpireTime { get; set; }

        public virtual bool IsEffect()
        {
            return ExpireTime >= DateTime.Now;
        }
    }
}
