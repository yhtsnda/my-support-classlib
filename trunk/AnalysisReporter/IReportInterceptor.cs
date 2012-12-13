using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisReporter
{
    public interface IReportInterceptor
    {
        void PreExecute(object entry);
        void PostExecute(object entry);
    }

    public interface IReportInterceptor<TEntity> : IReportInterceptor where TEntity : class
    {
        void PreExecute(TEntity entry);
        void PostExecute(TEntity entry);
    }
}
