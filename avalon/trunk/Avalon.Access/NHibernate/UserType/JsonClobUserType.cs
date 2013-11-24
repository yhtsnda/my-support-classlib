using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.SqlTypes;

namespace Avalon.NHibernateAccess
{
    /// <summary>
    /// JsonClobUserType
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonClobUserType<T> : JsonUserType<T> where T : ICloneable, new()
    {
        /// <summary>
        /// types
        /// </summary>
        static SqlType[] types = new SqlType[] { NHibernateUtil.StringClob.SqlType };
    }
}
