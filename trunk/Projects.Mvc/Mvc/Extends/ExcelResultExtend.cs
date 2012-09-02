using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingSiteCheck.Mvc;

namespace System.Web.Mvc
{
    public static class ExcelResultExtend
    {
        public static ActionResult Excel<T>(this Controller controller, IList<T> collection, string fileName, string title, string[] columns)
        {
            return new ExcelResult<T>(collection, fileName, title, columns);
        }
    }
}
