using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 授权请求
    /// </summary>
    public class AuthRequest
    {
        public AuthRequest(int clientId, AuthResponseType responseType, Uri redirectUri, string state = null)
        {
            this.ClientId = clientId;
            this.ResponseType = responseType;
            this.RedirectUri = redirectUri;
            this.State = state;
            Scope = new List<string>();
        }

        /// <summary>
        /// 请求的客户端ID
        /// </summary>
        public int ClientId { get; protected set; }

        /// <summary>
        /// 响应的类型
        /// </summary>
        public AuthResponseType ResponseType { get; protected set; }

        /// <summary>
        /// 回调跳转的地址
        /// </summary>
        public Uri RedirectUri { get; protected set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public string State { get; protected set; }

        /// <summary>
        /// 授权范围
        /// </summary>
        public IList<string> Scope { get; protected set; }
    }
}
