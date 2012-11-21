using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Projects.Tool.Util;

namespace Projects.Mvc
{
    /// <summary>
    /// EXCEL结果
    /// </summary>
    public class ExcelResult : ActionResult
    {
        ExcelBuilder Builder;
        String FileName;

        public ExcelResult(string fileName, ExcelBuilder builder)
        {
            Builder = builder;
            FileName = fileName;
        }

        public ExcelResult(string fileName)
        {
            Builder = new ExcelBuilder();
            FileName = fileName;
        }

        public void AddSource<T>(IList<T> source, string header, string[] columns) where T : class, new()
        {
            Builder.AddExportSet<T>(source, header, columns);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            //将数据源表中的
            var ms = Builder.ExportAll();
            var response = context.HttpContext.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.ContentEncoding = Encoding.UTF8;
            response.Charset = "";
            response.AppendHeader("Content-Disposition", 
                "attachment;filename=" + HttpUtility.UrlEncode(FileName, Encoding.UTF8));
            response.BinaryWrite(ms.GetBuffer());
            response.End();
        }
    }
}