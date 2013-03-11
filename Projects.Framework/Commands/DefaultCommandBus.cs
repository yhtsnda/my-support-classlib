using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Commands
{
    /// <summary>
    /// 默认的命令总线接口
    /// </summary>
    public class DefaultCommandBus : ICommandBus
    {
        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            try
            {
                var executor = DependencyResolver.ResolveOptional<ICommandExecutor<TCommand>>();
                if (executor != null)
                    executor.Execute(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
