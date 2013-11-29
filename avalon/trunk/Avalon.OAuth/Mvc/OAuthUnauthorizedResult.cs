using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Avalon.OAuth
{
    //public class OAuthUnauthorizedResult : OpenApiDataResult
    //{
    //    public OAuthUnauthorizedResult()
    //    {
    //        //this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
    //    }

    //    public string ErrorCode { get; set; }

    //    public string ErrorDescription { get; set; }

    //    public Uri ErrorUri { get; set; }

    //    public string Realm { get; set; }

    //    public HashSet<string> Scope { get; set; }

    //    public HttpStatusCode HttpStatusCode { get; set; }

    //    internal virtual string Scheme
    //    {
    //        get { return "Bearer"; }
    //    }

    //    public override void ExecuteResult(ControllerContext context)
    //    {
    //        if (context == null)
    //            throw new ArgumentNullException("content");

    //        var response = context.HttpContext.Response;
    //        response.StatusCode = HttpStatusCode;
    //        if (statusDescription != null)
    //            response.StatusDescription = ErrorCode;

    //        response.Headers.Set(HttpRequestHeaders.WwwAuthenticate, "OAuth2");
    //        Data = new InvokeResult<object>(HttpStatusCode, ErrorCode);

    //        base.ExecuteResult(context);
    //    }

    //    internal static OAuthUnauthorizedResult InvalidRequest(OAuthException exception)
    //    {
    //        if (exception == null)
    //            throw new ArgumentNullException("exception");

    //        return new OAuthUnauthorizedResult()
    //        {
    //            ErrorCode = BearerTokenErrorCodes.InvalidRequest,
    //            ErrorDescription = exception.Message,
    //            HttpStatusCode = HttpStatusCode.BadRequest
    //        };
    //    }

    //    internal static OAuthUnauthorizedResult InvalidToken(OAuthException exception)
    //    {
    //        if (exception == null)
    //            throw new ArgumentNullException("exception");

    //        return new OAuthUnauthorizedResult()
    //        {
    //            ErrorCode = BearerTokenErrorCodes.InvalidToken,
    //            ErrorDescription = exception.Message,
    //            HttpStatusCode = HttpStatusCode.Unauthorized
    //        };
    //    }

    //    internal static OAuthUnauthorizedResult InsufficientScope(HashSet<string> requiredScopes)
    //    {
    //        if (requiredScopes == null)
    //            throw new ArgumentNullException("requiredScopes");

    //        return new OAuthUnauthorizedResult()
    //        {
    //            ErrorCode = BearerTokenErrorCodes.InsufficientScope,
    //            HttpStatusCode = HttpStatusCode.Forbidden,
    //            Scope = requiredScopes
    //        };
    //    }
    //}
}
