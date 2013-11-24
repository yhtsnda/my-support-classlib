using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.SqlTypes;

namespace Avalon.NHibernateAccess
{
    /// <summary>
    /// JsonClobListUserType
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonClobListUserType<T> : JsonListUserType<T>
    {
        static SqlType[] types = new SqlType[] { NHibernateUtil.StringClob.SqlType };

        /// <summary>
        /// SqlTypes
        /// </summary>
        public override SqlType[] SqlTypes
        {
            get { return types; }
        }
    }
}
