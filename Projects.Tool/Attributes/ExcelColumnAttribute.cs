using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Attributes
{
    /// <summary>
    /// 用于标记属性在导出为Excel是显示的字段的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExcelColumnAttribute : Attribute
    {
        public string ColumnName { get; protected set; }

        /// <summary>
        /// 用于标记属性在导出为Excel是显示的字段的属性
        /// </summary>
        /// <param name="columnName">属性标题</param>
        public ExcelColumnAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }
    }
}
