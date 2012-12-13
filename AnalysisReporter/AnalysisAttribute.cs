using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AnalysisReporter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property,
        AllowMultiple = false, Inherited = false)]
    public class AnalysisAttribute : Attribute
    {
        private bool mExecuteAtPre = true;
        private bool mExecuteAtPost = false;

        public bool ExecuteAtPre { get { return this.mExecuteAtPre; } set { this.mExecuteAtPre = value; } }
        public bool ExecuteAtPost { get { return this.mExecuteAtPost; } set { this.mExecuteAtPost = value; } }

        public AnalysisAttribute()
        {
        }
    }
}
