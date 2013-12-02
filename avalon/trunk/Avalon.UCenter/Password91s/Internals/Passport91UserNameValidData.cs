using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    internal class Passport91UserNameValidData : AbstractPassport91XmlData
    {
        public string UserName { get; set; }

        public ResultWrapper<Passport91UserNameValidCode> Invoke()
        {
            CheckCode = UserUtil.Md5Strings(UserName, Passport91Service.AppKey).ToLower();

            var client = new Passport91ServiceClient();

            uint userId;
            var code = (Passport91UserNameValidCode)client.RegisterUserNameCheck(
                new UserNameToken() { UserName = AppName, Password = AppPassword, CheckCode = AppCheckCode },
                UserName,
                CheckCode);

            return new ResultWrapper<Passport91UserNameValidCode>(code, code.ToMessage());
        }
    }

    public enum Passport91UserNameValidCode
    {
        ServerException = -9,
        InvalidParameter = -3,
        InvalidCheckCode = 15,
        IpForrbidden = 16,

        InvalidUserNameLength = 50000,
        InvalidUserName = 50001,
        RepeatedUserName = 50002,
        Success = 50003,

        ApiException = 99999
    }

    public static class Passport91UserNameValidCodeExtend
    {
        public static string ToMessage(this Passport91UserNameValidCode code)
        {
            switch (code)
            {
                case Passport91UserNameValidCode.ServerException:
                    return "系统异常请联系管理员";
                case Passport91UserNameValidCode.InvalidParameter:
                    return "传入参数不完整";
                case Passport91UserNameValidCode.InvalidCheckCode:
                    return "校验码错误";
                case Passport91UserNameValidCode.IpForrbidden:
                    return "非法访问IP";

                case Passport91UserNameValidCode.InvalidUserNameLength:
                    return "帐号长度不合法";
                case Passport91UserNameValidCode.InvalidUserName:
                    return "帐号格式不合法";
                case Passport91UserNameValidCode.RepeatedUserName:
                    return "昵称已经被使用";
                case Passport91UserNameValidCode.Success:
                    return "验证成功";

                default:
                    return "系统接口异常";
            }
        }
        public static RegisterResultCode ToRegisterResultCode(this Passport91UserNameValidCode code)
        {
            switch (code)
            {
                case Passport91UserNameValidCode.InvalidUserNameLength:
                    return RegisterResultCode.InvalidUserNameLength;
                case Passport91UserNameValidCode.InvalidUserName:
                    return RegisterResultCode.InvalidUserName;
                case Passport91UserNameValidCode.RepeatedUserName:
                    return RegisterResultCode.RepeatedUserName;
                case Passport91UserNameValidCode.Success:
                    return RegisterResultCode.Success;
            }
            return RegisterResultCode.ApiException;
        }
    }
}
