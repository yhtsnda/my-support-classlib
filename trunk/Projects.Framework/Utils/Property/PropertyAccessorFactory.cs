using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 对象属性访问工厂
    /// </summary>
    public class PropertyAccessorFactory
    {
        private static Dictionary<Type, IPropertyAccessor> mAccessors = 
            new Dictionary<Type, IPropertyAccessor>();
        private static object mSyncRoot = new object();

        public static IPropertyAccessor GetPropertyAccess(Type type)
        {
            IPropertyAccessor accessor;
            if (!mAccessors.TryGetValue(type, out accessor))
            {
                lock (mSyncRoot)
                {
                    if (!mAccessors.TryGetValue(type, out accessor))
                    {
                        accessor = new DefaultPropertyAccessor(type);
                        mAccessors.Add(type, accessor);
                    }
                }
            }
            return accessor;
        }
    }
}
