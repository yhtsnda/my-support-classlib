using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Shards
{
    /// <summary>
    /// 分片的标识
    /// </summary>
    public class ShardId
    {
        /// <summary>
        /// Initializes a new instance of the ShardId class
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        public ShardId(string id)
        {
            this.Id = id;
        }

        /// <summary>
        /// 分片标识Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return Id.Equals(((ShardId)obj).Id);
        }
    }
}
