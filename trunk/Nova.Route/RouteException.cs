using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Route
{
    public class RouteException : Exception
    {
        public RouteException()
            : base()
        {
        }

        public RouteException(string message, Exception innerException) 
            : base(message, innerException)
        {

        }

        public RouteException(string message)
            : base(message)
        {
        }

        public RouteException(Exception innerException)
            : base("not contain exception message", innerException)
        {
        }
    }
}
