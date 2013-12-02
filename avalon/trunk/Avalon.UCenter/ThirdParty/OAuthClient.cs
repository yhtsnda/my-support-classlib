using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Avalon.Framework;
using System.Collections.Specialized;
using System.IO;
using Avalon.Utility;
using Avalon.Profiler;

namespace Avalon.UCenter
{
    public abstract class OAuthClient
    {
        OAuthClientSetting setting;
        public OAuthClient(OAuthClientSetting setting)
        {
            this.setting = setting;
        }

        public OAuthClientSetting Setting
        {
            get { return setting; }
        }

        protected string GetRedirectUri(MappingType source, long regPlat, string redirectUrl, string clientId)
        {
            return String.Format("{0}?source={1}&platcode={2}&client_id={4}&response_type=code&status={5}&redirect_uri={3}", Setting.CallbackUrl, (int)source, regPlat, HttpUtility.UrlEncode(redirectUrl), clientId, UserCenterConfig.ChooseVersion);
        }

        public virtual string GetAuthorizeUrl(MappingType source, long regPlat, string redirectUrl, string clientId)
        {
            var redirectUri = GetRedirectUri(source, regPlat, redirectUrl, clientId);
            return String.Format(
                "{0}?client_id={1}&response_type=code&redirect_uri={2}&scope={3}",
                Setting.AuthorizeUrl,
                Setting.ConsumerKey,
                HttpUtility.UrlEncode(redirectUri),
                Setting.Scope
                );
        }

        protected string GetTokenUri(string authorizationCode, MappingType source, long regPlat, string redirectUrl, int clientId, out NameValueCollection vc)
        {
            vc = new NameValueCollection();
            vc.Add("grant_type", "authorization_code");
            vc.Add("client_id", setting.ConsumerKey);
            vc.Add("client_secret", setting.ConsumerSecret);
            vc.Add("code", authorizationCode);
            vc.Add("redirect_uri", GetRedirectUri(source, regPlat, redirectUrl, clientId.ToString()));

            return String.Format("{0}", setting.TokenUrl);
        }

        public virtual string GetAccessToken(string authorizationCode, MappingType source, long regPlat, string redirectUrl, int clientId)
        {
            NameValueCollection vc;
            var uri = GetTokenUri(authorizationCode, source, regPlat, redirectUrl, clientId, out  vc);

            using (var client = new WebClient())
            {
                try
                {
                    // add watch
                    using (var c = ProfilerContext.Watch("get access_token from " + uri))
                    {
                        var content = client.UploadValues(uri, "POST", vc);
                        return ParseAccessToken(Encoding.UTF8.GetString(content));
                    }
                }
                catch (WebException ex)
                {
                    StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                    throw new Exception(sr.ReadToEnd() + vc["redirect_uri"]);
                }
            }
        }

        protected abstract string ParseAccessToken(string content);

        protected string GetOpenIdUri(string accessToken)
        {
            return String.Format("{0}?access_token={1}", setting.OpenIdUrl, accessToken);
        }

        protected abstract string ParseOpenId(string content);

        public virtual string GetUserKey(string accessToken)
        {
            var uri = GetOpenIdUri(accessToken);
            using (var client = new WebClient())
            {
                // add watch
                using (var c = ProfilerContext.Watch("get user_key from " + uri))
                {
                    var content = client.DownloadString(uri);
                    return ParseOpenId(content);
                }
            }
        }
    }
}
