using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public interface ISetter
    {
        void Set(object target, object value);

        string PropertyName { get; }
    }
}
