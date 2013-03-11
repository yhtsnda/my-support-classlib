using Projects.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class PagingResultProxyProvider : IProxyProvider
    {
        public bool IsMatch(Type type)
        {
            if (type == null)
                return false;

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(PagingResult<>) || IsMatch(type.BaseType);
        }

        public object Proxy(object entity, Func<object, object> subProxyHandler)
        {
            var elementType = GetElementType(entity.GetType());
            var inner = (IInnerPagingResult)Projects.Tool.Reflection.FastActivator.Create(typeof(InnerPagingResult<>).MakeGenericType(elementType));
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
            var inner = (IInnerPagingResult)Projects.Tool.Reflection.FastActivator.Create(typeof(InnerPagingResult<>).MakeGenericType(elementType));
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
            var inner = (IInnerPagingResult)Projects.Tool.Reflection.FastActivator.Create(typeof(InnerPagingResult<>).MakeGenericType(elementType));
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
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(PagingResult<>))
                return type.GetGenericArguments()[0];

            return GetElementType(type.BaseType);
        }

        interface IInnerPagingResult
        {
            void Wrap(object data);

            void SetItem(int index, object item);

            IEnumerable ToList();
        }

        class InnerPagingResult<T> : IInnerPagingResult
        {
            PagingResult<T> data;

            public void Wrap(object data)
            {
                this.data = (PagingResult<T>)data;
            }

            public void SetItem(int index, object item)
            {
                data.Items[index] = (T)item;
            }

            public IEnumerable ToList()
            {
                return data.Items.ToList();
            }
        }
    }
}
