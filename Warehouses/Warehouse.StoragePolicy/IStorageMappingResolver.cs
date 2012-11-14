using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.StoragePolicy
{
    public interface IStorageMappingResolver
    {
        /// <summary>
        /// 使用给出的类型解释表名
        /// </summary>
        /// <typeparam name="T">需要被解析的类型</typeparam>
        /// <returns>表名</returns>
        string ResolveTableName<T>() where T : class, new();

        /// <summary>
        /// 使用给定的类型解析字段名称
        /// </summary>
        /// <typeparam name="T">需要被解析的类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <returns>字段名称</returns>
        string ResolveFieldName<T>(string propertyName) where T : class, new();

        /// <summary>
        /// 判断给定的属性名称是否为Key(用于分布数据)的字段
        /// </summary>
        /// <typeparam name="T">需要被解析的类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        bool IsKeyField<T>(string propertyName) where T : class, new();

        /// <summary>
        /// 判断给定的属性名称是否为自增长的字段
        /// </summary>
        /// <typeparam name="T">需要被解析的类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        bool IsAutoIdentityField<T>(string propertyName) where T : class, new();
    }
}
