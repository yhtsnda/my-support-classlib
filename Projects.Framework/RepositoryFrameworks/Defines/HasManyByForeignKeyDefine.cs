using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;

namespace Projects.Framework
{
    public class HasManyByForeignKeyDefine
    {
        public HasManyByForeignKeyDefine()
        {
            ShardParams = ShardParams.Empty;
        }

        internal ShardParams ShardParams
        {
            get;
            private set;
        }

        internal IEnumerable Ids { get; private set; }

        public HasManyByForeignKeyDefine Shard(long param)
        {
            ShardParams = ShardParams.Form(param);
            return this;
        }

        public HasManyByForeignKeyDefine Shard(long param1, long param2)
        {
            ShardParams = ShardParams.Form(param1, param2);
            return this;
        }

        public HasManyByForeignKeyDefine ForeignKey(IEnumerable ids)
        {
            Ids = ids;
            return this;
        }
    }
}
