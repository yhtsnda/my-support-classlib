using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisReporter
{
    public class UserCenterReportInterceptor<TEntity> 
        : AbstractReportInterceptor<TEntity>
    {
        public UserCenterReportInterceptor()
        {
        }

        public override void PreExecute(TEntity entry)
        {
            Console.WriteLine("UserCenterReportInterceptor PreExecute");
        }

        public override void PostExecute(TEntity entry)
        {
            Console.WriteLine("UserCenterReportInterceptor PostExecute");
        }
    }
}
