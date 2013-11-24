using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{

    /// <summary>
    /// 两对象属性的映射关系
    /// </summary>
    public class PropertyMapping
    {
        public PropertyInfo Property { get; set; }

        public PropertyInfo MappingProperty { get; set; }

        public Type MappingType { get; set; }

        public override string ToString()
        {
            return Property.Name + ":" + MappingProperty.Name;
        }
    }
}
