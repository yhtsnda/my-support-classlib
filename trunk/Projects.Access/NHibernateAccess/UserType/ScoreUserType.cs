using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.SqlTypes;

namespace Projects.Accesses.NHibernateRepository
{
    /// <summary>
    /// ScoreUserType
    /// </summary>
    public class ScoreUserType : AbstractUserType
    {
        /// <summary>
        /// SqlTypes
        /// </summary>
        public override NHibernate.SqlTypes.SqlType[] SqlTypes
        {
            get { return new SqlType[] { new SqlType(DbType.Int32) }; }
        }

        /// <summary>
        /// ReturnedType
        /// </summary>
        public override Type ReturnedType
        {
            get { return typeof(float); }
        }

        /// <summary>
        /// NullSafeGet
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="names"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            object value = NHibernateUtil.UInt32.NullSafeGet(rs, names[0]);
            if (value == null)
                value = 0;

            return Convert.ToSingle((uint)value / 100f);
        }

        /// <summary>
        /// NullSafeSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public override void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value != null)
            {
                value = Convert.ToSingle(value) * 100;
                NHibernateUtil.UInt32.NullSafeSet(cmd, Convert.ToUInt32(value), index);
            }
            else
                NHibernateUtil.UInt32.NullSafeSet(cmd, value, index);
        }
    }
}
