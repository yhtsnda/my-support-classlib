using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Utility
{
    public static class FormatUtil
    {
        public const int ALIGN_RIGHT = 0;
        public const int ALIGN_LEFT = 1;
        private static char defaultSplitChar = ' ';
        private static string[] timeFormat = new String[] { "d ", "h ", "m ", "s ", "ms" };

        public static String Format(String s, int fillLength)
        {
            return Format(s, fillLength, defaultSplitChar, ALIGN_LEFT);
        }

        public static String Format(int i, int fillLength)
        {
            return Format(i.ToString(), fillLength, defaultSplitChar, ALIGN_RIGHT);
        }

        public static String Format(long l, int fillLength)
        {
            return Format(l.ToString(), fillLength, defaultSplitChar, ALIGN_RIGHT);
        }

        public static String Format(String s, int fillLength, char fillChar, int align)
        {
            if (s == null)
            {
                s = "";
            }
            else
            {
                s = s.Trim();
            }
            int charLen = fillLength - s.Length;
            if (charLen > 0)
            {
                char[] fills = new char[charLen];
                for (int i = 0; i < charLen; i++)
                {
                    fills[i] = fillChar;
                }
                StringBuilder str = new StringBuilder(s);
                switch (align)
                {
                    case ALIGN_RIGHT:
                        str.Insert(0, fills);
                        break;
                    case ALIGN_LEFT:
                        str.Append(fills);
                        break;
                    default:
                        str.Append(fills);
                        break;
                }
                return str.ToString();
            }
            else
            {
                return s;
            }
        }

        public static string FormatTime(long millis, int precision)
        {
            long[] la = new long[5];
            la[0] = (millis / 86400000);// days
            la[1] = (millis / 3600000) % 24;// hours
            la[2] = (millis / 60000) % 60;// minutes
            la[3] = (millis / 1000) % 60;// seconds
            la[4] = (millis % 1000);// ms

            int index = 0;
            for (int i = 0; i < la.Length; i++)
            {
                if (la[i] != 0)
                {
                    index = i;
                    break;
                }
            }

            StringBuilder buf = new StringBuilder();
            int validLength = la.Length - index;
            for (int i = 0; (i < validLength && i < precision); i++)
            {
                buf.Append(la[index]).Append(timeFormat[index]);
                index++;
            }
            return buf.ToString();
        }
    }
}
