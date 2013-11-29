using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    /// <summary>
    /// 授权请求
    /// </summary>
    public abstract class AuthorizeRequestBase
    {
        internal OAuthService OAuthService { get; set; }

        public int ClientId { get; private set; }

        public Uri RedirectUri { get; private set; }

        public string State { get; private set; }

        public string Scope { get; private set; }

        public long PlatCode { get; private set; }

        public string Browser { get; set; }

        public string IpAddress { get; set; }

        public string ExtendField { get; set; }

        public abstract AccountType AccountType { get; }

        public virtual void Parse(HttpRequestBase request)
        {
            ClientId = MessageUtil.GetInt32(request, Protocal.client_id);
            RedirectUri = new Uri(MessageUtil.GetString(request, Protocal.redirect_uri));
            State = MessageUtil.TryGetString(request, Protocal.state);
            Scope = MessageUtil.TryGetString(request, Protocal.scope);
            PlatCode = MessageUtil.GetInt64(request, "platcode");
            Browser = MessageUtil.TryGetString(request, "browser");
            IpAddress = MessageUtil.TryGetString(request, "ipaddress");
            ExtendField = MessageUtil.TryGetString(request, "extendfield");
        }

        public virtual AuthorizationCode Authorize()
        {
            var client = OAuthService.GetClientAuthorization(ClientId);

            if (client == null)
                OAuthError(AccessTokenRequestErrorCodes.InvoidClient, "client id invalid.");

            if (client.Status != ClientAuthorizeStatus.Normal)
                OAuthError(AccessTokenRequestErrorCodes.UnauthorizedClient, "client unauthorized", 401);

            client.ValidRedirectUri(RedirectUri);

            return null;
        }

        [DebuggerStepThrough]
        protected void OAuthError(string errorCode, string message, int code = 400)
        {
            throw new OAuthException(errorCode, message, code);
        }
    }
}
