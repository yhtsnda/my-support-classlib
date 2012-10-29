using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Exceptions
{
    public class ExceptionManager
    {
        static ExceptionManager();
        private ExceptionManager()
        {

        }

        public static bool HandleException(Exception ex)
        {
            return false;
        }
    }
}
