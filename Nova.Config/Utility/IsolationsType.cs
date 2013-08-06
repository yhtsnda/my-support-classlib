using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    public enum IsolationsType
    {
        ReadUncommitted = 1,
        ReadCommitted = 2,
        RepeatedRead = 3,
        Serializable = 4
    }
}
