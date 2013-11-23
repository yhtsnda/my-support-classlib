using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Avalon.Utility
{
    public class ImageHelper
    {
        static byte[] iconBitHeader =
		{ 
			(byte)0,(byte)0,	//idreserved, int16
			(byte)1,(byte)0,	//idtype,
			(byte)1,(byte)0,	//idcount
		};

        static void WriteDirEntry(Icon icon, BinaryWriter bw)
        {
            Bitmap bm = icon.ToBitmap();
            WriteDirEntry(bm, bw);
        }

        static void WriteDirEntry(Bitmap bm, BinaryWriter bw)
        {
            //BitmapData bmData1 = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);	//每个像素占3个字节（24位）

            int bpp1 = 24; //真彩色位图的bpp
            int bpp2 = 1;  //蒙版的bpp
            int stride1 = (bm.Width * bpp1 + 31) / 32 * 4;
            int stride2 = (bm.Width * bpp2 + 31) / 32 * 4;

            byte bwidth = (byte)bm.Width;
            byte bheight = (byte)bm.Height;
            byte bcolorcount = (byte)0;	//对于真彩图标32bpp来说，这个字段为0！
            byte breserved = (byte)0;
            Int16 wplanes = 1;
            Int16 wbitcount = (Int16)bpp1;		//8bit per pixel, 16 colors!
            Int32 dwbytesInRes = 40 + (stride1 + stride2) * bm.Height;   //40是BitmapInfoHeader的尺寸
            Int32 dwImageOffset = 22;	//???紧跟着就是第一个图像的入口！就是下一个字节的文件地址 6bytes header+ 16bytes entry

            //解锁
            //bm.UnlockBits(bmData1);

            //写入
            bw.Write(bwidth);
            bw.Write(bheight);
            bw.Write(bcolorcount);
            bw.Write(breserved);
            bw.Write(wplanes);
            bw.Write(wbitcount);
            bw.Write(dwbytesInRes);
            bw.Write(dwImageOffset);

            WriteIconImage(bm, bw);
        }

        static void WriteIconImage(Bitmap bm, BinaryWriter bw)
        {

            //在ICON文件中使用的关键变量只用到：bisize, biwidth, biheight, biplanes, bibitcount, bisizeimage.几个,其他变量必须为0。
            //biheight变量的值为高度象素量的2倍。

            ////写入BitmapHeaderInfo结构
            Int32 biSize = 40;	//BitmapHeaderInfo结构的size
            Int32 biWidth = bm.Width;
            Int32 biHeight = bm.Height * 2;
            Int16 biPlanes = 1;
            Int16 biBitCount = 24;
            Int32 biCompression = 0;
            Int32 biSizeImage = 0;//固定为0
            Int32 biXPelsPerMeter = 0;
            Int32 biYPelsPerMeter = 0;
            Int32 biClrUsed = 0;
            Int32 biClrImportant = 0;

            bw.Write(biSize);
            bw.Write(biWidth);
            bw.Write(biHeight);
            bw.Write(biPlanes);
            bw.Write(biBitCount);
            bw.Write(biCompression);
            bw.Write(biSizeImage);
            bw.Write(biXPelsPerMeter);
            bw.Write(biYPelsPerMeter);
            bw.Write(biClrUsed);
            bw.Write(biClrImportant);

            //对于bpp=32的真彩色图标来说，没有RGBQUAD这个段！在xor中是实际色彩！！！
            //这里是RGBQUAD段-（真实色彩图标缺此段)

            //写入byte[] XOR Mask
            int stride = (bm.Width * 24 + 31) / 32 * 4;
            for (int j = bm.Height - 1; j >= 0; j--)
            {
                for (int index = 0; index < bm.Width; index++)
                {
                    global::System.Drawing.Color pixelColor = bm.GetPixel(index, j);
                    bw.Write(pixelColor.B);
                    bw.Write(pixelColor.G);
                    bw.Write(pixelColor.R);
                }
                //行尾补齐
                for (int index = bm.Width * 3; index < stride; index++)
                {
                    bw.Write((byte)0);
                }
            }
            //写入AND Mask ， 1 bpp
            stride = (1 * bm.Width + 31) / 32 * 4; //and mask 以DWORD对齐后的扫描行宽度
            byte[] line = new byte[stride];

            for (int j = bm.Height - 1; j >= 0; j--)
            {
                for (int i = 0; i < stride; i++)
                    line[i] = 0;

                //处理当前扫描行
                for (int i = 0; i < bm.Width; i++)
                {
                    global::System.Drawing.Color color = bm.GetPixel(i, j);
                    int colorsum = color.R + color.G + color.B;
                    if (colorsum == 0)
                    {
                        //在位图中是黑色说明该像素应该是透明的
                        line[i / 8] |= (byte)(1 << (7 - (i & 0x07))); //i&7: 相当于i%8
                    }
                }
                bw.Write(line, 0, stride);
            }
        }

        /// <summary>
        /// 把图片转换成icon
        /// </summary>
        /// <param name="image">要转换的图片</param>
        /// <param name="path">icon的保存路径，使用完整的路径</param>
        public static void ConvertToIcon(Image image, string path)
        {
            using (Bitmap bitmap = new Bitmap(image, 16, 16))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    g.DrawImage(image, 0, 0, 16, 16);
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        IntPtr intptr = bitmap.GetHicon();
                        Icon icon = Icon.FromHandle(intptr);
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            bw.Write(iconBitHeader);
                            WriteDirEntry(icon, bw);

                        }
                    }
                }
            }
        }
    }
}
