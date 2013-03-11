using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Builders
{
    public sealed class WhereBuilderResult
    {
        public string WhereClause { get; internal set; }

        public Dictionary<string, object> ParameterValues { get; internal set; }

        public WhereBuilderResult()
        {
        }

        public WhereBuilderResult(string whereClause, Dictionary<string, object> parameterValues)
        {
            this.WhereClause = whereClause;
            this.ParameterValues = parameterValues;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(WhereClause);
            builder.Append(Environment.NewLine);
            ParameterValues.ToList().ForEach(v =>
            {
                builder.Append(string.Format("{0} = [{1}] (Type: {2})", v.Key, v.Value.ToString(), 
                    v.Value.GetType().FullName));
                builder.Append(Environment.NewLine);
            });
            return builder.ToString();
        }
    }
}
