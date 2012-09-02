using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BuildingSiteCheck;

namespace BuildingSiteCheck.Mvc
{
    /// <summary>
    /// EXCEL结果
    /// </summary>
    public class ExcelResult<T> : ActionResult
    {
        public IList<T> Source { get; set; }
        public string FileName { get; set; }
        public string Header { get; set; }
        public string[] Columns { get; set; }

        public ExcelResult(IList<T> source, string fileName, string header, string[] columns)
        {
            Source = source;
            FileName = fileName;
            Header = header;
            Columns = columns;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            //将数据源表中的
            var ms = ExportHelper.Export(Source, Header, Columns);

            var response = context.HttpContext.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.ContentEncoding = Encoding.UTF8;
            response.Charset = "";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, Encoding.UTF8));
            response.BinaryWrite(ms.GetBuffer());
            response.End();
        }
    }
}