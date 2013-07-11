using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Projects.Framework.Web
{
    /// <summary>
    /// XML扩展
    /// </summary>
    public class XmlResult : ActionResult
    {
        public XmlResult()
        {
        }

        public XmlResult(object data)
        {
            Data = data;
        }

        public XmlResult(object data, Encoding encoding)
        {
            Data = data;
            ContentEncoding = encoding;
        }

        public object Data
        {
            get;
            set;
        }

        public Encoding ContentEncoding
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/xml";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlSerializer xs = new XmlSerializer(Data.GetType());
                    xs.Serialize(ms, Data); // 把数据序列化到内存流中
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        //读取流对象
                        context.HttpContext.Response.Output.Write(sr.ReadToEnd());
                    }
                }
            }
        }
    }
}
