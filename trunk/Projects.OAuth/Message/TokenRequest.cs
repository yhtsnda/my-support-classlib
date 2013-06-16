using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 请求凭证的抽象基类
    /// </summary>
    public abstract class TokenRequestBase
    {
        //请求的头信息
        private readonly NameValueCollection header = new NameValueCollection();

        /// <summary>
        /// 请求凭证的抽象基类
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="clientSecret">客户端秘钥</param>
        public TokenRequestBase(int clientId, string clientSecret)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }

        public int ClientId { get; protected set; }

        public string ClientSecret { get; protected set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public abstract GrantType GrantType { get; }

        /// <summary>
        /// 请求的Header信息
        /// </summary>
        public NameValueCollection Haders { get { return header; } }
    }

    public class TokenRefreshRequest : TokenRequestBase
    {
        public TokenRefreshRequest(int clientId, string clientSecret, string refreshToken)
            : base(clientId, clientSecret)
        {
            this.RefreshToken = refreshToken;
        }

        public string RefreshToken { get; protected set; }

        public override GrantType GrantType
        {
            get { return OAuth.GrantType.RefreshToken; }
        }
    }

    public class TokenPasswordCredentialsRequest : TokenRequestBase
    {
        public TokenPasswordCredentialsRequest(int clientId, string clientSecret, string userName, string password)
            : base(clientId, clientSecret)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public string UserName { get; protected set; }

        public string Password { get; protected set; }

        public override GrantType GrantType
        {
            get { return OAuth.GrantType.Password; }
        }
    }

    public class TokenClientCredentialsRequest : TokenRequestBase
    {
        public TokenClientCredentialsRequest(int clientId, string clientSecret)
            : base(clientId, clientSecret)
        {

        }

        public override GrantType GrantType
        {
            get { return OAuth.GrantType.ClientCredentials; }
        }
    }

    public class TokenAuthrizationCodeRequest : TokenRequestBase
    {
        public TokenAuthrizationCodeRequest(int clientId, string clientSecret, string code, Uri redirectUri)
            : base(clientId, clientSecret)
        {
            this.RedirectUri = redirectUri;
            this.Code = code;
        }

        public Uri RedirectUri { get; protected set; }

        public string Code { get; protected set; }

        public override GrantType GrantType
        {
            get { return OAuth.GrantType.AuthorizationCode; }
        }
    }
}
