using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;
using FluentNHibernate;

namespace FluentNHibernate.Mapping
{
    public static class NHibernateMappingExtend
    {
        /// <summary>
        /// 一对多配置,适用新框架（不级联更新）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="part"></param>
        /// <param name="keyColumn">字段</param>
        /// <param name="orderBy">排序表达式 : createtime desc，可空</param>
        /// <returns></returns>
        public static OneToManyPart<T> Config<T>(this OneToManyPart<T> part, string keyColumn, string orderBy = "")
        {
            var p = part
                .KeyColumn(keyColumn)
                .LazyLoad()
                .Fetch.Select()
                .Inverse()
                .Cascade.None();

            if (!String.IsNullOrEmpty(orderBy))
                p = p.OrderBy(orderBy);

            return p;
        }

        /// <summary>
        /// 多对一配置,适用新框架（不级联更新）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="part"></param>
        /// <param name="column">字段</param>
        /// <returns></returns>
        public static ManyToOnePart<T> Config<T>(this ManyToOnePart<T> part, string column)
        {
            var p = part
                .LazyLoad()
                .Fetch.Select()
                //不能加这个，否则LoadType会使用  InternalLoadNullable ，则不能延迟加载 HHB
                //.NotFound.Ignore()
                .Cascade.None()
                .Not.Insert()
                .Not.Update()
                .Column(column);
            return p;
        }
    }
}
