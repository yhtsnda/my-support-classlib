using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 自定义方法名选择器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CustomActionNameAttribute : ActionNameSelectorAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public CustomActionNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            name = name.TrimEnd('/');
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            Name = name;
        }

        public string Name { get; private set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (controllerContext.HttpContext == null || controllerContext.HttpContext.Request == null)
                return false;

            var absolutePath = controllerContext.HttpContext.Request.Url.AbsolutePath;
            if (string.IsNullOrEmpty(absolutePath))
                return false;

            //HHB 必须将 controllerName 加入判断 2013-04-26
            var controllerName = controllerContext.Controller.GetType().Name.ToLower();
            if (controllerName.EndsWith("controller"))
                controllerName = controllerName.Remove(controllerName.Length - 10);

            var path = "/" + controllerName + "/" + Name;
            absolutePath = absolutePath.TrimEnd('/');
            return absolutePath.EndsWith(path, StringComparison.OrdinalIgnoreCase);
        }
    }
}
