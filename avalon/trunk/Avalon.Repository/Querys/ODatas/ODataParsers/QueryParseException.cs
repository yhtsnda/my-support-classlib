using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class QueryParseException : Exception
    {
        public QueryParseException(string format, params object[] args)
            : base(String.Format(format, args))
        {
        }
    }
}
