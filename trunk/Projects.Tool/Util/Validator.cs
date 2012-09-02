using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 验证器扩展方法类
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// 实体验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="predicate"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static T Validate<T>(this T target, Predicate<T> predicate, string errorMessage)
        {
            if (!predicate(target))
            {
                throw new Exception(errorMessage);
            }
            return target;
        }

        /// <summary>
        /// 实体验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="predicate"></param>
        /// <param name="exFunc"></param>
        /// <returns></returns>
        public static T Validate<T>(this T target, Predicate<T> predicate, Func<Exception> exFunc)
        {
            if (!predicate(target))
            {
                throw exFunc();
            }
            return target;
        }
    }
}
