using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Commands
{
    /// <summary>
    /// 命令总线接口
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
