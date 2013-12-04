using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class UserRegister
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码（明文）
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string FromUrl { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string ExtendFiled { get; set; }
        /// <summary>
        /// 平台代码
        /// </summary>
        public long PlatCode { get; set; }
        /// <summary>
        /// 注册源
        /// </summary>
        public MappingType Source { get; set; }
    }
}
