using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public class CommandBus
    {
        public static void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new AvalonException("command命令对象不能为空");
            DependencyResolver.Resolve<ICommandBus>().Send(command);
        }
    }
}
