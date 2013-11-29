using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 自定视图引擎
    /// </summary>
    public class CustomViewEngine : BuildManagerViewEngine
    {
        /// <summary>
        /// 自定视图引擎构造函数
        /// </summary>
        public CustomViewEngine()
            : this(null, null)
        {
        }

        /// <summary>
        /// 自定视图引擎构造函数
        /// </summary>
        /// <param name="customFolders">自定义视图查找目录</param>
        public CustomViewEngine(IEnumerable<string> customFolders)
            : this(customFolders, null)
        {
        }

        /// <summary>
        /// 自定视图引擎构造函数
        /// </summary>
        /// <param name="customFolders">自定义视图查找目录</param>
        /// <param name="viewPageActivator">对使用依赖项注入实例化视图页的方式进行精细控制</param>
        public CustomViewEngine(IEnumerable<string> customFolders, IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            List<string> areaLocationFormats = new List<string>() 
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
            };
            List<string> locationFormats = new List<string>() 
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
            };
            if (customFolders != null)
            {
                areaLocationFormats.AddRange(customFolders.Select(o => string.Concat("~/Areas/{2}/Views/", o, "/{0}.cshtml")));
                locationFormats.AddRange(customFolders.Select(o => string.Concat("~/Views/", o, "/{0}.cshtml")));
            }
            string[] areaLocationFormatArray = areaLocationFormats.ToArray();
            string[] locationFormatArray = locationFormats.ToArray();

            AreaViewLocationFormats = areaLocationFormatArray;
            AreaMasterLocationFormats = areaLocationFormatArray;
            AreaPartialViewLocationFormats = areaLocationFormatArray;

            ViewLocationFormats = locationFormatArray;
            MasterLocationFormats = locationFormatArray;
            PartialViewLocationFormats = locationFormatArray;

            FileExtensions = new[] {
                "cshtml",
                "vbhtml",
            };
        }

        /// <summary>
        /// 自定视图引擎构造函数
        /// </summary>
        /// <param name="viewPageActivator">对使用依赖项注入实例化视图页的方式进行精细控制</param>
        public CustomViewEngine(IViewPageActivator viewPageActivator)
            : this(null, viewPageActivator)
        {
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new RazorView(
                controllerContext,
                partialPath,
                layoutPath: null,
                runViewStartPages: false,
                viewStartFileExtensions: FileExtensions,
                viewPageActivator: ViewPageActivator
            );
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new RazorView(
                controllerContext,
                viewPath,
                layoutPath: masterPath,
                runViewStartPages: true,
                viewStartFileExtensions: FileExtensions,
                viewPageActivator: ViewPageActivator
            );
        }
    }
}
