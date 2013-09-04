using Nova.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Route
{
    public class RouteResultset
    {
        public const int SUM_FLAG = 1;
        public const int MIN_FLAG = 2;
        public const int MAX_FLAG = 3;
        public const int REWRITE_FIELD = 4;

        public RouteResultset(string statement)
        {
            this.Statement = statement;
            this.LimitSize = -1;
        }

        public string Statement { get; protected set; }
        public List<RouteResultsetNode> Nodes { get; set; }
        public int Flag { get; set; }
        public long LimitSize { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Statement).Append(", route={");
            if (Nodes != null)
            {
                for (int i = 0; i < Nodes.Count; ++i)
                {
                    builder.Append("\n").Append(FormatUtil.Format(i + 1, 3));
                    builder.Append(" -> ").Append(Nodes[i]);
                }
            }
            builder.Append("\n}");
            return builder.ToString();
        }
    }
}
