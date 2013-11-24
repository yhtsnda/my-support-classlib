using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Avalon.NHibernateAccess
{
    /// <summary>
    /// SplitUserType
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SplitUserType<T> : AbstractUserType, IParameterizedType
    {
        private static readonly SqlType[] sqlTypes = new SqlType[] { NHibernateUtil.String.SqlType };
        string separator = ",";

        /// <summary>
        /// SqlTypes
        /// </summary>
        public override SqlType[] SqlTypes
        {
            get { return sqlTypes; }
        }

        /// <summary>
        /// ReturnedType
        /// </summary>
        public override Type ReturnedType
        {
            get { return typeof(IList<T>); }
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
            object obj = NHibernateUtil.StringClob.NullSafeGet(rs, names[0]);
            if (obj == null)
                return new List<T>();

            string[] vs = ((string)obj).Split(new string[] { separator }, StringSplitOptions.None);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return vs.Select(o => (T)converter.ConvertFromString(o)).ToList();
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
                List<T> values = (List<T>)value;
                string sv = string.Join(separator, values);
                NHibernateUtil.String.NullSafeSet(cmd, sv, index);
            }
            else
            {
                NHibernateUtil.String.NullSafeSet(cmd, value, index);
            }
        }

        /// <summary>
        /// SetParameterValues
        /// </summary>
        /// <param name="parameters"></param>
        public void SetParameterValues(IDictionary<string, string> parameters)
        {
            string sep;
            if (parameters != null && parameters.TryGetValue("separator", out sep))
                separator = sep;
        }

        public override object DeepCopy(object value)
        {
            return DeepCopy((List<T>)value);
        }

        List<T> DeepCopy(List<T> list)
        {
            if (list == null)
                return null;

            return list.Select(o => o).ToList();
        }
    }
}
