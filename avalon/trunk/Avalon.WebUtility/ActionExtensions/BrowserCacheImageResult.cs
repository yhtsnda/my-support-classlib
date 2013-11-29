using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;

namespace Avalon.WebUtility
{
    public class BrowserCacheImageResult : BrowserCacheActionResult
    {
        /// <summary>
        /// 指定图像的文件格式
        /// </summary>
        private ImageFormat imageFormat;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastModifyFactory"></param>
        /// <param name="eTag"></param>
        /// <param name="valueFactory"></param>
        /// <param name="contentType"></param>
        public BrowserCacheImageResult(DateTime lastModified, string eTag, ImageFormat imageFormat, Func<Image> imageFactory)
            : base(lastModified, eTag, imageFactory)
        {
            this.imageFormat = imageFormat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        private string GetContentType(ImageFormat imageFormat)
        {
            if (imageFormat.Equals(ImageFormat.Gif))
                return "image/gif";
            else if (imageFormat.Equals(ImageFormat.Jpeg))
                return "image/jpeg";
            else if (imageFormat.Equals(ImageFormat.Png))
                return "image/png";
            else if (imageFormat.Equals(ImageFormat.Bmp))
                return "image/bmp";
            else if (imageFormat.Equals(ImageFormat.Tiff))
                return "image/tiff";
            else if (imageFormat.Equals(ImageFormat.Icon))
                return "image/vnd.microsoft.icon";
            else if (imageFormat.Equals(ImageFormat.Wmf))
                return "image/wmf";
            else
                return string.Empty;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (IsInBrowserCache(context.HttpContext))
                return;

            var response = context.HttpContext.Response;
            response.ContentType = GetContentType(this.imageFormat);
            using (var stream = new System.IO.MemoryStream())
            {
                var image = (Image)this._valueFactory();
                image.Save(stream, imageFormat);
                image.Dispose();
                response.BinaryWrite(stream.ToArray());
            }

            SendOuptToClient(context.HttpContext);
        }
    }
}
