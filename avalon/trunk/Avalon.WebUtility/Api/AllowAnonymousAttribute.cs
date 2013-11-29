using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// Actions and controllers with the AllowAnonymous attribute are skipped by the Authorize attribute on authorization. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowAnonymousAttribute : Attribute
    {
    }
}