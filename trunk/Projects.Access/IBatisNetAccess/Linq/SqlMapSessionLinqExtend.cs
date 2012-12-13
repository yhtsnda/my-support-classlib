using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBatisNet.DataMapper
{
    public static class SqlMapSessionLinqExtend
    {
        public static IQueryable<T> Query<T>(this ISqlMapSession session)
        {
            throw new NotImplementedException();
        }
    }
}
