using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public enum RegisterResultCode
    {
        Success = 0,
        EmptyUserName = 400001,
        EmptyPassword = 400002,
        EmptyNickName = 400003,
        InvalidUserName = 400011,
        InvalidUserNameLength = 400012,
        InvalidEmail = 400013,
        InvalidMobileNumber = 400014,
        InvalidPassword = 400015,
        InvalidPasswordLength = 400016,
        InvalidNickNameLength = 400017,
        RepeatedUserName = 400021,
        RepeatedNickName = 400022,

        InvalidVerifyCode = 400023,
        EmptyUserForBind = 400024,

        ApiException = 500500
    }

    public static class RegisterResultCodeExtend
    {
        public static string ToMessage(this RegisterResultCode code)
        {
            switch (code)
            {
                case RegisterResultCode.Success:
                    return "成功";
                case RegisterResultCode.EmptyUserName:
                    return "用户名不能为空";
                case RegisterResultCode.EmptyPassword:
                    return "密码不能为空";
                case RegisterResultCode.EmptyNickName:
                    return "昵称不能为空";
                case RegisterResultCode.InvalidUserName:
                    return "用户名格式错误";
                case RegisterResultCode.InvalidUserNameLength:
                    return "用户名长度错误";
                case RegisterResultCode.InvalidEmail:
                    return "邮箱输入的格式错误";
                case RegisterResultCode.InvalidMobileNumber:
                    return "手机号码的格式错误";
                case RegisterResultCode.InvalidPassword:
                    return "密码为数字和字母构成，不可包含符号";
                case RegisterResultCode.InvalidPasswordLength:
                    return "密码应为7~12位半角字符，可含字母、数字,区分大小写";
                case RegisterResultCode.InvalidNickNameLength:
                    return "昵称应为3到20个字符";
                case RegisterResultCode.RepeatedUserName:
                    return "帐号已经被使用";
                case RegisterResultCode.RepeatedNickName:
                    return "重复的昵称";
                case RegisterResultCode.InvalidVerifyCode:
                    return "验证码错误";
                case RegisterResultCode.EmptyUserForBind:
                    return "用户未绑定";
                default:
                    return "系统接口异常";
            }
        }
    }
}
