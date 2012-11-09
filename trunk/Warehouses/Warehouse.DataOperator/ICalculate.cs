using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    public interface ICalculate
    {
        void Reduce();

        void Merge();
    }
}
