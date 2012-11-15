using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Projects.Mvc
{
    public class CustomViewEngine : VirtualPathProviderViewEngine
    {
        public CustomViewEngine()
            : this(null, null)
        {
        }

        public CustomViewEngine(IEnumerable<string> customFolders)
            : this(customFolders, null)
        {
        }

        public CustomViewEngine(IEnumerable<string> customFolders, IViewPageActivator viewPageActivator) 
            : base()
        {
            List<string> areaLocationFormats = new List<string>()
            {
                "~Areas/{2}/Views/{1}/{0}.cshtml",
                "~Areas/{1}/Views/Shared/{0}.cshtml"
            };
            List<string> locationFormats = new List<string>()
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
            if (customFolders != null)
            {
                areaLocationFormats.AddRange(
                    customFolders.Select(o => String.Concat("~/Areas/{2}/Views/", o, "/{0}.cshtml")));
                areaLocationFormats.AddRange(
                    customFolders.Select(o => String.Concat("~/Areas/", o, "/Views/{1}/{0}.cshtml")));
                locationFormats.AddRange(customFolders.Select(o => String.Concat("~/Views/", o, "/{0}.cshtml")));
                locationFormats.AddRange(customFolders.Select(o => String.Concat("~/Areas/",o ,"/Views/{1}/{0}.cshtml")));
            }
            string[] areaLocationFormatsArray = areaLocationFormats.ToArray();
            string[] locationFormatsArray = locationFormats.ToArray();

            AreaViewLocationFormats = areaLocationFormatsArray;
            AreaMasterLocationFormats = areaLocationFormatsArray;
            AreaPartialViewLocationFormats = areaLocationFormatsArray;

            ViewLocationFormats = locationFormatsArray;
            MasterLocationFormats = locationFormatsArray;
            PartialViewLocationFormats = locationFormatsArray;

            FileExtensions = new[] { "cshtml", "vbhtml"};
        }

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
                viewStartFileExtensions: FileExtensions);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new RazorView(
                controllerContext, 
                viewPath, 
                layoutPath: masterPath, 
                runViewStartPages: true, 
                viewStartFileExtensions: FileExtensions);
        }
    }
}
