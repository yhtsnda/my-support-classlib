
using Avalon.HttpClient;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuthClient
{
    public class Passport91Helper
    {
        static ApiHttpClient client = new ApiHttpClient();

        public static VerifyCode GetVerifyCode(string basePath)
        {
            var url = UriPath.Combine(basePath, "AjaxAction/AC_verifycode.ashx?nduseraction=getverifycodestate&verifycodetype=UserLogin");
            client.Encoding = Encoding.UTF8;
            return client.HttpGet<VerifyCode>(url);
        }

        public static LoginResult Login(string basePath, string siteFlag, string userName, string password)
        {
            var url = UriPath.Combine(basePath, "AjaxAction/AC_userlogin.ashx?siteflag={0}&nduseraction=login&txtUserName={1}&txtPassword={2}&checkcode={3}");
            url = String.Format(url, siteFlag, userName, password, "");
            client.Encoding = Encoding.GetEncoding("gb2312");
            return client.HttpGet<LoginResult>(url);
        }

        public static Passport91LoginResult GetLoginResult(string basePath, string siteFlag, string userName, string password)
        {
            var login = Login(basePath, siteFlag, userName, password);
            if (login.OpCode == 4)
            {
                return Passport91LoginResult.Parse(login.Url);
            }
            throw new Exception(login.OpCode + ":" + login.Description);
        }

        public class Passport91LoginResult
        {
            public long Passport91Id;

            public string UserName;

            public string Password;

            public string CookieOrdernumberMaster;

            public string CookieOrdernumberSlave;

            public string CookieCheckCode;

            public string CookieSiteFlag;

            public static Passport91LoginResult Parse(string url)
            {
                var uri = new Uri(url);
                var data = HttpUtility.ParseQueryString(uri.Query);
                return new Passport91LoginResult()
                {
                    Passport91Id = Int64.Parse(data["c_userid"]),
                    UserName = data["txtUserName"],
                    Password = data["txtPassword"],
                    CookieOrdernumberMaster = data["c_ordernumber_master"],
                    CookieOrdernumberSlave = data["c_ordernumber_slave"],
                    CookieCheckCode = data["c_checkcode"],
                    CookieSiteFlag = data["c_siteflag"]
                };
            }

            public Passport91AuthorizeData ToAuthorizeData(int clientId, string redirectUri, long platCode, string state = null, string scope = null, string browser = null, string ipAddress = null, string extendField = null)
            {
                return new Passport91AuthorizeData(
                    clientId,
                    redirectUri,
                    platCode,
                    Passport91Id,
                    UserName,
                    Password,
                    CookieOrdernumberMaster,
                    CookieOrdernumberSlave,
                    CookieCheckCode,
                    CookieSiteFlag,
                    state,
                    scope,
                    browser,
                    ipAddress,
                    extendField
                    );
            }

            public PasswordPassport91TokenData ToTokenData(int clientId, string clientSecret, long platCode, string browser = null, string ipAddress = null, string extendField = null)
            {
                return new PasswordPassport91TokenData(
                    clientId,
                    clientSecret,
                    platCode,
                    Passport91Id,
                    UserName,
                    Password,
                    CookieOrdernumberMaster,
                    CookieOrdernumberSlave,
                    CookieCheckCode,
                    CookieSiteFlag,
                    browser,
                    ipAddress,
                    extendField
                    );
            }
        }

        public class VerifyCode
        {
            public int OpCode;

            public string Description;
        }

        public class LoginResult
        {
            public int OpCode;
            public string Description;
            public int SiteFlag;
            public string Url;
            public bool Isrecord;
            public string WxAccount;
        }
    }
}
