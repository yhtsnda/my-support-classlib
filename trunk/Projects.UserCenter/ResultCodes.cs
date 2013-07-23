using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    public enum LoginResultCode
    {
        Success = 10000,
        Error = 10001,
        UserNotFound = 10002,
        WrongPassword = 10003,
        MappingNotFound = 10004
    }

    public enum RegisterResultCode
    {
        Success = 10000,
        Error = 10001
    }

    public enum ModifyResultCode
    {
        Success = 10000,
        Error = 10001,
        UserHasDisabled = 10002,
        PasswordNoMatch = 10003
    }

    public static class ResultCodeExtend
    {
        public static string ToMessage(this LoginResultCode result)
        {
            switch (result)
            {
                case LoginResultCode.Success:
                    return "操作成功";
                case LoginResultCode.Error:
                    return "操作失败";
                case LoginResultCode.UserNotFound:
                    return "用户信息未找到";
                case LoginResultCode.WrongPassword:
                    return "密码错误";
                case LoginResultCode.MappingNotFound:
                    return "账号映射未找到";
                default:
                    return "未知的结果";
            }
        }
    }

}
