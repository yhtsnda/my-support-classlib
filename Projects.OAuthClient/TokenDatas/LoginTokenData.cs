using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 登录凭据数据
    /// </summary>
    public abstract class LoginTokenData : SimpleTokenData
    {
        public LoginTokenData(int clientId, string clientSecret, long platCode,
            string browser = null, string ip = null, string extend = null)
            : base(clientId, clientSecret)
        {
            this.PlatCode = platCode;
            this.Browser = browser;
            this.IpAddress = ip;
            this.ExtendField = extend;
        }

        public long PlatCode { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public string ExtendField { get; set; }

        public override void UpdateDatas(NameValueCollection data)
        {
            base.UpdateDatas(data);

            data.Add(Protocal.PLATCODE, PlatCode.ToString());
            data.Add("browser", Browser);
            data.Add("ipaddress", IpAddress);
            data.Add("extendfield", ExtendField);
        }
    }

    /// <summary>
    /// 登录用户中心的凭据数据
    /// </summary>
    public class LoginUserCenterTokenData : LoginTokenData
    {
        public LoginUserCenterTokenData(int clientId, string clientSecret, long platCode,
            string userName, string password, string browser = null,
            string ip = null, string extend = null)
            : base(clientId, clientSecret, platCode, browser, ip, extend)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add("username", UserName);
            data.Add("password", Password);
            data.Add(Protocal.GRANT_TYPE, Protocal.PASSWORD);
            data.Add(Protocal.ACCOUNT_TYPE, "user_center");

            base.UpdateDatas(data);
        }
    }

    /// <summary>
    /// 使用第三方账户登录的凭据数据
    /// </summary>
    public class LoginThridTokenData : LoginTokenData
    {
        public LoginThridTokenData(int clientId, string clientSecret, long platCode,
            string accessToken, int mappingType, string browser = null,
            string ip = null, string extend = null)
            : base(clientId, clientSecret, platCode, browser, ip, extend)
        {
            this.AccessToken = accessToken;
            this.MappingType = mappingType;
        }

        public string AccessToken { get; set; }
        public int MappingType { get; set; }

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add("accesstoken", AccessToken);
            data.Add("mappingtype", MappingType.ToString());
            data.Add(Protocal.GRANT_TYPE, Protocal.THIRD_TOKEN);
            data.Add(Protocal.ACCOUNT_TYPE, "third_token");

            base.UpdateDatas(data);
        }
    }
}
