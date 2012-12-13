using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisReporter
{
    internal class DefaultAnalysisProcessor<TEntity> : IAnalysisProcessor where TEntity : class
    {
        object Process(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
