using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CharTypes
    {
        private static bool[] hexFlags = new bool[256];
        private static bool[] identifierFlags = new bool[256];
        private static bool[] whitespaceFlags = new bool[256];

        static CharTypes()
        {
            for (char c = (char)0; c < hexFlags.Length; ++c)
            {
                if (c >= 'A' && c <= 'F')
                    hexFlags[c] = true;
                else if (c >= 'a' && c <= 'f')
                    hexFlags[c] = true;
                else if (c >= '0' && c <= '9')
                    hexFlags[c] = true;
            }

            for (char c = (char)0; c < identifierFlags.Length; ++c)
            {
                if (c >= 'A' && c <= 'Z')
                    identifierFlags[c] = true;
                else if (c >= 'a' && c <= 'z')
                    identifierFlags[c] = true;
                else if (c >= '0' && c <= '9')
                    identifierFlags[c] = true;
            }
            identifierFlags['_'] = true;
            identifierFlags['$'] = true;
            whitespaceFlags[' '] = true;
            whitespaceFlags['\n'] = true;
            whitespaceFlags['\r'] = true;
            whitespaceFlags['\t'] = true;
            whitespaceFlags['\f'] = true;
            whitespaceFlags['\b'] = true;
        }

        public static bool IsWhitespace(char c)
        {
            return c <= whitespaceFlags.Length && whitespaceFlags[c];
        }

        public static bool IsIdentifierChar(char c)
        {
            return c > identifierFlags.Length || identifierFlags[c];
        }

        public static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool IsHex(char c)
        {
            return c < 256 && hexFlags[c];
        }
    }
}
