using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Exceptions
{
    public class IExceptionHandler
    {
        /// <summary>
        /// 传递特定的异常
        /// </summary>
        /// <param name="ex">要被传递的异常</param>
        /// <returns>如果传递成功,返回true;否则返回false;</returns>
        bool HandleException(Exception ex);
    }
}
