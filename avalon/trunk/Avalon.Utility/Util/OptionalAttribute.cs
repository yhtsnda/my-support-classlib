using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    public class OptionalAttribute : Attribute
    {
    }
}
