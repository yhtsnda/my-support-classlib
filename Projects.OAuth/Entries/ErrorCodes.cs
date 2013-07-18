using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 验证请求的错误码
    /// </summary>
    public static class AuthRequestErrorCode
    {
        /// <summary>
        /// 请求缺少某个必需参数，包含一个不支持的参数或参数值，或者格式不正确。
        /// </summary>
        public const string InvalidRequest = "invalid_request";

        /// <summary>
        /// 提供的客户端标识符是无效的。
        /// </summary>
        public const string InvoidClient = "invalid_client";

        /// <summary>
        /// 客户端没有权限使用该请求的响应类型。
        /// </summary>
        public const string UnauthorizedClient = "unauthorized_client";

        /// <summary>
        /// 提供的重定向URI与预先注册的值不匹配。
        /// </summary>
        public const string RedirectUriMismatch = "redirect_uri_mismatch";

        /// <summary>
        /// 终端用户或授权服务器拒绝了请求。
        /// </summary>
        public const string AccessDenied = "access_denied";

        /// <summary>
        /// 请求的响应类型不为授权服务器所支持。
        /// </summary>
        public const string UnsupportedResponseType = "unsupported_response_type";

        /// <summary>
        /// 请求的作用域是无效的、未知的，或格式不正确的。
        /// </summary>
        public const string InvalidScope = "invalid_scope";

        /// <summary>
        /// 验证服务器发生不可预期的错误
        /// </summary>
        public const string ServerError = "server_error";

        /// <summary>
        /// 验证服务器发生暂时性的过载错误
        /// </summary>
        public const string TemporarilyUnavailable = "temporarily_unavailable";
    }

    /// <summary>
    /// AccessToken请求错误码
    /// </summary>
    public static class AccessTokenRequestErrorCode
    {
        /// <summary>
        /// 请求缺少某个必需参数，包含一个不支持的参数或参数值，或者格式不正确。
        /// </summary>
        public const string InvalidRequest = "invalid_request";

        /// <summary>
        /// 提供的客户端标识符是无效的。
        /// </summary>
        public const string InvoidClient = "invalid_client";

        /// <summary>
        /// 客户端没有权限使用该请求的响应类型。
        /// </summary>
        public const string UnauthorizedClient = "unauthorized_client";

        /// <summary>
        /// 提供的访问许可是无效的、过期的或已撤销的
        /// </summary>
        public const string InvalidGrant = "invalid_grant";

        /// <summary>
        /// 提供的重定向URI与预先注册的值不匹配。
        /// </summary>
        public const string RedirectUriMismatch = "redirect_uri_mismatch";

        /// <summary>
        /// 包含的访问许可——它的类型或其它属性——不被授权服务器所支持。
        /// </summary>
        public const string UnsupportedGrantType = "unsupported_grant_type";

        /// <summary>
        /// 包含的帐号类型——它的类型或其它属性——不被授权服务器所支持。
        /// </summary>
        public const string UnsupportedAccountType = "unsupported_account_type";

        /// <summary>
        /// 请求的响应类型不为授权服务器所支持。
        /// </summary>
        public const string UnsupportedResponseType = "unsupported_response_type";

        /// <summary>
        /// 请求的作用域是无效的、未知的，或格式不正确的。
        /// </summary>
        public const string InvalidScope = "invalid_scope";
    }

    /// <summary>
    /// 持有票据错误
    /// </summary>
    public static class BearerTokenErrorCode
    {
        /// <summary>
        /// 请求缺少某个必需参数，包含一个不支持的参数或参数值，参数重复，使用多种方式包含访问令牌，或者请求格式不正确。
        /// </summary>
        /// <remarks>
        /// 资源服务器应该使用HTTP 400（Bad Request）状态码进行响应。
        /// </remarks>
        public const string InvalidRequest = "invalid_request";

        /// <summary>
        /// 提供的访问令牌是过期的、已撤销的、格式不正确的，或由于其它原因是无效的。
        /// </summary>
        /// <remarks>
        /// 资源服务器应该使用HTTP 401（Unauthorized）状态码进行响应。客户端可能请求一个新的访问令牌并重试受保护资源请求。
        /// </remarks>
        public const string InvalidToken = "invalid_token";

        /// <summary>
        /// 请求需要比访问令牌所提供的权限更高的权限。
        /// </summary>
        /// <remarks>
        /// 资源服务器应该使用HTTP 403（Forbidden）状态码进行响应并且包含“scope”属性，带上访问该受保护资源必需的作用域。
        /// </remarks>
        public const string InsufficientScope = "insufficient_scope";
    }
}
