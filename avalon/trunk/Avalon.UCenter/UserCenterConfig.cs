using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public static class UserCenterConfig
    {
        /// <summary>
        /// 授权回调版本选择（正式机：91 测试机：91test 本地：91dev）
        /// </summary>
        public readonly static string ChooseVersion = GetAppSetting("ChooseVersion");

        /// <summary>
        /// ResetUrl
        /// </summary>
        public readonly static string Passport91ResetUrl = GetAppSetting("passport91.resetUrl");

        /// <summary>
        /// services url
        /// </summary>
        public readonly static string Passport91ServiceUrl = GetAppSetting("passport91.serviceUrl");

        /// <summary>
        /// site flag
        /// </summary>
        public readonly static string Passport91SiteFlag = GetAppSetting("passport91.siteFlag");

        /// <summary>
        /// app key
        /// </summary>
        public readonly static string Passport91AppKey = GetAppSetting("passport91.appKey");

        /// <summary>
        /// app password
        /// </summary>
        public readonly static string Passport91AppPassword = GetAppSetting("passport91.appPassword");

        /// <summary>
        /// app name
        /// </summary>
        public readonly static string Passport91AppName = GetAppSetting("passport91.appName");

        /// <summary>
        /// 用户中心地址
        /// </summary>
        public readonly static string UserSiteUrl = GetAppSetting("Url.User");

        /// <summary>
        ///注册时验证码出现的开始次数
        /// </summary>
        public readonly static int VerifyCodeAmount = GetAppSettingForInt("verifycode.amount");

        /// <summary>
        ///注册时验证码出现的规定时间
        /// </summary>
        public readonly static int VerifyCodeDuration = GetAppSettingForInt("verifycode.duration");

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name] ?? string.Empty;
        }

        private static int GetAppSettingForInt(string name)
        {
            int v;
            Int32.TryParse(ConfigurationManager.AppSettings[name], out v);
            return v;
        }

        private static bool GetAppSettingForBoolean(string name)
        {
            bool v;
            Boolean.TryParse(ConfigurationManager.AppSettings[name], out v);
            return v;
        }
    }
}
