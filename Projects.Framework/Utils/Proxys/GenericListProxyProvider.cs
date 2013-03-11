using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class GenericListProxyProvider : IProxyProvider
    {
        public bool IsMatch(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>) || type.GetInterface(typeof(IList<>).FullName) != null;
        }

        public object Proxy(object entity, Func<object, object> subProxyHandler)
        {
            var elementType = GetElementType(entity.GetType());
            var inner = (IInnerList)Projects.Tool.Reflection.FastActivator.Create(typeof(InnerList<>).MakeGenericType(elementType));
            inner.Wrap(entity);

            int index = 0;
            foreach (var item in inner.ToList())
            {
                inner.SetItem(index, subProxyHandler(item));
                index++;
            }

            return entity;
        }

        public object Poco(object entity, Func<object, object> subPocoHandler)
        {
            var elementType = GetElementType(entity.GetType());
            var inner = (IInnerList)Projects.Tool.Reflection.FastActivator.Create(typeof(InnerList<>).MakeGenericType(elementType));
            inner.Wrap(entity);

            int index = 0;
            foreach (var item in inner.ToList())
            {
                inner.SetItem(index, subPocoHandler(item));
                index++;
            }

            return entity;
        }

        public void Fetch(object entity, Action<object> subFetchHandler)
        {
            var elementType = GetElementType(entity.GetType());
            var inner = (IInnerList)Projects.Tool.Reflection.FastActivator.Create(typeof(InnerList<>).MakeGenericType(elementType));
            inner.Wrap(entity);

            int index = 0;
            foreach (var item in inner.ToList())
            {
                subFetchHandler(item);
                index++;
            }
        }

        Type GetElementType(Type type)
        {
            if (type.GetGenericTypeDefinition() == typeof(IList<>))
                return type.GetGenericArguments()[0];
            var bi = type.GetInterface(typeof(IList<>).FullName);
            return bi.GetGenericArguments()[0];
        }

        interface IInnerList
        {
            void Wrap(object entity);

            void SetItem(int index, object item);

            IEnumerable ToList();
        }

        class InnerList<T> : IInnerList
        {
            IList<T> data;

            public void Wrap(object data)
            {
                this.data = (IList<T>)data;
            }

            public void SetItem(int index, object item)
            {
                data[index] = (T)item;
            }

            public IEnumerable ToList()
            {
                return data.ToList();
            }
        }
    }
}
