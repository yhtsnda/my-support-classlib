using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Framework.OAuth2
{
    public class OAuthClient
    {
        protected OAuthClient()
        { }

        public OAuthClient(string name, string secret)
        {
            this.Name = name;
            this.Secret = secret;
        }
        /// <summary>
        /// 应用标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 应用介绍
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// App Key
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// App Secret
        /// </summary>
        public virtual string Secret { get; set; }
    }
}
