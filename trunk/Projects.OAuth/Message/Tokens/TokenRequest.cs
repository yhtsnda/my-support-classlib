using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

using Projects.Tool.Http;

namespace Projects.OAuth
{
    /// <summary>
    /// 请求凭证的抽象基类
    /// </summary>
    public abstract class TokenRequestBase
    {
        internal OAuthService OAuthService { get; set; }

        public int ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        public abstract GrantType GrantType { get; }

        public virtual void Parse(HttpRequestBase request)
        {
            ClientId = MessageUtility.GetInt32(request, Protocal.CLIENT_ID);
            ClientSecret = MessageUtility.GetString(request, Protocal.CLIENT_SECRET);
        }

        public abstract ServerAccessGrant Token();

        protected void ValidClient()
        {
            var client = OAuthService.GetClientAuth(ClientId);

            if(client == null)
                OAuthError(AccessTokenRequestErrorCode.InvoidClient, "client id invalid.");
            if(client.Status != ClientAuthStatus.Enabled)
                OAuthError(AccessTokenRequestErrorCode.UnauthorizedClient, "client unauthorized", 401);
            if (ClientSecret != client.Secret)
                OAuthError(AccessTokenRequestErrorCode.InvoidClient, "client secret invalid.");
        }

        [DebuggerStepThrough]
        protected void OAuthError(string errorCode, string message, int code = 400)
        {
            throw new OAuthException(errorCode, message, code);
        }
    }

    /// <summary>
    /// 带刷新凭据的请求
    /// </summary>
    public class TokenRefreshRequest : TokenRequestBase
    {
        public string RefreshToken { get; protected set; }

        public override GrantType GrantType
        {
            get { return GrantType.RefreshToken; }
        }

        public override ServerAccessGrant Token()
        {
            base.ValidClient();
            ServerAccessGrant accessGrant = OAuthService.GetServerAccessGrantByRefreshToken(RefreshToken);

            if(accessGrant == null)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "refresh token invalid", 400);
            if(ClientId != accessGrant.ClientId)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "client id is not match.", 400);

            //如果授权刷新凭证不在有效
            if (!accessGrant.IsRefreshEffective())
            {
                OAuthService.DeleteServerAccessGrant(accessGrant);
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "refresh token expire", 400);
            }

            var refreshedToken = new ServerAccessGrant(accessGrant.ClientId, accessGrant.UserId)
            {
                Scope = accessGrant.Scope,
                GrantType = accessGrant.GrantType
            };

            OAuthService.CreateServerAccessGrant(refreshedToken);
            OAuthService.DeleteServerAccessGrant(accessGrant);
            return refreshedToken;
        }
    }

    /// <summary>
    /// 登录请求基类
    /// </summary>
    public abstract class TokenLoginRequest : TokenRequestBase
    {
        public long PlatCode { get; private set; }

        public string Browser { get; private set; }

        public string IpAddress { get; private set; }

        public string ExtendField { get; private set; }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            PlatCode = MessageUtility.GetInt64(request, "platcode");
            Browser = MessageUtility.TryGetString(request, "browser");
            IpAddress = MessageUtility.TryGetString(request, "ipaddress");
            ExtendField = MessageUtility.TryGetString(request, "extendfield");

            if (String.IsNullOrEmpty(Browser))
                Browser = GetBrowser(request);
            if (String.IsNullOrEmpty(IpAddress))
                IpAddress = Projects.Tool.Http.IpAddress.GetIP();
        }

        string GetBrowser(HttpRequestBase request)
        {
            var browserCode = "";
            if (request.Browser != null)
            {
                browserCode = request.Browser.Browser;
            }
            return browserCode;
        }

        public override GrantType GrantType
        {
            get { throw new NotImplementedException(); }
        }
    }

    /// <summary>
    /// 用户中心登录请求
    /// </summary>
    public class TokenPasswordUserCenterRequest : TokenLoginRequest
    {
        public string UserName { get; protected set; }

        public string Password { get; protected set; }

        public override GrantType GrantType
        {
            get { return GrantType.Password; }
        }

        public AccountType AccountType
        {
            get { return AccountType.UserCenter; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            UserName = MessageUtility.GetString(request, Protocal.USER_NAME);
            Password = MessageUtility.GetString(request, Protocal.PASSWORD);
        }

        public override ServerAccessGrant Token()
        {
            ValidClient();

            var result = OAuthService.ValidatePassword(UserName, Password, PlatCode, Browser, IpAddress, ExtendField);
            if (result.Code != 0)
                OAuthError(result.Code.ToString(), result.Message, result.Code);
            return OAuthService.CreateServerAccessGrant(ClientId, result.UserId);
        }
    }

    /// <summary>
    /// 第三方接入登录请求
    /// </summary>
    public class TokenPasswordThirdTokenRequest : TokenLoginRequest
    {
        public string AccessToken { get; set; }

        public int MappingType { get; set; }

        public override GrantType GrantType
        {
            get { return GrantType.Password; }
        }

        public AccountType AccountType
        {
            get { return AccountType.ThirdToken; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            AccessToken = MessageUtility.GetAccessToken(request);
            MappingType = MessageUtility.GetInt32(request, "mappingtype");
        }

        public override ServerAccessGrant Token()
        {
            ValidClient();

            var result = OAuthService.ValidateThirdToken(AccessToken, MappingType, PlatCode, Browser, IpAddress, ExtendField);
            if (result.Code != 0)
                OAuthError(result.Code.ToString(), result.Message, result.Code);

            return OAuthService.CreateServerAccessGrant(ClientId, result.UserId);
        }
    }

    /// <summary>
    /// 客户端保有证书的请求
    /// </summary>
    public class TokenClientCredentialsRequest : TokenRequestBase
    {
        public override GrantType GrantType
        {
            get { return GrantType.ClientCredentials; }
        }

        public override ServerAccessGrant Token()
        {
            ValidClient();
            return OAuthService.CreateServerAccessGrant(ClientId);
        }
    }

    /// <summary>
    /// 使用授权码的请求
    /// </summary>
    public class TokenAuthrizationCodeRequest : TokenRequestBase
    {
        public Uri RedirectUri { get; protected set; }

        public string Code { get; protected set; }

        public override GrantType GrantType
        {
            get { return GrantType.AuthorizationCode; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            Code = MessageUtility.GetString(request, Protocal.CODE);
            RedirectUri = new Uri(MessageUtility.GetString(request, Protocal.REDIRECT_URI));
        }

        public override ServerAccessGrant Token()
        {
            ValidClient();

            var code = OAuthService.GetAuthorizationCode(Code);
            if (code == null)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "code invalid");

            if (!code.IsEffect())
            {
                OAuthService.DeleteAuthorizationCode(code);
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "code expire");
            }

            if (code.AppId != ClientId)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "client id is not match.", 400);

            OAuthService.DeleteAuthorizationCode(code);

            return OAuthService.CreateServerAccessGrant(ClientId, code.UserId);
        }
    }

    /// <summary>
    /// 持有信任用户凭据的请求
    /// </summary>
    public class TokenUserRequest : TokenRequestBase
    {
        public int UserId { get; set; }

        public string AccessToken { get; set; }

        public override GrantType GrantType
        {
            get { return GrantType.UserToken; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            UserId = MessageUtility.GetInt32(request, "userid");
            AccessToken = MessageUtility.GetAccessToken(request);
        }

        public override ServerAccessGrant Token()
        {
            ValidClient();

            var accessGrant = OAuthService.GetServerAccessGrant(AccessToken);
            if (accessGrant == null)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "invalid access token.");
            if (!accessGrant.IsEffective())
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "access token expired.");

            if (UserId <= 0)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "invalid userid");

            return OAuthService.CreateServerAccessGrant(accessGrant.ClientId, UserId);
        }
    }
}
