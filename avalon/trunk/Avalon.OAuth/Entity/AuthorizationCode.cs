using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    /// <summary>
    /// 授权码
    /// </summary>
    public class AuthorizationCode
    {
        const int ExpireMinutes = 10;

        public AuthorizationCode(int appId, long userId)
            : this()
        {
            AppId = appId;
            UserId = userId;
        }

        public AuthorizationCode()
        {
            CreateTime = NetworkTime.Now;
            ExpireTime = CreateTime.AddMinutes(ExpireMinutes);
            Code = Guid.NewGuid().ToString("N");
        }

        public virtual int Id { get; set; }

        public virtual string Code { get; set; }

        public virtual int AppId { get; set; }

        public virtual long UserId { get; set; }

        public virtual DateTime CreateTime { get; set; }

        public virtual DateTime ExpireTime { get; set; }

        public virtual bool IsExpire()
        {
            return ExpireTime < NetworkTime.Now;
        }
    }
}
