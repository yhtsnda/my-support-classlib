using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class ValidatorException : Exception
    {
        public ValidatorException()
        {
        }

        public ValidatorException(string message)
            : base(message)
        {
        }

        public ValidatorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
