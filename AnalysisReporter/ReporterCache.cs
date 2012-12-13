using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisReporter
{
    public class ReporterCache
    {
        private static ReporterCache mReportCache;
        private Dictionary<string, ReportDefineMetadata> mReportDefineMetadatas;

        protected ReporterCache()
        {
            this.mReportDefineMetadatas = new Dictionary<string, ReportDefineMetadata>();
        }

        public static ReporterCache Instance
        {
            get
            {
                if (mReportCache == null)
                    mReportCache = new ReporterCache();
                return mReportCache;
            }
        }

        public ReportDefineMetadata TryGetMetadata(string key)
        {
            ReportDefineMetadata metaData = default(ReportDefineMetadata);
            this.mReportDefineMetadatas.TryGetValue(key, out metaData);
            if (metaData == default(ReportDefineMetadata))
                return null;
            return metaData;
        }
    }
}
