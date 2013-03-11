
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework
{
    internal interface ITargetAccess
    {
        object Get(object entity);

        void Set(object entity, object value);
    }

    class DefaultTargetAccess : ITargetAccess
    {
        Func<object, object> getter;
        Action<object, object> setter;

        public DefaultTargetAccess(Func<object, object> getter, Action<object, object> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }

        public object Get(object entity)
        {
            return getter(entity);
        }

        public void Set(object entity, object value)
        {
            setter(entity, value);
        }
    }

    internal class TargetAccessImpl
    {
        const string TargetField = "__target";
        static Dictionary<Type, ITargetAccess> dic = new Dictionary<Type, ITargetAccess>();

        public static ITargetAccess GetAcess(Type type)
        {
            var access = dic.TryGetValue(type);
            if (access == null)
            {
                access = new DefaultTargetAccess(CreateGetter(type), CreateSetter(type));
                dic.Add(type, access);
            }
            return access;
        }

        static Func<object, object> CreateGetter(Type type)
        {
            var entity = Expression.Parameter(typeof(object), "entity");
            var field = Expression.Field(Expression.Convert(entity, type), TargetField);
            var main = Expression.Convert(field, typeof(object));

            return (Func<object, object>)Expression.Lambda(typeof(Func<object, object>), main, entity).Compile();
        }

        static Action<object, object> CreateSetter(Type type)
        {
            var entity = Expression.Parameter(typeof(object), "entity");
            var value = Expression.Parameter(typeof(object), "value");

            var main = Expression.Assign(Expression.Field(Expression.Convert(entity, type), type, TargetField), value);

            return (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, entity, value).Compile();
        }
    }
}
