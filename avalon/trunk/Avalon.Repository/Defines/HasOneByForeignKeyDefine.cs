using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public class HasOneByForeignKeyDefine
    {
        public HasOneByForeignKeyDefine()
        {
            ShardParams = ShardParams.Empty;
        }

        internal ShardParams ShardParams
        {
            get;
            private set;
        }

        internal object Id { get; private set; }

        public HasOneByForeignKeyDefine Shard(long param)
        {
            ShardParams = ShardParams.Form(param);
            return this;
        }

        public HasOneByForeignKeyDefine Shard(long param1, long param2)
        {
            ShardParams = ShardParams.Form(param1, param2);
            return this;
        }

        public HasOneByForeignKeyDefine ForeignKey(object id)
        {
            Id = id;
            return this;
        }
    }
}
