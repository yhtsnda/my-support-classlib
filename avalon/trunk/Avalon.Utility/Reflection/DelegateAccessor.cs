using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;

namespace Avalon.Utility
{
    public static class DelegateAccessor
    {
        public static Func<object, object> CreatePropertyGetter(PropertyInfo property)
        {
            var expInstance = Expression.Parameter(typeof(object), "instance");
            var expProperty = Expression.Property(Expression.Convert(expInstance, property.ReflectedType), property);
            var main = Expression.Convert(expProperty, typeof(object));

            return (Func<object, object>)Expression.Lambda(typeof(Func<object, object>), main, expInstance).Compile();
        }

        public static Action<object, object> CreatePropertySetter(PropertyInfo property)
        {
            if (property.ReflectedType.IsValueType)
            {
                var refHandler = CreateSetMethod(property);
                return new Action<object, object>((instance, value) =>
                {
                    refHandler(ref instance, value);
                });
            }
            else
            {
                var expInstance = Expression.Parameter(typeof(object), "instance");
                var expValue = Expression.Parameter(typeof(object), "value");

                var main = Expression.Call(
                    Expression.Convert(expInstance, property.ReflectedType),
                    property.GetSetMethod(true),
                    Expression.Convert(expValue, property.PropertyType));

                return (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, expInstance, expValue).Compile();
            }
        }

        public static Func<object, object> CreateFieldGetter(FieldInfo field)
        {
            var expInstance = Expression.Parameter(typeof(object), "instance");
            var expField = Expression.Field(Expression.Convert(expInstance, field.ReflectedType), field);
            var main = Expression.Convert(expField, typeof(object));

            return (Func<object, object>)Expression.Lambda(typeof(Func<object, object>), main, expInstance).Compile();
        }

        public static Action<object, object> CreateFieldSetter(FieldInfo field)
        {
            if (field.IsInitOnly || field.ReflectedType.IsValueType)
            {
                var refHandler = CreateSetMethod(field);
                return new Action<object, object>((instance, value) =>
                {
                    refHandler(ref instance, value);
                });
            }
            else
            {
                var expInstance = Expression.Parameter(typeof(object), "instance");
                var expValue = Expression.Parameter(typeof(object), "value");
                var expField = Expression.Field(Expression.Convert(expInstance, field.ReflectedType), field);

                var main = Expression.Assign(expField, Expression.Convert(expValue, field.FieldType));
                return (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, expInstance, expValue).Compile();
            }
        }

        public static Action<object, object> CreateFieldCloner(FieldInfo field)
        {
            if (field.ReflectedType.IsValueType)
            {
                return new Action<object, object>((source, target) =>
                {
                    var value = CreateFieldGetter(field)(source);
                    CreateFieldSetter(field)(target, value);
                });
            }
            else
            {
                var expSource = Expression.Parameter(typeof(object), "source");
                var expTarget = Expression.Parameter(typeof(object), "target");

                var expFieldLeft = Expression.Field(Expression.Convert(expTarget, field.ReflectedType), field);
                var expFieldRight = Expression.Field(Expression.Convert(expSource, field.ReflectedType), field);

                var main = Expression.Assign(expFieldLeft, expFieldRight);
                return (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, expSource, expTarget).Compile();
            }
        }

        public static Action<object, object> CreatePropertyCloner(PropertyInfo property)
        {
            if (property.ReflectedType.IsValueType)
            {
                return new Action<object, object>((source, target) =>
                {
                    var value = CreatePropertyGetter(property)(source);
                    CreatePropertySetter(property)(target, value);
                });
            }
            else
            {
                var expSource = Expression.Parameter(typeof(object), "source");
                var expTarget = Expression.Parameter(typeof(object), "target");

                var expPropertySource = Expression.Property(Expression.Convert(expSource, property.ReflectedType), property);

                var main = Expression.Call(Expression.Convert(expTarget, property.ReflectedType), property.GetSetMethod(true), expPropertySource);
                return (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, expSource, expTarget).Compile();
            }
        }

        public delegate void RefSetterDelegate(ref object target, object value);

        static Type[] ParamTypes = new Type[] { typeof(object).MakeByRefType(), typeof(object) };

        public static RefSetterDelegate CreateSetMethod(MemberInfo memberInfo)
        {
            Type ParamType;
            if (memberInfo is PropertyInfo)
                ParamType = ((PropertyInfo)memberInfo).PropertyType;
            else if (memberInfo is FieldInfo)
                ParamType = ((FieldInfo)memberInfo).FieldType;
            else
                throw new Exception("Can only create set methods for properties and fields.");

            DynamicMethod setter = new DynamicMethod(
                "",
                typeof(void),
                ParamTypes,
                memberInfo.ReflectedType.Module,
                true);
            ILGenerator generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldind_Ref);

            if (memberInfo.ReflectedType.IsValueType)
            {
                generator.DeclareLocal(memberInfo.ReflectedType.MakeByRefType());
                generator.Emit(OpCodes.Unbox, memberInfo.ReflectedType);
                generator.Emit(OpCodes.Stloc_0);
                generator.Emit(OpCodes.Ldloc_0);
            }

            generator.Emit(OpCodes.Ldarg_1);
            if (ParamType.IsValueType)
                generator.Emit(OpCodes.Unbox_Any, ParamType);

            if (memberInfo is PropertyInfo)
                generator.Emit(OpCodes.Callvirt, ((PropertyInfo)memberInfo).GetSetMethod());
            else if (memberInfo is FieldInfo)
                generator.Emit(OpCodes.Stfld, (FieldInfo)memberInfo);

            if (memberInfo.ReflectedType.IsValueType)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ldobj, memberInfo.ReflectedType);
                generator.Emit(OpCodes.Box, memberInfo.ReflectedType);
                generator.Emit(OpCodes.Stind_Ref);
            }
            generator.Emit(OpCodes.Ret);

            return (RefSetterDelegate)setter.CreateDelegate(typeof(RefSetterDelegate));
        }
    }
}
