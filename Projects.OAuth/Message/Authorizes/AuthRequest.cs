using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.OAuth
{
    /// <summary>
    /// 授权请求基类
    /// </summary>
    public abstract class AuthRequest
    {
        internal OAuthService OAuthService { get; set; }

        /// <summary>
        /// 请求的客户端ID
        /// </summary>
        public int ClientId { get; private set; }

        /// <summary>
        /// 重定向的URI
        /// </summary>
        public Uri RedirectUri { get; private set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public string State { get; private set; }

        /// <summary>
        /// 授权范围
        /// </summary>
        public string Scope { get; private set; }

        /// <summary>
        /// 平台编码
        /// </summary>
        public long PlatCode { get; private set; }

        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public string ExtendField { get; set; }

        /// <summary>
        /// 账号类型 
        /// </summary>
        public abstract AccountType AccountType { get; }

        public virtual void Parse(HttpRequestBase request)
        {
            ClientId = MessageUtility.GetInt32(request, Protocal.CLIENT_ID); ;
            RedirectUri = new Uri(MessageUtility.GetString(request, Protocal.REDIRECT_URI));
            State = MessageUtility.TryGetString(request, Protocal.STATE);
            Scope = MessageUtility.TryGetString(request, Protocal.SCOPE);
            PlatCode = MessageUtility.GetInt64(request, "platcode");
            Browser = MessageUtility.TryGetString(request, "browser");
            IpAddress = MessageUtility.TryGetString(request, "ipaddress");
            ExtendField = MessageUtility.TryGetString(request, "extendfield");
        }

        /// <summary>
        /// 授权验证
        /// </summary>
        /// <returns>授权码</returns>
        public virtual AuthorizationCode Authorize()
        {
            var client = OAuthService.GetClientAuth(ClientId);

            if(client == null)
                OAuthError(AccessTokenRequestErrorCode.InvoidClient, "client id invalid.");
            if(client.Status == ClientAuthStatus.Disabled)
                OAuthError(AccessTokenRequestErrorCode.UnauthorizedClient, "client unauthorized", 401);

            var redirectUri = new Uri(client.CallbackPath);
            if (!String.Equals(RedirectUri.AbsolutePath, redirectUri.AbsolutePath, StringComparison.InvariantCulture))
                OAuthError(AccessTokenRequestErrorCode.RedirectUriMismatch, "redirect uri mismatch.");
            return null;
        }

        [DebuggerStepThrough]
        protected void OAuthError(string errorCode, string message, int code = 400)
        {
            throw new OAuthException(errorCode, message, code);
        }
    }

    /// <summary>
    /// 用户中心授权请求
    /// </summary>
    public class AuthUserCenterRequest : AuthRequest
    {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public override AccountType AccountType
        {
            get { return AccountType.UserCenter; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            UserName = MessageUtility.GetString(request, Protocal.USER_NAME);
            Password = MessageUtility.GetString(request, Protocal.PASSWORD);
        }

        public override AuthorizationCode Authorize()
        {
            base.Authorize();

            var result = OAuthService.ValidatePassword(UserName, Password, PlatCode, Browser, IpAddress, ExtendField);
            if (result.Code != 0)
                OAuthError(result.Code.ToString(), result.Message, 400);

            return OAuthService.CreateAuthorizationCode(ClientId, result.UserId);
        }
    }

    /// <summary>
    /// 第三方授权请求
    /// </summary>
    public class AuthThirdRequest : AuthRequest
    {
        public string AccessToken { get; set; }

        public int MappingType { get; set; }

        public override AccountType AccountType
        {
            get { return AccountType.ThirdToken; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            AccessToken = MessageUtility.ParseAccessToken(request);
            MappingType = MessageUtility.GetInt32(request, "mappingtype");
        }

        public override AuthorizationCode Authorize()
        {
            base.Authorize();

            var result = OAuthService.ValidateThirdToken(AccessToken, MappingType, PlatCode, Browser, IpAddress, ExtendField);
            if (result.Code != 0)
                OAuthError(result.Code.ToString(), result.Message, 400);

            return OAuthService.CreateAuthorizationCode(ClientId, result.UserId);
        }
    }

    public class AuthCodeRequest
    {
        public int ClientId { get; set; }

        public Uri RedirectUri { get; set; }

        public string State { get; set; }

        public string Scope { get; set; }

        public long PlatCode { get; set; }

        public string ExtendField { get; set; }

        public void Parse(HttpRequestBase request)
        {
            ClientId = MessageUtility.GetInt32(request, Protocal.CLIENT_ID);
            AuthResponseType responseType;
            if (!AuthResponseTypeExtend.TryParse(MessageUtility.GetString(request, Protocal.RESPONSE_TYPE), out responseType))
                throw new OAuthException(AuthRequestErrorCode.UnsupportedResponseType, "invalid response type", 400);

            RedirectUri = new Uri(MessageUtility.GetString(request, Protocal.REDIRECT_URI));
            State = MessageUtility.TryGetString(request, Protocal.STATE);
            Scope = MessageUtility.TryGetString(request, Protocal.SCOPE);
            PlatCode = MessageUtility.GetInt64(request, "platcode");
            ExtendField = MessageUtility.TryGetString(request, "extendfield");
        }
    }
}
