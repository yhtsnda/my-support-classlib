using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 命令执行都接口
    /// </summary>
    public interface ICommandExecutor<TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        void Execute(TCommand command);
    }
}
