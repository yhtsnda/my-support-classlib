using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 类属性读取器接口
    /// </summary>
    public interface IPropertyAccessor
    {
        Type EntityType { get; }

        IGetter GetGetter(string propertyName);

        object[] GetPropertyValues(object target);

        void SetPropertyValues(object target, object[] values);

        object CreateInstance();

        IDictionary ToDictionary(object target);
    }
}
