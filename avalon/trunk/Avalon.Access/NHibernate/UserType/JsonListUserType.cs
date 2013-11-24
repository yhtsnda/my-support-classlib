using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.SqlTypes;
using Avalon.Utility;

namespace Avalon.NHibernateAccess
{
    /// <summary>
    /// JsonListUserType
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonListUserType<T> : AbstractUserType
    {
        static SqlType[] types = new SqlType[] { NHibernateUtil.String.SqlType };

        public JsonListUserType()
        {
            var type = typeof(T);
            if (!type.IsValueType && type != typeof(string) && type.GetInterface(typeof(ICloneable).FullName) == null)
                throw new AvalonException("类型 {0} 无法克隆, 请实现 ICloneable 接口。", type.FullName);
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
            get { return typeof(List<T>); }
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

            var source = (IList<T>)value;
            return source.Select(o => CloneValue(o)).ToList();
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
                return new List<T>();

            return JsonConverter.FromJson<List<T>>(value);
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

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
                return true;

            IList lx = (IList)x;
            IList ly = (IList)y;
            if (lx == null || ly == null)
                return base.Equals(x, y);

            if (lx.Count != ly.Count)
                return false;

            for (int i = 0; i < lx.Count; i++)
            {
                if (!lx[i].Equals(ly[i]))
                    return false;
            }

            return true;
        }

        T CloneValue(T value)
        {
            var type = typeof(T);
            if (type.IsValueType || type == typeof(string))
                return value;
            return (T)((ICloneable)value).Clone();
        }
    }
}
