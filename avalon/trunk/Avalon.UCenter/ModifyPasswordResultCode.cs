using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public enum ModifyPasswordResultCode
    {
        Success = 1,

        InvalidPassword = 400015,
        InvalidPasswordLength = 400016,

        WrongPassword = 400034,

        ApiException = 500500
    }

    public static class ModifyPasswordResultCodeExtend
    {
        public static string ToMessage(this ModifyPasswordResultCode code)
        {
            switch (code)
            {
                case ModifyPasswordResultCode.Success:
                    return "成功";
                case ModifyPasswordResultCode.InvalidPassword:
                    return "密码为数字和字母构成，不可包含符号";
                case ModifyPasswordResultCode.InvalidPasswordLength:
                    return "密码应为7~12位半角字符，可含字母、数字,区分大小写";
                case ModifyPasswordResultCode.WrongPassword:
                    return "密码错误";
                default:
                    return "系统接口异常";
            }
        }
    }
}
