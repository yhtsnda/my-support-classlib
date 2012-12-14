using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 数据表字段名属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ColumnNameAttribute : Attribute
    {
        private string mColumn = String.Empty;
        private bool mIsAutoIdentity = false;

        public string Column { get { return mColumn; } }
        public bool IsAutoIdentity { get { return mIsAutoIdentity; } set { mIsAutoIdentity = value; } }

        public ColumnNameAttribute(string name)
        {
            this.mColumn = name;
        }
    }
}
