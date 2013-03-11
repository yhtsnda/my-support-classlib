using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 当前数据操作的上下文
    /// </summary>
    public class DataContext
    {
        /// <summary>
        /// 将未保存到存储层的属性进行刷入操作
        /// </summary>
        public static void Flush()
        {
            Projects.Tool.Util.Workbench.Flush();
        }
    }
}
