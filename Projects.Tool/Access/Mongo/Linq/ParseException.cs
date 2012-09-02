using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Projects.Tool.MongoAccess
{
    public class ParserException : Exception
    {
        public ParserException(string message, Expression expr)
            : base(message)
        {
        }
    }
}
