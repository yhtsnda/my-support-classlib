using System.Web.Mvc;

namespace Projects.Framework.Web
{
    /// <summary>
    /// 未授权的返回结果
    /// </summary>
    public class FrameworkHttpUnauthorizedResult : HttpUnauthorizedResult
    {
        public FrameworkHttpUnauthorizedResult()
            : base()
        { }

        public FrameworkHttpUnauthorizedResult(string statusDescription)
            : base(statusDescription)
        { }
    }
}
