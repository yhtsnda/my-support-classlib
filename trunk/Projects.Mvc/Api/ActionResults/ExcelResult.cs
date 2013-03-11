using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Projects.Tool.Util;

namespace System.Web.Mvc
{
    /// <summary>
    /// EXCEL结果
    /// </summary>
    public class ExcelResult : ActionResult
    {
        String FileName;
        MemoryStream ExcelStream;
        ExcelBuilder Builder;

        /// <summary>
        /// Excel格式的ActionResult
        /// </summary>
        /// <param name="fileName">导出Excel的文件名</param>
        /// <param name="builder">内置的ExcelBuilder对象</param>
        public ExcelResult(string fileName, ExcelBuilder builder)
        {
            ExcelStream = builder.ExportAll();
            FileName = fileName;
        }

        /// <summary>
        /// 生成一个文件流
        /// </summary>
        /// <param name="fileName">导出Excel的文件名</param>
        /// <param name="stream">Excel文件流,可以通过ExcelBuilder构建也可以自定义构建</param>
        public ExcelResult(string fileName, MemoryStream stream)
        {
            FileName = fileName;
            ExcelStream = stream;
        }

        /// <summary>
        /// 通过内置的Builder构建Excel结果,需要调用AddSource方法添加数据源
        /// </summary>
        /// <param name="fileName">导出Excel的文件名</param>
        public ExcelResult(string fileName)
        {
            Builder = new ExcelBuilder();
            FileName = fileName;
        }

        /// <summary>
        /// 向导出流中添加源
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="header">标题</param>
        /// <param name="columns">列名集合</param>
        public void AddSource<T>(IList<T> source, string header, string[] columns) where T : class, new()
        {
            if (Builder == null)
                throw new NullReferenceException("如需要调用AddSource方法,请通过ExcelResult(string)初始化对象");
            Builder.AddExportSet<T>(source, header, columns);
        }

        /// <summary>
        /// 向导出流中添加源
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="source">数据源</param>
        public void AddSource<T>(IList<T> source) where T : class ,new()
        {
            if (Builder == null)
                throw new NullReferenceException("如需要调用AddSource方法,请通过ExcelResult(string)初始化对象");
            Builder.AddExportSet<T>(source);
        }

        /// <summary>
        /// 向导出流中添加源
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="header">标题</param>
        public void AddSource<T>(IList<T> source, string header) where T : class ,new()
        {
            if (Builder == null)
                throw new NullReferenceException("如需要调用AddSource方法,请通过ExcelResult(string)初始化对象");
            Builder.AddExportSet<T>(source, header);
        }

        /// <summary>
        /// 生成Excel导出的
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.ContentEncoding = Encoding.UTF8;
            response.Charset = "";
            response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(FileName, Encoding.UTF8));

            if (ExcelStream != null)
            {
                response.BinaryWrite(ExcelStream.GetBuffer());
            }
            else
            {
                //将数据源表中的
                var ms = Builder.ExportAll();
                response.BinaryWrite(ms.GetBuffer());
            }
            response.End();
        }
    }

}