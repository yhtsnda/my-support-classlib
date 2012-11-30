using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 默认的Getter的动态实现
    /// </summary>
    public class DefaultGetter : IGetter
    {
        PropertyInfo mMethod;
        string mPropertyName;
        Func<object, object> mGetter;

        public DefaultGetter(PropertyInfo method)
        {
            this.mMethod = method;
            this.mPropertyName = method.Name;
            CreateGetter();
        }

        void CreateGetter()
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var property = Expression.Property(Expression.Convert(instance, mMethod.ReflectedType), mMethod);
            var main = Expression.Convert(property, typeof(object));
            mGetter = (Func<object, object>)Expression
                .Lambda(typeof(Func<object, object>), main, instance)
                .Compile();
        }

        /// <summary>
        /// 属性的Get方法
        /// </summary>
        /// <param name="target">实例对象</param>
        /// <returns></returns>
        public object Get(object target)
        {
            return mGetter(target);
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get { return mPropertyName; }
        }
    }
}
