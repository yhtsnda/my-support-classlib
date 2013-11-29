using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public abstract class TokenRequestBase
    {
        internal OAuthService OAuthService { get; set; }

        public int ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        public int ClientCode { get; private set; }

        public abstract GrantType GrantType { get; }

        public virtual void Parse(HttpRequestBase request)
        {
            var authorization = request.Headers[HttpRequestHeaders.Authorization];
            if (!String.IsNullOrEmpty(authorization))
            {
                var auth = Encoding.ASCII.GetString(Convert.FromBase64String(authorization.Substring(6)));
                var vs = auth.Split(':');
                ClientId = Int32.Parse(vs[0]);
                ClientSecret = vs[1];
            }
            else
            {
                ClientId = MessageUtil.GetInt32(request, Protocal.client_id);
                ClientSecret = MessageUtil.GetString(request, Protocal.client_secret);

                var code = MessageUtil.TryGetString(request, Protocal.client_code);
                if (!string.IsNullOrEmpty(code))
                    ClientCode = MessageUtil.GetInt32(request, Protocal.client_code);
            }
        }

        public abstract AccessGrant Token();

        protected void ValidClient()
        {
            var client = OAuthService.GetClientAuthorization(ClientId);

            if (client == null)
                OAuthError(AccessTokenRequestErrorCodes.InvoidClient, "client id invalid.");

            if (client.Status != ClientAuthorizeStatus.Normal)
                OAuthError(AccessTokenRequestErrorCodes.UnauthorizedClient, "client unauthorized", 401);

            if (ClientSecret != client.Secret)
                OAuthError(AccessTokenRequestErrorCodes.InvoidClient, "client secret invalid.");
        }

        [DebuggerStepThrough]
        protected void OAuthError(string errorCode, string message, int code = 400)
        {
            throw new OAuthException(errorCode, message, code);
        }
    }

    public class TokenRequestBaseDoc
    {
        /// <summary>
        /// 应用的app_key
        /// </summary>
        public int client_id { get; set; }

        /// <summary>
        /// 应用的app_secret
        /// </summary>
        public string client_secret { get; set; }
    }
}
