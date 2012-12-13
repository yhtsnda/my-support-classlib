using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisReporter
{
    internal interface IAnalysisProcessor
    {
        object Process(object entity);
    }
}
