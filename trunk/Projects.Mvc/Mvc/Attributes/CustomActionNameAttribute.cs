using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Projects.Mvc
{
    /// <summary>
    /// 自定义的方法选择器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CustomActionNameAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }

        public CustomActionNameAttribute(string name)
        {
            if(String.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            name = name.TrimEnd('/');
            if(String.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            Name = name;
        }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (controllerContext.HttpContext == null || controllerContext.HttpContext.Request == null)
            {
                return false;
            }
            var absolutePath = controllerContext.HttpContext.Request.Url.AbsolutePath;
            if (String.IsNullOrEmpty(absolutePath))
                return false;

            absolutePath = absolutePath.TrimEnd('/');
            return absolutePath.EndsWith(Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
