using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Avalon.WebUtility
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ODataQueryAttribute : ActionFilterAttribute
    {
        Type filterType;
        Type resultType;

        public ODataQueryAttribute(Type filterType, Type resultType)
        {
            this.filterType = filterType;
            this.resultType = resultType;
        }

        public Type FilterType
        {
            get { return filterType; }
        }

        public Type ResultType
        {
            get { return resultType; }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ismetadata = filterContext.HttpContext.Request.QueryString.AllKeys.Contains("$metadata");
            if (ismetadata)
            {
                ODataQueryMetadata metadata = new ODataQueryMetadata()
                {
                    FilterType = new ODataType(filterType),
                    ResultType = new ODataType(resultType)
                };
                filterContext.Result = new JsonResult() { Data = metadata, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            base.OnActionExecuting(filterContext);
        }

        class ODataQueryMetadata
        {
            public ODataType FilterType { get; set; }

            public ODataType ResultType { get; set; }
        }

        class ODataType
        {
            public ODataType(Type type)
            {
                Name = type.Name;
                FullName = type.FullName;
                Properties = type.GetProperties().Select(o => new ODataProperty(o)).ToList();
            }
            public string Name { get; set; }

            public string FullName { get; set; }

            public List<ODataProperty> Properties { get; set; }
        }

        class ODataProperty
        {
            public ODataProperty(PropertyInfo property)
            {
                Name = property.Name;
                Type = property.PropertyType.Name;
                FullName = property.PropertyType.FullName;
            }

            public string Name { get; set; }

            public string Type { get; set; }

            public string FullName { get; set; }
        }
    }
}
