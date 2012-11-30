﻿using System.Web.Mvc;

namespace Projects.Mvc
{
    /// <summary>
    /// 未授权的返回结果
    /// </summary>
    public class BuildingSiteCheckHttpUnauthorizedResult : HttpUnauthorizedResult
    {
        public BuildingSiteCheckHttpUnauthorizedResult()
            : base()
        { }

        public BuildingSiteCheckHttpUnauthorizedResult(string statusDescription)
            : base(statusDescription)
        { }
    }
}