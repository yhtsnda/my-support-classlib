using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 数据表名属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableNameAttribute : Attribute
    {
        private string mSchemaName = String.Empty;
        private bool mContainSchema = false;
        private string mTable = String.Empty;

        public string SchemaName { get { return mSchemaName; } set { mSchemaName = value; } }
        public bool ContainSchema { get { return mContainSchema; } set { mContainSchema = value; } }
        public string Table { get { return mTable; } }

        public TableNameAttribute(string name)
        {
            this.mTable = name;
        }
    }
}
