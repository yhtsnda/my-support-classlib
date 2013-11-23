using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 验证器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// 是否能够验证该对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool CanHandler(T entity);

        /// <summary>
        /// 执行验证
        /// </summary>
        /// <param name="entity"></param>
        ValidationResult Validate(T entity);
    }
}
