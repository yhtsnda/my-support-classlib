using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    public interface ICalculate<Mapped>
    {
        void Reduce(List<Mapped> collection);

        void Merge();

        List<Mapped> Map(Array collection);
    }
}
