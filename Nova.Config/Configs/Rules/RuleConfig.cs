using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    public class RuleConfig
    {
        public RuleConfig(IList<string> columns, string argorithm)
        {
            if (columns == null || columns.Count == 0)
                throw new ArgumentNullException("columns is null");
            if (String.IsNullOrEmpty(argorithm))
                throw new ArgumentNullException("argorithm is null");

            this.Columns = columns;
            this.Algorithm = argorithm;
        }

        public IList<string> Columns { get; protected set; }
        public string Algorithm { get; protected set; }
        public IRuleAlgorithm RuleAlgorithm { get; set; }
    }
}
