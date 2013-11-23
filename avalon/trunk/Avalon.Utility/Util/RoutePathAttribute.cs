using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 路由的路径，用于文档的生成。
    /// </summary>
    public class RoutePathAttribute : Attribute
    {
        public RoutePathAttribute(string path)
        {
            Path = path;
        }

        public string Path
        {
            get;
            private set;
        }
    }
}
