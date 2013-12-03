using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public enum LoginResultCode
    {
        Success = 0,

        EmptyUserName = 400001,
        EmptyPassword = 400002,
        EmptyAppCode = 400003,

        InvalidAppCode = 400004,
        BindingNotFound = 400031,

        //升级相关
        NeedToUpgrade = 400032,
        UserNotFound = 400033,
        WrongPassword = 400034,

        ApiException = 500500
    }

    public static class LoginResultCodeExtend
    {
        public static string ToMessage(this LoginResultCode code)
        {
            switch (code)
            {
                case LoginResultCode.Success:
                    return "成功";
                case LoginResultCode.EmptyUserName:
                    return "用户名不能为空";
                case LoginResultCode.EmptyPassword:
                    return "密码不能为空";
                case LoginResultCode.EmptyAppCode:
                    return "应用平台码为空";
                case LoginResultCode.InvalidAppCode:
                    return "无效的平台码";
                case LoginResultCode.BindingNotFound:
                    return "未进行绑定";
                case LoginResultCode.NeedToUpgrade:
                    return "用户需要进行升级";
                case LoginResultCode.UserNotFound:
                    return "用户未找到";
                case LoginResultCode.WrongPassword:
                    return "错误的密码";

                default:
                    return "系统接口异常";
            }
        }
    }
}
