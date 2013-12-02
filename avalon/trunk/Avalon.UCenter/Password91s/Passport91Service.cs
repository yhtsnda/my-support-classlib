using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using Avalon.Utility;
using Avalon.Framework;
using System.Configuration;
using Avalon.Utility;
using Avalon.HttpClient;
using Avalon.Profiler;

namespace Avalon.UCenter
{
    public class Passport91Service : IService
    {
        public static readonly string AppName;
        public static readonly string AppPassword;
        public static readonly string AppKey;
        public static readonly string SiteFlag;
        public static readonly string ServiceUrl;
        public static readonly string RestUrl;

        ApiHttpClient client = new ApiHttpClient();

        static Passport91Service()
        {
            AppName = UserCenterConfig.Passport91AppName; //ConfigurationManager.AppSettings["passport91.appName"];
            AppPassword = UserCenterConfig.Passport91AppPassword; //ConfigurationManager.AppSettings["passport91.appPassword"];
            AppKey = UserCenterConfig.Passport91AppKey; //ConfigurationManager.AppSettings["passport91.appKey"];
            SiteFlag = UserCenterConfig.Passport91SiteFlag; // ConfigurationManager.AppSettings["passport91.siteFlag"];
            ServiceUrl = UserCenterConfig.Passport91ServiceUrl; // ConfigurationManager.AppSettings["passport91.serviceUrl"];
            RestUrl = UserCenterConfig.Passport91ResetUrl; // ConfigurationManager.AppSettings["passport91.resetUrl"];

            if (String.IsNullOrEmpty(AppName))
                throw new Avalon.Utility.ConfigurationException("缺少配置 passport91.appName");
            if (String.IsNullOrEmpty(AppPassword))
                throw new Avalon.Utility.ConfigurationException("缺少配置 passport91.appPassword");
            if (String.IsNullOrEmpty(AppKey))
                throw new Avalon.Utility.ConfigurationException("缺少配置 passport91.appKey");
            if (String.IsNullOrEmpty(SiteFlag))
                throw new Avalon.Utility.ConfigurationException("缺少配置 passport91.siteFlag");
            if (String.IsNullOrEmpty(ServiceUrl))
                throw new Avalon.Utility.ConfigurationException("缺少配置 passport91.serviceUrl");
            if (String.IsNullOrEmpty(RestUrl))
                throw new Avalon.Utility.ConfigurationException("缺少配置 passport91.resetUrl");
        }

        public ResultWrapper<Passport91LoginValidCode> ValidLogin(Passport91LoginData login)
        {
            using (var profiler = ProfilerContext.Watch("passport 91 valid login"))
            {
                Passport91ValidLoginData data = new Passport91ValidLoginData()
                {
                    UserId = login.Passport91Id,
                    CookieCheckcode = login.CookieCheckcode,
                    CookieOrdernumberMaster = login.CookieOrdernumberMaster,
                    CookieOrdernumberSlave = login.CookieOrdernumberSlave,
                    CookieSiteflag = login.CookieSiteflag,
                    IpAddress = login.IpAddress,
                    Action = "CheckUserLoginByCookie",
                    TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    UserName = AppName,
                    Format = "Json",
                    SiteFlag = SiteFlag
                };

                return data.Invoke();
            }
        }

        public ResultWrapper<Passport91RegisterCode, long> Register(UserRegister register)
        {
            using (var profiler = ProfilerContext.Watch("passport 91 register"))
            {
                var guid = Guid.NewGuid().ToString();

                Passport91RegisterData data = new Passport91RegisterData()
                {
                    AppName = Passport91Service.AppName,
                    AppPassword = UserUtil.Md5Strings(Passport91Service.AppPassword, guid).ToLower(),
                    AppCheckCode = guid,
                    UserName = register.UserName,
                    Password = UserUtil.Encrypt_PHP(register.Password, "ab343ty"),
                    NickName = UserUtil.Encrypt_PHP(register.NickName, "ab343ty"),
                    Mobile = "",
                    IpAddress = register.IpAddress,
                    RegType = 1
                };

                return data.Invoke();
            }
        }

        public ResultWrapper<Passport91UserNameValidCode> ValidUserName(string userName)
        {
            using (var profiler = ProfilerContext.Watch("passport 91 valid user name"))
            {
                var guid = Guid.NewGuid().ToString();
                Passport91UserNameValidData data = new Passport91UserNameValidData()
                {
                    AppName = Passport91Service.AppName,
                    AppPassword = UserUtil.Md5Strings(Passport91Service.AppPassword, guid).ToLower(),
                    AppCheckCode = guid,
                    UserName = userName
                };
                return data.Invoke();
            }
        }

        public ResultWrapper<Passport91ModifyPasswordCode> ModifyPassword(string userName, string passwordOld, string passwordNew)
        {
            using (var profiler = ProfilerContext.Watch("passport 91 modify password"))
            {
                var guid = Guid.NewGuid().ToString();

                Passport91ModifyPasswordData data = new Passport91ModifyPasswordData()
                {
                    AccountName = userName,
                    Action = "ChangePassword",
                    Format = "Json",
                    PasswordOld = passwordOld,
                    PasswordNew = UserUtil.Encrypt_PHP(passwordNew, "ab343ty"),
                    TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    UserName = AppName
                };
                return data.Invoke();
            }
        }

    }
}

