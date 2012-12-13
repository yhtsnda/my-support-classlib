using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AnalysisReporter
{
    public class ReportDefineMetadata
    {
        private Type mReportType;
        private List<PropertyInfo> mReportProperties;

        public ReportDefineMetadata(Type reportType)
        {
            this.mReportType = reportType;
            this.mReportProperties = new List<PropertyInfo>();
        }

        public Type ReportType
        {
            get { return this.mReportType; }
        }

        public List<PropertyInfo> ReportProperties
        {
            get { return this.mReportProperties; }
        }
    }
}
