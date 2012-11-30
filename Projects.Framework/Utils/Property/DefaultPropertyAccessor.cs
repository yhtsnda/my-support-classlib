using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Projects.Tool.Reflection;

namespace Projects.Framework
{
    internal class DefaultPropertyAccessor : IPropertyAccessor
    {
        Type mEntityType;
        Func<object, object[]> mGetterValuesHandler;
        Action<object, object[]> mSetterValuesHandler;
        Func<object, IDictionary> mDictionaryHandler;
        Dictionary<string, IGetter> mGetters;
        PropertyInfo[] mProperties;

        public DefaultPropertyAccessor(Type entityType)
        {
            this.mEntityType = entityType;
            var properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.GetProperty).Where(o => o.CanWrite).ToList();

        }

        #region IPropertyAccessor接口实现
        public Type EntityType
        {
            get { return mEntityType; }
        }

        public IGetter GetGetter(string propertyName)
        {
            return mGetters.TryGetValue(propertyName);
        }

        public object[] GetPropertyValues(object target)
        {
            return mGetterValuesHandler(target);
        }

        public void SetPropertyValues(object target, object[] values)
        {
            mSetterValuesHandler(target, values);
        }

        public object CreateInstance()
        {
            return Projects.Tool.Reflection.FastActivator.Create(mEntityType);
        }

        public IDictionary ToDictionary(object target)
        {
            return mDictionaryHandler(target);
        }
        #endregion 

        #region 私有方法
        private void CreateGetters(List<PropertyInfo> properties)
        {
            mGetters = new Dictionary<string, IGetter>();
            foreach (var prop in properties)
                mGetters.Add(prop.Name, new DefaultGetter(prop));
        }

        /// <summary>
        /// 创建Setter函数
        /// </summary>
        private void CreateSetterValues(Type entityType, List<PropertyInfo> properties)
        {
            var param1 = Expression.Parameter(typeof(object), "target");
            var param2 = Expression.Parameter(typeof(object[]), "values");

            List<Expression> blocks = new List<Expression>();
            var target = Expression.Variable(entityType, "entity");
            blocks.Add(Expression.Assign(target, Expression.Convert(param1, entityType)));
            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var value = Expression.ArrayAccess(param2, Expression.Constant(i));
                blocks.Add(Expression.Call(target, property.GetSetMethod(true), 
                    Expression.Convert(value, property.PropertyType)));
            }
            var main = Expression.Block(new ParameterExpression[] { target }, blocks);
            mSetterValuesHandler = (Action<object, object[]>)Expression
                .Lambda(typeof(Action<object, object[]>), main, param1, param2)
                .Compile();
        }

        private void CreateGetterValues(Type entityType, List<PropertyInfo> properties)
        {
        }
        #endregion
    }
}
