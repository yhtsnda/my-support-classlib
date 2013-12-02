using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Avalon.UCenter
{
    internal class Passport91RegisterData : AbstractPassport91XmlData
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string NickName { get; set; }

        public string Mobile { get; set; }

        public string IpAddress { get; set; }

        public int RegType { get; set; }

        public ResultWrapper<Passport91RegisterCode, long> Invoke()
        {
            CheckCode = UserUtil.Md5Strings(
              UserName,
              Password,
              NickName,
              Mobile,
              IpAddress,
              RegType.ToString(),
              Passport91Service.AppKey
              ).ToLower();

            var client = new Passport91ServiceClient();

            uint userId;
            var code = (Passport91RegisterCode)client.RegisterUserInfo_Cop_591UP_WithRegType(
                new UserNameToken() { UserName = AppName, Password = AppPassword, CheckCode = AppCheckCode },
                UserName,
                Password,
                NickName,
                Mobile,
                IpAddress,
                RegType,
                CheckCode,
                out userId);

            return new ResultWrapper<Passport91RegisterCode, long>(code, userId, code.ToMessage());
        }
    }


    public enum Passport91RegisterCode
    {
        ServerException = -9,
        InvalidParameter = -3,
        InvalidCheckCode = 15,
        IpForrbidden = 16,

        RegisterFailure = 10000,
        RegisterFailure2 = 10001,
        Success = 10002,

        InvalidUserNameLength = 50000,
        InvalidUserName = 50001,
        ForbidUserName = 50002,
        InvalidNickNameLength = 50004,
        RepeatedNickName = 50005,
        InvalidPasswordLength = 50011,
        InvalidPassword = 50013,
        InvalidMobileNumber = 50017,
        InvalidMobileNumber2 = 50018,

        ApiException = 99999
    }

    public static class Passport91RegisterCodeExtend
    {
        public static RegisterResultCode ToRegisterResultCode(this Passport91RegisterCode code)
        {
            switch (code)
            {
                case Passport91RegisterCode.Success:
                    return RegisterResultCode.Success;
                case Passport91RegisterCode.InvalidUserNameLength:
                    return RegisterResultCode.InvalidUserNameLength;
                case Passport91RegisterCode.InvalidUserName:
                    return RegisterResultCode.InvalidUserName;
                case Passport91RegisterCode.InvalidNickNameLength:
                    return RegisterResultCode.InvalidNickNameLength;
                case Passport91RegisterCode.RepeatedNickName:
                    return RegisterResultCode.RepeatedNickName;
                case Passport91RegisterCode.InvalidPasswordLength:
                    return RegisterResultCode.InvalidPasswordLength;
                case Passport91RegisterCode.InvalidPassword:
                    return RegisterResultCode.InvalidPassword;
            }
            return RegisterResultCode.ApiException;
        }

        public static string ToMessage(this Passport91RegisterCode code)
        {
            switch (code)
            {
                case Passport91RegisterCode.ServerException:
                    return "系统异常请联系管理员";
                case Passport91RegisterCode.InvalidParameter:
                    return "传入参数不完整";
                case Passport91RegisterCode.InvalidCheckCode:
                    return "校验码错误";
                case Passport91RegisterCode.IpForrbidden:
                    return "非法访问IP";
                case Passport91RegisterCode.RegisterFailure:
                    return "注册失败";
                case Passport91RegisterCode.RegisterFailure2:
                    return "注册失败2";
                case Passport91RegisterCode.Success:
                    return "注册成功";
                case Passport91RegisterCode.InvalidUserNameLength:
                    return "帐号长度不合法";
                case Passport91RegisterCode.InvalidUserName:
                    return "帐号格式不合法";
                case Passport91RegisterCode.ForbidUserName:
                    return "帐号已经被使用";
                case Passport91RegisterCode.InvalidNickNameLength:
                    return "昵称长度不合法";
                case Passport91RegisterCode.RepeatedNickName:
                    return "昵称已经被使用";
                case Passport91RegisterCode.InvalidPasswordLength:
                    return "密码长度不合法";
                case Passport91RegisterCode.InvalidPassword:
                    return "密码格式不合法";
                case Passport91RegisterCode.InvalidMobileNumber:
                    return "手机号码不合法";
                case Passport91RegisterCode.InvalidMobileNumber2:
                    return "手机号码不合法2";
                default:
                    return "系统接口异常";
            }
        }
    }
}
