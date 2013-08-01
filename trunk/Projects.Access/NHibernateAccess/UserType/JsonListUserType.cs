using System;
using System.Collections;
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
    /// JsonListUserType
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonListUserType<T> : AbstractUserType
    {
        static SqlType[] types = new SqlType[] { NHibernateUtil.String.SqlType };

        /// <summary>
        /// SqlTypes
        /// </summary>
        public override SqlType[] SqlTypes
        {
            get { return types; }
        }

        /// <summary>
        /// 返回类型
        /// </summary>
        public override Type ReturnedType
        {
            get { return typeof(T); }
        }

        public JsonListUserType()
        {
            var type = typeof(T);
            if (!type.IsValueType && type != typeof(string) && type.GetInterface(typeof(ICloneable).FullName) == null)
                throw new PlatformException("类型 {0} 无法克隆, 请实现 ICloneable 接口。", type.FullName);
        }

        /// <summary>
        /// 深拷贝
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
        /// 获取方法
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="names"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            string value = (string)NHibernateUtil.String.NullSafeGet(rs, names[0]);
            if (value == null || value.Length == 0)
                return new List<T>();

            return JsonConverter.FromJson<List<T>>(value);
        }

        /// <summary>
        /// 设置方法
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public override void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value != null)
                NHibernateUtil.String.NullSafeSet(cmd, JsonConverter.ToJson(value), index);
            else
                NHibernateUtil.String.NullSafeSet(cmd, value, index);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool Equals(object x, object y)
        {
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
            if (type.IsValueType || type == typeof(String))
                return value;
            return (T)((ICloneable)value).Clone();
        }
    }
}
