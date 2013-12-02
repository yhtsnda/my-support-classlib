using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public enum Passport91LoginCode
    {
        /// <summary>
        /// 验证码错误
        /// </summary>
        InvalidCode = 1,

        /// <summary>
        /// 帐号为空
        /// </summary>
        EmptyUserName = 2,

        /// <summary>
        ///  密码为空
        /// </summary>
        EmptyPassword = 3,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 4,

        /// <summary>
        /// 成功
        /// </summary>
        Success2 = 5,

        /// <summary>
        /// 帐号不存在
        /// </summary>
        InvalidUserName = 6,

        /// <summary>
        /// 密码错误
        /// </summary>
        InvalidPassword = 7,

        /// <summary>
        /// 登录限制
        /// </summary>
        SystemLimit = 8,

        /// <summary>
        /// 系统异常
        /// </summary>
        SystemError = -9
    }

    public static class Passport91LoginCodeExtend
    {
        public static string ToName(this Passport91LoginCode code)
        {
            switch (code)
            {
                case Passport91LoginCode.InvalidCode:
                    return "验证码错误";
                case Passport91LoginCode.EmptyUserName:
                    return "帐号为空";
                case Passport91LoginCode.EmptyPassword:
                    return "密码为空";
                case Passport91LoginCode.Success:
                case Passport91LoginCode.Success2:
                    return "成功";
                case Passport91LoginCode.InvalidUserName:
                    return "帐号不存在";
                case Passport91LoginCode.InvalidPassword:
                    return "密码错误";
                case Passport91LoginCode.SystemLimit:
                    return "登录限制";
                case Passport91LoginCode.SystemError:
                    return "系统异常";
            }
            return "未知状态";
        }
    }
}
