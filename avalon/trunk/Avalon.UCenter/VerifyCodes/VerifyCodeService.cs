using Avalon.Framework;
using Avalon.Profiler;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.UCenter
{
    public class VerifyCodeService : IService
    {
        const string VerifyCodeSessionIdFormat = "vc:{0}";
        //string strCode = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        string strCode = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefhijkmnpqrstuvwxy";
        static Random rnd = new Random(Environment.TickCount);
        IVerifyCodeRepository verifyCodeRepository;

        private CacheDomain<IpRegionAccumlator, string> _cacheIpRegionAccumlator;

        public VerifyCodeService(IVerifyCodeRepository verifyCodeRepository)
        {
            this.verifyCodeRepository = verifyCodeRepository;

            _cacheIpRegionAccumlator = CacheDomain.CreateSingleKey<IpRegionAccumlator, string>(
                    o => o.IpAddress,
                     GetVerifyCodeIpRegionAccumlatorInner,
                     null,
                     "verifycode:ip:num",
                     "cache:verifycode:ip:num:{0}");
        }

        /// <summary>
        /// 进行验证码的判断
        /// </summary>
        public bool ValidVerifyCode(string sessionId, string verifyCode)
        {
#if DEBUG
            if (HttpContext.Current != null && HttpContext.Current.Request["__skipverifycode__"] == "true")
                return true;
#endif

            return verifyCodeRepository.Valid(sessionId, verifyCode);
        }

        /// <summary>
        /// 获取新的验证码
        /// </summary>
        public Image GetVerifyCode(Avalon.OAuth.VerifyCodeType verifyCodeType, string ipAddress, out string sessionId)
        {
            sessionId = String.Format(VerifyCodeSessionIdFormat, Guid.NewGuid().ToString("N"));
            switch (verifyCodeType)
            {
                case OAuth.VerifyCodeType.Judge:
                    var ip = _cacheIpRegionAccumlator.GetItem(ipAddress);
                    var item = ip.RegionAccumlator;
                    item.Increase();
                    _cacheIpRegionAccumlator.SetItemToCache(ip);
                    if (item.SumValue > UserCenterConfig.VerifyCodeAmount)
                        return ResetVerifyCode(sessionId);
                    else
                    {
                        verifyCodeRepository.SaveVerifyCode(sessionId, string.Empty);
                        return null;
                    }
                case OAuth.VerifyCodeType.No:
                    verifyCodeRepository.SaveVerifyCode(sessionId, string.Empty);
                    return null;
                default:
                    return ResetVerifyCode(sessionId);
            }
        }

        /// <summary>
        /// 重新产生验证码
        /// </summary>
        public Image ResetVerifyCode(string sessionId)
        {
            var verifyCode = GenerateVerifyCode(4);
            verifyCodeRepository.SaveVerifyCode(sessionId, verifyCode);
            return GenerateImage(verifyCode);
        }

        Bitmap GenerateImage(string verifyCode)
        {
            Arguments.NotNullOrWhiteSpace(verifyCode, "verifyCode");

            var len = verifyCode.Length;
            int width = 27 * len;
            int height = 50;
            Bitmap image = new Bitmap(width, height);

            int nRed, nGreen, nBlue;
            Random rnd = new Random(Environment.TickCount);
            nRed = rnd.Next(128) + 128;
            nGreen = rnd.Next(128) + 128;
            nBlue = rnd.Next(128) + 128;

            Graphics g = Graphics.FromImage(image);
            g.FillRectangle(new SolidBrush(Color.FromArgb(nRed, nGreen, nBlue)), 0, 0, width, height);

            //混淆线
            int lines = 3;
            using (Pen pen = new Pen(Color.FromArgb(nRed - 60, nGreen - 60, nBlue - 40), 2))
            {
                for (int i = 0; i < lines; i++)
                {
                    g.DrawLine(pen, rnd.Next(width), rnd.Next(height), rnd.Next(width), rnd.Next(height));
                }
            }

            int px = 5;
            int index = 0;
            foreach (var c in verifyCode)
            {
                int y = rnd.Next(6) + 2;
                var s = c.ToString();
                using (Font font = new Font("Courier New", GetNextSize(rnd, index, len, width, px), FontStyle.Bold))
                {
                    g.DrawString(s, font, new SolidBrush(Color.FromArgb(nRed - 60 + y * 3, nGreen - 60 + y * 3, nBlue - 40 + y * 3)), px, y);
                    g.ResetTransform();
                    px += (int)(g.MeasureString(s, font).Width / 1.5);
                }
                index++;
            }

            return image;
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        int GetNextSize(Random rnd, int index, int count, double width, double px)
        {
            double p = width / (px * count / index);
            double min = 3 * p;
            double size = (p == 0 ? 20 : 16 * p);
            return 16 + rnd.Next((int)min, (int)(min + size));
        }

        string GenerateVerifyCode(int length)
        {
            StringBuilder verity = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                char c = strCode[rnd.Next(strCode.Length)];
                verity.Append(c);
            }
            return verity.ToString();
        }

        IpRegionAccumlator GetVerifyCodeIpRegionAccumlatorInner(string ipAddress)
        {
            return new IpRegionAccumlator(ipAddress);
        }

        class IpRegionAccumlator : IQueryTimestamp
        {
            public IpRegionAccumlator(string ipAddress)
            {
                Timestamp = NetworkTime.Now.Ticks;
                IpAddress = ipAddress;
                RegionAccumlator = new RegionAccumlator(UserCenterConfig.VerifyCodeDuration * 1000 / 6, 6);
            }

            public long Timestamp { get; set; }

            public string IpAddress { get; set; }

            public RegionAccumlator RegionAccumlator { get; set; }
        }
    }
}
