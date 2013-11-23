using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.Web;

namespace Avalon.Utility
{
    public static class FileHelper
    {
        /// <summary>
        /// 将虚拟路径映射到服务器上的物理路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else if (HostingEnvironment.IsHosted)
            {
                return HostingEnvironment.MapPath(path);
            }
            else if (VirtualPathUtility.IsAppRelative(path))
            {
                string physicalPath = VirtualPathUtility.ToAbsolute(path, "/");
                physicalPath = physicalPath.Replace('/', '\\');
                physicalPath = physicalPath.Substring(1);
                physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, physicalPath);

                return physicalPath;
            }
            else
            {
                throw new Exception(string.Format("文件路径无法解析: {0}", path));
            }
        }
    }
}
