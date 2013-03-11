using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Projects.Framework
{
    public class DefaultPropertyAccessor : IPropertyAccessor
    {
        Type entityType;
        Dictionary<string, IGetter> getters;
        Dictionary<string, ISetter> setters;

        Func<object, object[]> getDatasHandler;
        Action<object, object[]> setDatasHandler;

        Func<object, IDictionary> dictionaryHandler;


        public DefaultPropertyAccessor(Type entityType)
        {
            this.entityType = entityType;

            var properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            CreateGetters(properties.Where(o => o.CanRead));
            CreateSetters(properties.Where(o => o.CanWrite));

            var repositoryProperties = properties.Where(o => o.CanRead && o.CanWrite).ToList();

            getDatasHandler = ReflectionHelper.CreateGetDatasHandler(entityType, repositoryProperties);
            setDatasHandler = ReflectionHelper.CreateSetDatasHandler(entityType, repositoryProperties);

            CreateToDictionary(entityType);
        }

        public Type EntityType
        {
            get { return entityType; }
        }

        public IGetter GetGetter(string propertyName)
        {
            return getters.TryGetValue(propertyName);
        }

        public ISetter GetSetter(string propertyName)
        {
            return setters.TryGetValue(propertyName);
        }

        public Func<object, object[]> GetDatasHandler
        {
            get { return getDatasHandler; }
        }

        public Action<object, object[]> SetDatasHandler
        {
            get { return setDatasHandler; }
        }

        public void MergeData(object source, object target)
        {
            var values = getDatasHandler(source);
            setDatasHandler(target, values);
        }

        void CreateToDictionary(Type entityType)
        {
            var fields = entityType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var param = Expression.Parameter(typeof(object), "target");

            var target = Expression.Variable(entityType, "entity");
            var dic = Expression.Variable(typeof(Hashtable), "dic");
            List<Expression> blocks = new List<Expression>();

            //var entity = (type)target;
            blocks.Add(Expression.Assign(target, Expression.Convert(param, entityType)));
            //dic = new Hashtable(int);
            blocks.Add(Expression.Assign(dic, Expression.New(typeof(Hashtable).GetConstructor(new Type[] { typeof(int) }), Expression.Constant(fields.Length))));

            MethodInfo addMethod = typeof(Hashtable).GetMethod("Add");
            foreach (var field in fields)
            {
                blocks.Add(Expression.Call(dic, addMethod,
                    Expression.Convert(Expression.Constant(field.Name), typeof(object)),
                    Expression.Convert(Expression.Field(target, field), typeof(object)))
                    );
            }
            LabelTarget returnTarget = Expression.Label(typeof(Hashtable));
            var returnExpr = Expression.Return(returnTarget, dic);

            blocks.Add(returnExpr);
            blocks.Add(Expression.Label(returnTarget, Expression.Constant(new Hashtable())));

            var main = Expression.Block(new ParameterExpression[] { target, dic }, blocks);
            dictionaryHandler = (Func<object, IDictionary>)Expression.Lambda(typeof(Func<object, IDictionary>), main, param).Compile();
        }

        void CreateGetters(IEnumerable<PropertyInfo> properties)
        {
            getters = new Dictionary<string, IGetter>();
            foreach (var property in properties)
                getters.Add(property.Name, new DefaultGetter(property));
        }

        void CreateSetters(IEnumerable<PropertyInfo> properties)
        {
            setters = new Dictionary<string, ISetter>();
            foreach (var property in properties)
                setters.Add(property.Name, new DefaultSetter(property));
        }

        public object CreateInstance()
        {
            return Projects.Tool.Reflection.FastActivator.Create(entityType);
        }

        public System.Collections.IDictionary ToDictionary(object target)
        {
            return dictionaryHandler(target);
        }
    }
}
