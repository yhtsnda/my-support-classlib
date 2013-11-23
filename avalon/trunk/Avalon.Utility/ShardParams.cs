using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 分区分库的依赖参数，目前仅支持2个参数。非long类型的可以先转为long类型，如DateTime.
    /// </summary>
    public struct ShardParams
    {
        public static ShardParams Empty = new ShardParams();
        public ShardParams(long param1, long param2)
        {
            Param1 = param1;
            Param2 = param2;
        }

        public long Param1;
        public long Param2;

        public static ShardParams Form(long param1, long param2)
        {
            return new ShardParams(param1, param2);
        }

        public static ShardParams Form(long param)
        {
            return new ShardParams(param, 0);
        }

        public override string ToString()
        {
            return Param1 + "," + Param2;
        }

        /// <summary>
        ///  HHB 2013-4-26 update for redis hash parse valuetype
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ShardParams Parse(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return Empty;

            string[] vs = value.Split(',');
            if (vs.Length != 2)
                return Empty;
            long p1, p2;
            if (Int64.TryParse(vs[0], out p1) && Int64.TryParse(vs[1], out p2))
                return new ShardParams(p1, p2);

            return Empty;
        }
    }
}
