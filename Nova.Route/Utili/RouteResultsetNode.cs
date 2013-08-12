using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Route
{
    public class RouteResultsetNode
    {
        public const int DEFAULT_REPLICA_INDEX = -1;

        public RouteResultsetNode(string name, string statement)
            : this(name, DEFAULT_REPLICA_INDEX, statement)
        {
        }

        public RouteResultsetNode(string name, int index, string statement)
        {
            this.Name = name;
            this.ReplicaIndex = index;
            this.Statement = statement;
        }

        public string Name { get; protected set; }
        public int ReplicaIndex { get; protected set; }
        public string Statement { get; protected set; }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj.GetType().IsAssignableFrom(typeof(RouteResultsetNode)))
            {
                RouteResultsetNode node = (RouteResultsetNode)obj;
                if (ReplicaIndex == node.ReplicaIndex && Equals(Name, node.Name))
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(this.Name).Append('.');
            if (this.ReplicaIndex < 0)
            {
                s.Append("default");
            }
            else
            {
                s.Append(this.ReplicaIndex);
            }
            s.Append('{').Append(this.Statement).Append('}');
            return s.ToString();
        }

        private static bool Equals(string str1, string str2)
        {
            if (String.IsNullOrEmpty(str1))
                return str2 == null;
            return str1.Equals(str2);
        }
    }
}
