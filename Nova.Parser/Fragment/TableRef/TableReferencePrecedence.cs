using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public enum TableReferencePrecedence
    {
        Reference = 0,
        Join = 1,
        Factor = 2,
    }
}
