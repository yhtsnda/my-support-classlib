using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class EmptyData : ICloneable
    {
        public static EmptyData Value = new EmptyData();

        public override bool Equals(object obj)
        {
            if (obj is EmptyData)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public object Clone()
        {
            return this;
        }
    }
}
