using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Access.MongoAccess
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CollectionNameAttribute : Attribute
    {
        public string Name { get; set; }
        public string Database { get; set; }

        public CollectionNameAttribute(){}
        public CollectionNameAttribute(string name) : this(null, name){}
        public CollectionNameAttribute(string database, string name)
        {
            Database = database;
            Name = name;
        }
    }
}
