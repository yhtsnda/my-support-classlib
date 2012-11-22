using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Attributes
{
    /// <summary>
    /// 用于标记属性在导出为Excel是显示的字段的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExcelCaptionAttribute : Attribute
    {
        public string Caption { get; protected set; }

        public string TitleFormat { get; set; }

        public ExcelHeaderSuffix TitleStyle { get; set; }

        /// <summary>
        /// 用于标记属性在导出为Excel是显示的字段的属性
        /// </summary>
        /// <param name="columnName">属性标题</param>
        public ExcelCaptionAttribute(string caption)
        {
            this.Caption = caption;
        }
    }

    public enum ExcelHeaderSuffix
    {
        Static,
        DateTime,
        RandomNumber
    }
}
