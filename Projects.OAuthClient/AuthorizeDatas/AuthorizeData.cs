using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 进行授权传递的数据
    /// </summary>
    public abstract class AuthorizeData
    {
        public AuthorizeData(int clientId, string redirectUri, long platCode, 
            string state = null, string scope = null, string browser = null, 
            string ipAddress = null, string extendField = null)
        {
            ClientId = clientId;
            RedirectUri = redirectUri;
            PlatCode = platCode;
            State = state;
            Scope = scope;
            Browser = browser;
            IpAddress = ipAddress;
            ExtendField = extendField;
        }
        public int ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string State { get; set; }

        public string Scope { get; set; }

        public long PlatCode { get; set; }

        public string Browser { get; set; }

        public string IpAddress { get; set; }

        public string ExtendField { get; set; }

        public virtual void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.CLIENT_ID, ClientId.ToString());
            data.Add(Protocal.REDIRECT_URI, RedirectUri);
            data.Add(Protocal.STATE, State);
            data.Add(Protocal.SCOPE, Scope);
            data.Add(Protocal.RESPONSE_TYPE, Protocal.CODE);
            data.Add(Protocal.PLATCODE, PlatCode.ToString());
            data.Add("browser", Browser);
            data.Add("ipaddress", IpAddress);
            data.Add("extendfield", ExtendField);
        }
    }

    /// <summary>
    /// 通过用户中心进行授权传递的数据
    /// </summary>
    public class UserCenterAuthorizeData : AuthorizeData
    {
        public UserCenterAuthorizeData(int clientId, string redirectUri, long platCode,
            string userName, string password, string state = null, 
            string scope = null, string browser = null, string ipAddress = null, string extendField = null)
            : base(clientId, redirectUri, platCode, state, scope, browser, ipAddress, extendField)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add("username", UserName);
            data.Add("password", Password);
            data.Add(Protocal.ACCOUNT_TYPE, "user_center");

            base.UpdateDatas(data);
        }
    }

    /// <summary>
    /// 通过第三方账号进行授权传递的数据
    /// </summary>
    public class ThirdTokenAuthorizeData : AuthorizeData
    {
        public ThirdTokenAuthorizeData(int clientId, string redirectUri, long platCode,
            string accessToken, int mappingType, string state = null, 
            string scope = null, string browser = null, string ipAddress = null, string extendField = null)
            : base(clientId, redirectUri, platCode, state, scope, browser, ipAddress, extendField)
        {
            AccessToken = accessToken;
            MappingType = mappingType;
        }

        public string AccessToken { get; set; }

        public int MappingType { get; set; }

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add("accesstoken", AccessToken);
            data.Add("mappingtype", MappingType.ToString());
            data.Add(Protocal.ACCOUNT_TYPE, "third_token");

            base.UpdateDatas(data);
        }
    }

    public enum MappingType
    {
        /// <summary>
        /// 新浪微博
        /// </summary>
        SinaWeibo = 10,

        /// <summary>
        /// 腾讯微博
        /// </summary>
        Tencent = 20
    }
}
