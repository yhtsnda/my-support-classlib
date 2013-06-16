using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 凭证的响应类
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// 授权凭证
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 凭证类型
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// 过期时长(秒)
        /// </summary>
        public int ExpiredIn { get; set; }

        /// <summary>
        /// 自刷新凭证
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 凭证作用范围
        /// </summary>
        public IList<string> Scope { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
    }

    /// <summary>
    /// 获取凭证失败的响应类
    /// </summary>
    public class TokenFailedResponse
    {
        public string Error { get; set; }

        public string ErrorDescription { get; set; }

        public Uri ErrorUri { get; set; }
    }
}
