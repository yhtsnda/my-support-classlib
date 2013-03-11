using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    public interface IGetter
    {
        object Get(object target);

        string PropertyName { get; }
    }
}
