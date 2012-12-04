using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Shards
{
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
    }
}
