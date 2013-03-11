using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.OAuth
{
    /// <summary>
    /// 
    /// </summary>
    public class OAuthClientConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string AuthorizeUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string TokenUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string CallbackUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConsumerKey { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConsumerSecret { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Scope { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="authorizeUrl"></param>
        /// <param name="tokenUrl"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="scope"></param>
        public OAuthClientConfig(string consumerKey, string consumerSecret,
            string authorizeUrl, string tokenUrl,
            string callbackUrl = "oob",
            string scope = "basic")
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            AuthorizeUrl = authorizeUrl;
            TokenUrl = tokenUrl;
            CallbackUrl = callbackUrl;
            Scope = scope;
        }
    }
}
