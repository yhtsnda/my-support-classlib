using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal class LinqDatabase
    {
        static LinqDatabase inner;

        Dictionary<Type, object> databases = new Dictionary<Type, object>();

        public static LinqDatabase Instance
        {
            get
            {
                if (inner == null)
                    inner = new LinqDatabase();
                return inner;
            }
        }

        public List<T> GetData<T>()
        {
            var item = databases.TryGetValue(typeof(T));
            if (item != null)
                return (List<T>)item;

            var newItem = new List<T>();
            databases.Add(typeof(T), newItem);
            return newItem;
        }
    }
}
