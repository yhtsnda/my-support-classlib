using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace BuildingSiteCheck.Mvc
{
    public class BuildingSiteCheckHttpLoginRedirectResult : HttpStatusCodeResult
    {
        public const string DefaultReturnUrlParamName = "rtn";
        private string mLoginUrl = String.Empty;
        private string mReturnUrlParamName = String.Empty;

        public string ReturnUrl { get; private set; }

        public string LoginUrl
        {
            get { return mLoginUrl; }
            set { mLoginUrl = value; }
        }

        public string ReturnUrlParamName
        {
            get { return mReturnUrlParamName; }
            set { mReturnUrlParamName = value; }
        }

        public BuildingSiteCheckHttpLoginRedirectResult(string returnUrl, string statusDescription, 
            string loginUrl = null, string returnUrlParamName = null) : base(302, statusDescription)
        {
            ReturnUrl = returnUrl;
            this.mLoginUrl = loginUrl;
            this.mReturnUrlParamName = returnUrlParamName;
        }

        public BuildingSiteCheckHttpLoginRedirectResult(string returnUrl, string loginUrl = "", string returnUrlParamName=null):
            this(returnUrl,null,loginUrl,returnUrlParamName)
        {
            
        }

        public BuildingSiteCheckHttpLoginRedirectResult():this(null,null,null,null)
        {
            
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            base.ExecuteResult(context);

            if (String.IsNullOrEmpty(mLoginUrl))
                mLoginUrl = FormsAuthentication.LoginUrl;
            if (String.IsNullOrEmpty(mReturnUrlParamName))
                mReturnUrlParamName = DefaultReturnUrlParamName;

            if (!String.IsNullOrEmpty(ReturnUrl))
            {
                mLoginUrl += mLoginUrl.Contains("?") ? "&" : "?";
                mLoginUrl += mReturnUrlParamName + "=" + HttpUtility.UrlEncode(ReturnUrl);
            }
            context.HttpContext.Response.RedirectLocation = mLoginUrl;
        }
    }
}
