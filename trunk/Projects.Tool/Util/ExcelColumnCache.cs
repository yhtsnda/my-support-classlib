using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Projects.Tool.Attributes;

namespace Projects.Tool.Util
{
    internal class ExcelColumnCache
    {
        protected static Dictionary<string, string[]> mColumns;
        protected static ExcelColumnCache mCache = null;

        public static ExcelColumnCache Instance
        {
            get
            {
                if (mCache == null)
                    mCache = new ExcelColumnCache();
                return mCache;
            }
        }

        private ExcelColumnCache()
        {
            if (mColumns == null)
                mColumns = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// 将Excel列写入到缓存中
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="columns">列名</param>
        public void Add(string key, string[] columns)
        {
            Add(key, columns, false);
        }

        /// <summary>
        /// 获取指定类型在缓存中的列
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public string[] Get(Type type)
        {
            string key = type.FullName;
            if (mColumns.ContainsKey(key))
                return mColumns[key];
            else
            {
                var columns = GetColumns(type);
                Add(key, columns, true);
                return columns;
            }
        }

        /// <summary>
        /// 将Excel列写入到缓存中
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="columns">列名</param>
        /// <param name="forceUpdate">是否强制替换</param>
        protected void Add(string key, string[] columns, bool forceUpdate = false)
        {
            if (!mColumns.ContainsKey(key))
                mColumns.Add(key, columns);
            else
            {
                //需要强制更新
                if (forceUpdate)
                {
                    mColumns[key] = columns;
                }
            }
        }

        /// <summary>
        /// 从给定的类型中获取特定的特性,以作为数据列的标题
        /// </summary>
        /// <returns>字段信息</returns>
        internal virtual string[] GetColumns(Type type)
        {
            IList<string> columns = new List<string>();
            var properties = type.GetProperties(BindingFlags.Public);
            foreach (var pro in properties)
            {
                //如果可以找到ExcelColumnAttribute属性
                var objs = pro.GetCustomAttributes(typeof(ExcelCaptionAttribute), false);
                if (objs.Any())
                {
                    var attr = objs[0] as ExcelCaptionAttribute;
                    columns.Add(attr.Caption);
                }
            }
            return columns.ToArray();
        }

        /// <summary>
        /// 获取标题
        /// </summary>
        /// <param name="type">数据源的类型</param>
        /// <returns>标题字段</returns>
        internal virtual string GetHeader(Type type)
        {
            string header = String.Empty;
            var objs = type.GetCustomAttributes(typeof(ExcelCaptionAttribute), false);
            if (objs.Any())
            {
                var attr = objs[0] as ExcelCaptionAttribute;
                if (attr.TitleStyle == ExcelHeaderSuffix.Static)
                    header = attr.Caption;
                if(attr.TitleStyle == ExcelHeaderSuffix.RandomNumber)
                {
                    Random rnd = new Random();
                    header = String.Format("{0}_{1}", attr.Caption, rnd.Next(1000,99999999));
                }
                if (attr.TitleStyle == ExcelHeaderSuffix.DateTime)
                {
                    if (String.IsNullOrEmpty(attr.TitleFormat))
                        header = String.Format("{0}_{1}", attr.Caption, DateTime.Now.ToString("yyyyMMdd"));
                    else
                        header = String.Format("{0}_{1}", attr.Caption, DateTime.Now.ToString(attr.TitleFormat));
                }
                return header;
            }
            return String.Format("导出的数据_{1}", DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
