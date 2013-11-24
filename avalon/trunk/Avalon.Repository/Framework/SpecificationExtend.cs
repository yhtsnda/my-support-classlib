using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public static partial class SpecificationExtend
    {
        public static bool IsIn<T>(this T projection, ICollection values)
        {
            throw new Exception("Not to be used directly - use inside Query expression");
        }

        public static bool IsNotIn<T>(this T projection, ICollection values)
        {
            throw new Exception("Not to be used directly - use inside Query expression");
        }

        public static bool IsNull<T>(this T projection)
        {
            throw new Exception("Not to be used directly - use inside Query expression");
        }

        public static bool IsNotNull<T>(this T projection)
        {
            throw new Exception("Not to be used directly - use inside Query expression");
        }

        public static bool IsLike(this string projection, string comparsion)
        {
            throw new Exception("Not to be used directly - use inside Query expression");
        }

        public static bool IsLike(this string projection, string comparsion, LikeMode mode)
        {
            throw new Exception("Not to be used directly - use inside Query expression");
        }
    }

    public enum LikeMode
    {
        Anywhere,
        Start,
        End
    }
}
