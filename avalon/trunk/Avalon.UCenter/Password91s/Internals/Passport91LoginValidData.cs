using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    internal class Passport91ValidLoginData : AbstractPassport91RestData
    {
        public long UserId { get; set; }

        public string SiteFlag { get; set; }

        public string IpAddress { get; set; }

        public string CookieOrdernumberMaster { get; set; }

        public string CookieOrdernumberSlave { get; set; }

        public string CookieSiteflag { get; set; }

        public string CookieCheckcode { get; set; }

        public ResultWrapper<Passport91LoginValidCode> Invoke()
        {
            CheckCode = UserUtil.Md5Strings(
                UserId.ToString(),
                SiteFlag,
                IpAddress,
                CookieOrdernumberMaster,
                CookieOrdernumberSlave,
                CookieSiteflag,
                CookieCheckcode,
                TimeStamp,
                Passport91Service.AppPassword,
                Passport91Service.AppKey
                );

            try
            {
                var url = UriPath.Combine(Passport91Service.RestUrl, "Account/Default.ashx");
                var result = InvokeApi<Passport91LoginValidCode>(url);
                return new ResultWrapper<Passport91LoginValidCode>(result.Code, result.Code.ToMessage());
            }
            catch (Exception ex)
            {
                return new ResultWrapper<Passport91LoginValidCode>(Passport91LoginValidCode.ApiException, ex.Message);
            }
        }
    }

    public enum Passport91LoginValidCode
    {
        UnknownMethod = 10001,
        InvalidArgument = 10002,
        ServerException = 10003,

        UnknownUser = 11001,
        IpForbidden = 11003,
        UserForbidden = 11004,
        Unauthorized = 11005,
        LicenseExpired = 11006,
        InvalidValidCode = 11007,
        MethodForbidden = 11008,
        InvalidTimestamp = 11009,

        UnknownUser2 = 12001,
        UnauthorizedUser = 12002,

        PassportSuccess = 20000,
        PassportSystemException = 20002,
        PassportInvalidCookie = 20005,
        PassportTimeOut = 20007,

        ApiException = 99999
    }

    public static class Passport91LoginValidCodeExtend
    {
        public static string ToMessage(this Passport91LoginValidCode code)
        {
            switch (code)
            {
                case Passport91LoginValidCode.UnknownMethod:
                    return "方法未知";
                case Passport91LoginValidCode.InvalidArgument:
                    return "参数不完整";
                case Passport91LoginValidCode.ServerException:
                    return "系统内部异常";
                case Passport91LoginValidCode.UnknownUser:
                    return "未知用户：帐号不存在或帐号没有任何权限";
                case Passport91LoginValidCode.IpForbidden:
                    return "IP 不在限制范围";
                case Passport91LoginValidCode.UserForbidden:
                    return "用户被禁用";
                case Passport91LoginValidCode.Unauthorized:
                    return "未知方法，或帐号没有此方法的权限";
                case Passport91LoginValidCode.LicenseExpired:
                    return "方法使用期限已过";
                case Passport91LoginValidCode.InvalidValidCode:
                    return "校验码不正确";
                case Passport91LoginValidCode.MethodForbidden:
                    return "方法被禁用";
                case Passport91LoginValidCode.InvalidTimestamp:
                    return "时间戳不在允许范围";
                case Passport91LoginValidCode.UnknownUser2:
                    return "未知用户：帐号不存在或帐号没有任何权限";
                case Passport91LoginValidCode.UnauthorizedUser:
                    return "指定游戏帐号的注册平台不在授权范围";

                case Passport91LoginValidCode.PassportSuccess:
                    return "校验成功";
                case Passport91LoginValidCode.PassportSystemException:
                    return "系统异常";
                case Passport91LoginValidCode.PassportInvalidCookie:
                    return "Cookie 内容非法";
                case Passport91LoginValidCode.PassportTimeOut:
                    return "登录超时";
                default:
                    return "接口调用异常";
            }
        }

        public static LoginResultCode ToLoginResultCode(this Passport91LoginValidCode code)
        {
            switch (code)
            {
                case Passport91LoginValidCode.PassportSuccess:
                    return LoginResultCode.Success;
            }
            return LoginResultCode.ApiException;
        }
    }
}
