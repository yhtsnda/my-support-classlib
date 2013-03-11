using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Util;

namespace Projects.Tool.OAuth
{
    /// <summary>
    /// 
    /// </summary>
    public class OAuthAccessToken
    {
        /// <summary>
        /// Access Token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Access Token的有效期，单位秒
        /// </summary>
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        /// <summary>
        /// Refresh Token
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Scope
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
