using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    public interface IPropertyAccessor
    {
        Type EntityType { get; }

        IGetter GetGetter(string propertyName);

        ISetter GetSetter(string propertyName);

        object CreateInstance();

        Func<object, object[]> GetDatasHandler { get; }

        Action<object, object[]> SetDatasHandler { get; }

        void MergeData(object source, object destination);

        IDictionary ToDictionary(object target);
    }
}
