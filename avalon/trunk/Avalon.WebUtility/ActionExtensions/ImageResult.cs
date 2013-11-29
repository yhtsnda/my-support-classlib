using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;
using System.IO;
namespace Avalon.WebUtility
{
    public class ImageResult : ActionResult
    {
        public ImageResult()
        { }

        public ImageResult(Image image)
        {
            Image = image;
        }

        public ImageResult(Image image, ImageFormat format)
        {
            Image = image;
            ImageFormat = format;
        }

        /// <summary>
        ///
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// 指定图像的文件格式
        /// </summary>
        public ImageFormat ImageFormat { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (Image == null)
                throw new ArgumentNullException("Image");

            if (ImageFormat == null)
                throw new ArgumentNullException("ImageFormat");

            context.HttpContext.Response.Clear();

            if (ImageFormat.Equals(ImageFormat.Gif))
                context.HttpContext.Response.ContentType = "image/gif";
            else if (ImageFormat.Equals(ImageFormat.Jpeg))
                context.HttpContext.Response.ContentType = "image/jpeg";
            else if (ImageFormat.Equals(ImageFormat.Png))
                context.HttpContext.Response.ContentType = "image/png";
            else if (ImageFormat.Equals(ImageFormat.Bmp))
                context.HttpContext.Response.ContentType = "image/bmp";
            else if (ImageFormat.Equals(ImageFormat.Tiff))
                context.HttpContext.Response.ContentType = "image/tiff";
            else if (ImageFormat.Equals(ImageFormat.Icon))
                context.HttpContext.Response.ContentType = "image/vnd.microsoft.icon";
            else if (ImageFormat.Equals(ImageFormat.Wmf))
                context.HttpContext.Response.ContentType = "image/wmf";


            //add by liubin 2012 修复图片资源未被释放引起的 A generic error occurred in GDI+.
            MemoryStream ms = new MemoryStream();
            Image.Save(ms, ImageFormat);
            Image.Dispose();
            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.BinaryWrite(ms.ToArray());
            ms.Close();
      
        }
    }
}
