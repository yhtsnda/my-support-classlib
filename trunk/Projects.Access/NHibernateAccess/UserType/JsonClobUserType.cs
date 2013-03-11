using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Projects.Tool.Util;
using NHibernate;
using NHibernate.SqlTypes;

namespace Projects.Framework.NHibernateAccess
{
    /// <summary>
    /// JsonClobUserType
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonClobUserType<T> : AbstractUserType where T : ICloneable, new()
    {
        /// <summary>
        /// types
        /// </summary>
        static SqlType[] types = new SqlType[] { NHibernateUtil.StringClob.SqlType };

        /// <summary>
        /// IsMutable
        /// </summary>
        public override bool IsMutable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// SqlTypes
        /// </summary>
        public override SqlType[] SqlTypes
        {
            get { return types; }
        }

        /// <summary>
        /// ReturnedType
        /// </summary>
        public override Type ReturnedType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// DeepCopy
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object DeepCopy(object value)
        {
            if (value == null)
                return null;
            return ((ICloneable)value).Clone();
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
            string value = (string)NHibernateUtil.StringClob.NullSafeGet(rs, names[0]);
            if (value == null || value.Length == 0)
                return new T();

            return JsonConverter.FromJson<T>(value);
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
                NHibernateUtil.StringClob.NullSafeSet(cmd, JsonConverter.ToJson(value), index);
            else
                NHibernateUtil.StringClob.NullSafeSet(cmd, value, index);
        }
    }
}
