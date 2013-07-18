using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    public class ClientAppModel
    {
        public ClientAppModel()
        {
        }

        /// <summary>
        /// 应用代码(对应OAuth的ClientId)
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 应用标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 应用描述
        /// </summary>
        public string Description { get; set; }
    }
}
