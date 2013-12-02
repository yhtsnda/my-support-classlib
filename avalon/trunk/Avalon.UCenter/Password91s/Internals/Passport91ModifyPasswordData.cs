using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    internal class Passport91ModifyPasswordData : AbstractPassport91RestData
    {
        public string AccountName { get; set; }

        public string PasswordOld { get; set; }

        public string PasswordNew { get; set; }

        public ResultWrapper<Passport91ModifyPasswordCode> Invoke()
        {
            CheckCode = UserUtil.Md5Strings(
                AccountName,
                PasswordOld,
                PasswordNew,
                TimeStamp,
                Passport91Service.AppPassword,
                Passport91Service.AppKey
                );

            try
            {
                var url = UriPath.Combine(Passport91Service.RestUrl, "Customize/Default.ashx");
                var result = InvokeApi<Passport91ModifyPasswordCode>(url);
                return new ResultWrapper<Passport91ModifyPasswordCode>(result.Code, result.Code.ToMessage());
            }
            catch (Exception ex)
            {
                return new ResultWrapper<Passport91ModifyPasswordCode>(Passport91ModifyPasswordCode.ApiException, ex.Message);
            }
        }
    }

    public enum Passport91ModifyPasswordCode
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

        PassportAccountNotExists = 20001,
        PassportLoadAccountInfoError = 20009,
        PassportAccountIsVIP = 20021,
        PassportLoadShopInfoError = 20029,
        PassportChangePasswordSuccess = 20040,
        PassportChangePasswordDeny = 20041,
        PassportOldPasswordError = 20042,
        PassportChangePasswordError = 20045,
        PassportPasswordDecodeError = 20046,
        PassportNewPwdDecodeError = 20047,
        PassportSecureCheckError = 20048,
        PassportPasswordLengthError = 20051,
        PassportPasswordFormatError = 20053,
        PassportUserHasSecureInfo = 20061,
        PassportUserBindSecureInfo = 20062,
        PassportUserBindSecureMobile = 20063,
        PassportLoadUserSecureMobileError = 20068,
        PassportLoadUserSecureInfoError = 20069,

        ApiException = 99999

    }

    public static class Passport91ModifyPasswordCodeExtend
    {
        public static ModifyPasswordResultCode ToModifyPasswordResultCode(this Passport91ModifyPasswordCode code)
        {
            switch (code)
            {
                case Passport91ModifyPasswordCode.PassportChangePasswordSuccess:
                    return ModifyPasswordResultCode.Success;
                case Passport91ModifyPasswordCode.PassportPasswordLengthError:
                    return ModifyPasswordResultCode.InvalidPasswordLength;
                case Passport91ModifyPasswordCode.PassportPasswordFormatError:
                    return ModifyPasswordResultCode.InvalidPassword;
                case Passport91ModifyPasswordCode.PassportOldPasswordError:
                    return ModifyPasswordResultCode.WrongPassword;
            }
            return ModifyPasswordResultCode.ApiException;
        }

        public static string ToMessage(this Passport91ModifyPasswordCode code)
        {
            switch (code)
            {
                case Passport91ModifyPasswordCode.UnknownMethod:
                    return "方法未知";
                case Passport91ModifyPasswordCode.InvalidArgument:
                    return "参数不完整";
                case Passport91ModifyPasswordCode.ServerException:
                    return "系统内部异常";
                case Passport91ModifyPasswordCode.UnknownUser:
                    return "未知用户：帐号不存在或帐号没有任何权限";
                case Passport91ModifyPasswordCode.IpForbidden:
                    return "IP 不在限制范围";
                case Passport91ModifyPasswordCode.UserForbidden:
                    return "用户被禁用";
                case Passport91ModifyPasswordCode.Unauthorized:
                    return "未知方法，或帐号没有此方法的权限";
                case Passport91ModifyPasswordCode.LicenseExpired:
                    return "方法使用期限已过";
                case Passport91ModifyPasswordCode.InvalidValidCode:
                    return "校验码不正确";
                case Passport91ModifyPasswordCode.MethodForbidden:
                    return "方法被禁用";
                case Passport91ModifyPasswordCode.InvalidTimestamp:
                    return "时间戳不在允许范围";
                case Passport91ModifyPasswordCode.UnknownUser2:
                    return "未知用户：帐号不存在或帐号没有任何权限";
                case Passport91ModifyPasswordCode.UnauthorizedUser:
                    return "指定游戏帐号的注册平台不在授权范围";

                case Passport91ModifyPasswordCode.PassportAccountNotExists:
                    return "用户不存在";
                case Passport91ModifyPasswordCode.PassportLoadAccountInfoError:
                    return "加载用户信息失败";
                case Passport91ModifyPasswordCode.PassportAccountIsVIP:
                    return "当前用户是商城  VIP 用户";
                case Passport91ModifyPasswordCode.PassportLoadShopInfoError:
                    return "加载商城信息失败";
                case Passport91ModifyPasswordCode.PassportChangePasswordSuccess:
                    return "修改密码成功";
                case Passport91ModifyPasswordCode.PassportChangePasswordDeny:
                    return "不允许修改当前用户密码";
                case Passport91ModifyPasswordCode.PassportOldPasswordError:
                    return "原密码不正确";
                case Passport91ModifyPasswordCode.PassportChangePasswordError:
                    return "修改密码失败";
                case Passport91ModifyPasswordCode.PassportPasswordDecodeError:
                    return "密码解密失败";
                case Passport91ModifyPasswordCode.PassportNewPwdDecodeError:
                    return "新密码解密失败";
                case Passport91ModifyPasswordCode.PassportSecureCheckError:
                    return "密保有效性检查失败";
                case Passport91ModifyPasswordCode.PassportPasswordLengthError:
                    return "密码长度不合法";
                case Passport91ModifyPasswordCode.PassportPasswordFormatError:
                    return "密码只能为数字或字母";
                case Passport91ModifyPasswordCode.PassportUserHasSecureInfo:
                    return "用户填写了密保信息";
                case Passport91ModifyPasswordCode.PassportUserBindSecureInfo:
                    return "用户绑定了密保邮箱";
                case Passport91ModifyPasswordCode.PassportUserBindSecureMobile:
                    return "用户绑定了密宝手机";
                case Passport91ModifyPasswordCode.PassportLoadUserSecureMobileError:
                    return "加载用户密宝手机信息失败";
                case Passport91ModifyPasswordCode.PassportLoadUserSecureInfoError:
                    return "加载用户密保信息失败";
                default:
                    return "系统接口异常";
            }
        }
    }
}
