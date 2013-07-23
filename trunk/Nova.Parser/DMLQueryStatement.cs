using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class DMLQueryStatement : DMLStatement, IQueryExpression
    {
        protected static IList EnsureListType(IList list)
        {
            if (list == null || list.Count <= 0)
                return null;
            if (list is ArrayList)
                return list;
            return new ArrayList(list);
        }

        protected static List<List<IExpression>> CheckAndConvertValuesList(List<List<IExpression>> valueList)
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
