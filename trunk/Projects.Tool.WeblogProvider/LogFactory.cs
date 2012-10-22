using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Interfaces;

namespace Projects.Tool.WeblogProvider
{
    public class LogFactory : ILogFactory
    {
        /// <summary>
        /// 返回日志操作类实例
        /// </summary>
        /// <param name="name">模块名称</param>
        /// <returns>日志操作类实例</returns>
        public ILog GetLogger(string name)
        {
            return new Log(name);
        }

        /// <summary>
        /// 返回日志操作类实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>日志操作类实例</returns>
        public ILog GetLogger(Type type)
        {
            return new Log(type.Name);
        }

        /// <summary>
        /// 刷新日志管理器
        /// </summary>
        public void Flush()
        {
            LogManager.Flush();
        }
    }
}
