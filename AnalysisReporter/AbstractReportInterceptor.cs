using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisReporter
{
    public abstract class AbstractReportInterceptor<TEntity> 
        : IReportInterceptor<TEntity> where TEntity : class
    {
        public virtual void PreExecute(TEntity entry)
        {

        }

        public virtual void PostExecute(TEntity entry)
        {

        }

        void IReportInterceptor.PreExecute(object entry)
        {
            PreExecute((TEntity)entry);
        }

        void IReportInterceptor.PostExecute(object entry)
        {
            PostExecute((TEntity)entry);
        }
    }
}
