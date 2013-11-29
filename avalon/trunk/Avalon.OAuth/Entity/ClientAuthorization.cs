using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    public class ClientAuthorization
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        public virtual int ClientId { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 客户端描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        public virtual string Secret { get; set; }

        /// <summary>
        /// 授权范围
        /// </summary>
        public virtual IList<string> Scopes { get; set; }

        /// <summary>
        /// 授权状态
        /// </summary>
        public virtual ClientAuthorizeStatus Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 验证码出现的规则
        /// </summary>
        public virtual VerifyCodeType VerifyCodeType { get; set; }

        /// <summary>
        /// 回调的路径
        /// </summary>
        public virtual string RedirectPath { get; set; }

        public virtual void ValidRedirectUri(Uri redirectUri)
        {
            var rpUri = new Uri(RedirectPath);
            if (!String.Equals(redirectUri.AbsoluteUri, rpUri.AbsoluteUri, StringComparison.InvariantCulture))
                throw new OAuthException(AuthorizationRequestErrorCodes.RedirectUriMismatch, "redirect uri mismatch.", 400);
        }
    }
}
