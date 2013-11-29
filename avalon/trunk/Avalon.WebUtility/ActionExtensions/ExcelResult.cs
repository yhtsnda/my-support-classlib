using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using Avalon.Utility;


namespace Avalon.WebUtility
{
    public class ExcelResult<T> : ActionResult where T : new()
    {
        public ExcelResult(IList<T> entity, string fileName)
        {
            this.Entity = entity;
            this.Extra = null;
            this.FileName = fileName;
        }

        public ExcelResult(IList<T> entity, IList<Dictionary<string, string>> extra) 
        {
            this.Entity = entity;
            this.Extra = extra;
            this.FileName = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_"
            + DateTime.Now.Hour + "_" + DateTime.Now.Minute;
        }
        public ExcelResult(IList<T> entity)
        {
            this.Extra = null;
            this.Entity = entity;
            this.FileName = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_"
                + DateTime.Now.Hour + "_" + DateTime.Now.Minute;
        }

        public IList<T> Entity
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }
        public IList<Dictionary<string, string>> Extra
        {
            get;
            set;
        }
        private static readonly string _fileType = ".xls";

        public override void ExecuteResult(ControllerContext context)
        {
            if (Entity == null)
            {
                new EmptyResult().ExecuteResult(context);
                return;
            }
            SetResponse(context.HttpContext);
        }
        void SetResponse(HttpContextBase httpContext)
        {
            var builder = new StringBuilder();
            System.Reflection.PropertyInfo[] myPropertyInfo = Entity.First().GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            for (var i = 0; i < myPropertyInfo.Count(); i++)
            {
                
                string headName = ((System.ComponentModel.DataAnnotations.DisplayAttribute)(myPropertyInfo[i].GetCustomAttributes(true).FirstOrDefault())).GetOrDefault(o => o.Name);
                builder.Append(headName);
                if (Extra == null || Extra.Count == 0)
                    builder.Append(i == myPropertyInfo.Count() - 1 ? "\n" : "\t");
                else
                    builder.Append("\t");
            }
            if (Extra != null && Extra.Count > 0)
            {
                for (var i = 0; i < Extra[0].Count; i++)
                {
                    builder.Append(Extra[0].ElementAtOrDefault(i).Key);
                    builder.Append(i == Extra[0].Count - 1 ? "\n" : "\t");
                }
            }

            for (var j = 0; j < Entity.Count; j++)
            {
                for (var i = 0; i < myPropertyInfo.Count(); i++)
                {
                    string sign = string.Empty;
                    if (Extra == null || Extra.Count == 0)
                        sign = i == myPropertyInfo.Count() - 1 ? "\n" : "\t";
                    else
                        sign = "\t";
                    string ss = string.Format("{0}", myPropertyInfo[i].GetValue(Entity[j], null)).Replace("\n", "");
                    if (ss.IsNullOrEmpty())
                    {
                        builder.Append(sign);
                    }
                    else
                    {
                        builder.Append(ss + sign);
                    }
                }
                if (Extra != null && Extra.Count > 0)
                {
                    for (var i = 0; i < Extra[j].Count; i++)
                    {
                        builder.Append(Extra[j].ElementAtOrDefault(i).Value);
                        builder.Append(i == Extra[j].Count - 1 ? "\n" : "\t");
                    }
                }

            }
            var encoding = new UTF8Encoding(false);
            byte[] bytes = encoding.GetBytes(builder.ToString());
            httpContext.Response.Clear();
            // 设置编码和附件格式
            httpContext.Response.Buffer = true;
            httpContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            httpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(FileName, encoding) + _fileType);
            httpContext.Response.Write(builder);
            httpContext.Response.End();
        }
    }
}
