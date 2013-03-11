using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public interface IAccessValues
    {
        object[] GetPropertyValues(object target);

        void SetPropertyValues(object target, object[] values);

        IDictionary ToDictionary(object target);
    }
}
