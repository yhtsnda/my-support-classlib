using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Projects.Tool;
using Projects.Tool.Util;

namespace Projects.Tool.OAuth
{
    /// <summary>
    /// OAuth2.0调用端
    /// </summary>
    public class OAuthClient
    {
        /// <summary>
        /// OAuth配置
        /// </summary>
        public OAuthClientConfig Config { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        public OAuthClient(OAuthClientConfig config)
        {
            Config = config;
        }

        /// <summary>
        /// 获取AuthorizationCode
        /// </summary>
        public virtual void BeginAuthentication()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Redirect(GetAuthenticationUrl());
            }
        }

        /// <summary>
        /// 获取授权的地址
        /// </summary>
        /// <returns></returns>
        public virtual string GetAuthenticationUrl()
        {
            return string.Format(
                    "{0}?client_id={1}&response_type=code&redirect_uri={2}&scope={3}",
                    Config.AuthorizeUrl,
                    Config.ConsumerKey,
                    Config.CallbackUrl,
                    Config.Scope
                );
        }

        /// <summary>
        /// 使用Authentication Code获取Access Token
        /// </summary>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        public virtual OAuthAccessToken GetAccessToken(string authorizationCode)
        {
            var args = new Dictionary<string, string>();
            args.Add("grant_type", "authorization_code");
            args.Add("code", authorizationCode);
            args.Add("client_id", Config.ConsumerKey);
            args.Add("client_secret", Config.ConsumerSecret);
            args.Add("redirect_uri", Config.CallbackUrl);
            args.Add("scope", Config.Scope);

            return GetRemoteAccessToken(args);
        }

        /// <summary>
        /// 使用Refresh Token获取Access Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public virtual OAuthAccessToken RefreshAccessToken(string refreshToken)
        {
            var args = new Dictionary<string, string>();
            args.Add("grant_type", "refresh_token");
            args.Add("refresh_token", refreshToken);
            args.Add("client_id", Config.ConsumerKey);
            args.Add("client_secret", Config.ConsumerSecret);
            args.Add("scope", Config.Scope);

            return GetRemoteAccessToken(args);
        }

        /// <summary>
        /// 执行HTTP请求以获取数据
        /// </summary>
        /// <param name="method"></param>
        /// <param name="requestUrl"></param>
        /// <param name="requestArgs"></param>
        /// <param name="encoding"></param>
        /// <exception cref="T:Projects.Tool.OAuth.OAuthNetworkException"></exception>
        /// <exception cref="T:Projects.Tool.OAuth.OAuthException"></exception>
        /// <returns></returns>
        public string Invoke(HttpMethod method, string requestUrl, string requestArgs, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            return (new OAuthNetworkService()).MakeHttpRequest(method, requestUrl, requestArgs, null, encoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected OAuthAccessToken GetRemoteAccessToken(IEnumerable<KeyValuePair<string, string>> args)
        {
            try
            {
                var response = Invoke(
                    HttpMethod.POST,
                    Config.TokenUrl,
                    string.Join("&", args.Select(arg => string.Format("{0}={1}", arg.Key, HttpHelper.UrlEncode(arg.Value))))
                    );
                return JsonConverter.FromJson<OAuthAccessToken>(response);
            }
            catch (OAuthNetworkException oane)
            {
                throw new OAuthUnauthorizedException("获取OAuth授权失败", oane.ResponseText, oane);
            }
            catch (OAuthException oae)
            {
                throw oae;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
