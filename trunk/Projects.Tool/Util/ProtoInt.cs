using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 采用 Big-Endian 的方式存储
    /// </summary>
    public static class ProtoInt
    {
        public static int Read(Stream stream)
        {
            int value = 0;
            int offset = 0;
            int v = 0;
            do
            {
                v = stream.ReadByte();
                value += (v & 127) << (offset * 7);
                offset += 1;
            }
            while (v > 127);
            return value;
        }

        /// <summary>
        /// 将value的内容写入流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">要写入流的内容</param>
        /// <returns>占的字节数</returns>
        public static int Write(Stream stream, int value)
        {
            if (value < 0)
                throw new ArgumentNullException("参数不能小于0");

            //如果小等于127（111 1111），直接写入
            if (value <= 127)
            {
                stream.WriteByte((byte)(value));
                return 1;
            }

            int v = value;

            stream.WriteByte((byte)((v & 127) | 128));
            v = v >> 7;
            int count = 1;
            while (v > 0)
            {
                if (v > 127)
                    stream.WriteByte((byte)((v & 127) | 128));
                else
                    stream.WriteByte((byte)v);
                v = v >> 7;
                count++;
            }
            return count;
        }
    }
}
