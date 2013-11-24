using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Avalon.NHibernateAccess
{
    /// <summary>
    /// AbstractUserType
    /// </summary>
    public abstract class AbstractUserType : IUserType
    {
        /// <summary>
        /// SqlTypes
        /// </summary>
        public abstract SqlType[] SqlTypes
        {
            get;
        }

        /// <summary>
        /// ReturnedType
        /// </summary>
        public abstract Type ReturnedType
        {
            get;
        }

        /// <summary>
        /// 是否易变
        /// </summary>
        public virtual bool IsMutable
        {
            get { return true; }
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public virtual int GetHashCode(object x)
        {
            return (x == null) ? 0 : x.GetHashCode();
        }

        /// <summary>
        /// NullSafeGet
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="names"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public abstract object NullSafeGet(System.Data.IDataReader rs, string[] names, object owner);

        /// <summary>
        /// NullSafeSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public abstract void NullSafeSet(System.Data.IDbCommand cmd, object value, int index);

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract object DeepCopy(object value);

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="original"></param>
        /// <param name="target"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public virtual object Replace(object original, object target, object owner)
        {
            return original;
        }

        /// <summary>
        /// Assemble
        /// </summary>
        /// <param name="cached"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public virtual object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        /// <summary>
        /// Disassemble
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object Disassemble(object value)
        {
            return DeepCopy(value);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual bool Equals(object x, object y)
        {
            if (object.ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            return x.Equals(y);
        }
    }
}
