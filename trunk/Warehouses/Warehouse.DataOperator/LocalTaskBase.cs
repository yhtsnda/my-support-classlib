using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    public abstract class LocalTaskBase : ITask
    {
        public event EventHandler OnException;

        public event EventHandler OnProcess;

        public event EventHandler OnProcessOver;

        public abstract void Process();

        public abstract void Stop();

        public abstract void Pause();
    }
}
