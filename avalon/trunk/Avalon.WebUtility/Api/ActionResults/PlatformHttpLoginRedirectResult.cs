using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 重定向到登录页面的返回结果。其中 LoginUrl 默认为 FormsAuthentication.LoginUrl。
    /// </summary>
    public class PlatformHttpLoginRedirectResult : HttpStatusCodeResult
    {
        public const string DefaultReturnUrlParamName = "rtn";
        string loginUrl;
        string returnUrlParamName;

        public PlatformHttpLoginRedirectResult()
            : this(null, null, null, null)
        { }

        public PlatformHttpLoginRedirectResult(string returnUrl, string loginUrl = "", string retrunUrlParamName = null)
            : this(returnUrl, null, loginUrl, retrunUrlParamName)
        { }

        public PlatformHttpLoginRedirectResult(string returnUrl, string statusDescription, string loginUrl = null, string returnUrlParamName = null)
            : base(302, statusDescription)
        {
            ReturnUrl = returnUrl;
            this.loginUrl = loginUrl;
            this.returnUrlParamName = returnUrlParamName;
        }

        public string ReturnUrl
        {
            get;
            private set;
        }

        public string LoginUrl
        {
            get { return loginUrl; }
            set { loginUrl = value; }
        }

        public string ReturnUrlParamName
        {
            get { return returnUrlParamName; }
            set { returnUrlParamName = value; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            base.ExecuteResult(context);

            if (String.IsNullOrEmpty(loginUrl))
                loginUrl = FormsAuthentication.LoginUrl;

            if (String.IsNullOrEmpty(returnUrlParamName))
                returnUrlParamName = DefaultReturnUrlParamName;

            if (!String.IsNullOrEmpty(ReturnUrl))
            {
                loginUrl += loginUrl.Contains("?") ? "&" : "?";
                loginUrl += returnUrlParamName + "=" + HttpUtility.UrlEncode(ReturnUrl);
            }
            context.HttpContext.Response.RedirectLocation = loginUrl;
        }
    }
}