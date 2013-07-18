using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    /// <summary>
    /// 注册基类
    /// </summary>
    public class BaseUserRegisterModel
    {
        public string Password { get; protected set; }

        public string NickName { get; protected set; }

        public string RealName { get; protected set; }

        public string Email { get; protected set; }

        public string IpAddress { get; protected set; }

        public string ExtendField { get; protected set; }

        public long PlatCode { get; protected set; }

        public BaseUserRegisterModel(string password, long platCode,
            string nickName = "", string realName = "", string email = "",
            string ipAddress = "", string browser = "", string fromUrl = "",
            string extendField = "")
        {
            this.Password = password;
            this.PlatCode = platCode;
            this.NickName = nickName;
            this.RealName = realName;
            this.Email = email;
            this.IpAddress = ipAddress;
            this.ExtendField = extendField;
        }
    }

    /// <summary>
    /// 用户中心注册类
    /// </summary>
    public class UserRegisterModel : BaseUserRegisterModel
    {
        public UserRegisterModel(string userName, string password, long platCode,
            string nickName = "", string realName = "", string email = "",
            string ipAddress = "", string browser = "", string fromUrl = "",
            string extendField = "")
            : base(password, platCode, nickName, realName, email, ipAddress, browser, fromUrl, extendField)
        {
            this.UserName = userName;
        }

        public string UserName { get; protected set; }
    }

    /// <summary>
    /// 第三方注册类
    /// </summary>
    public class MappingRegisterModel : BaseUserRegisterModel
    {
        public MappingRegisterModel(string mappingKey, string password, long platCode, MappingType type, 
            string nickName = "", string realName = "", string email = "",
            string ipAddress = "", string browser = "", string fromUrl = "",
            string extendField = "")
            : base(password, platCode, nickName, realName, email, ipAddress, browser, fromUrl, extendField)
        {
            this.MappingKey = mappingKey;
            this.MappingType = type;
        }

        public string MappingKey { get; protected set; }

        public MappingType MappingType { get; protected set; }
    }
}
