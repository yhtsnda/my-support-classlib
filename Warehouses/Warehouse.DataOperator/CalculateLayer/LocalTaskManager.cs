using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Warehouse.DataOperator
{
    public class LocalTaskManager
    {
        /// <summary>
        /// 处理器缓存
        /// </summary>
        protected static Dictionary<Type, Func<object>> processerCache = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// 检查并创建处理器的实例 
        /// </summary>
        /// <param name="processer">处理器类</param>
        /// <returns>如果处理器继承至ICalculate接口,则返回该处理器的实例否则抛出异常</returns>
        public static object CheckAndCreate(Type processer)
        {
            Func<object> fun;
            if (!processerCache.TryGetValue(processer, out fun))
            {
                lock (processerCache)
                {
                    if (!processerCache.TryGetValue(processer, out fun))
                    {
                        //检查处理器是否为一个类
                        if (!processer.IsClass && processer.IsAbstract)
                            throw new ArgumentException("处理器必须是一个类,且不能是抽象类");
                        //检查处理器是否继承ICalculate接口
                        TypeFilter filter = new TypeFilter(CalculateFinder);
                        var findResult = processer.FindInterfaces(filter, "Warehouse.DataOperator.ICalculate");
                        //如果处理器是一个继承至ICalculate接口的类
                        if (findResult.Length > 0)
                        {
                            processerCache[processer] = fun =
                                Expression.Lambda<Func<object>>(Expression.New(processer),
                                new ParameterExpression[0]).Compile();
                            return fun.Invoke();
                        }
                        //如果不是
                        throw new ArgumentException("处理器必须继承ICalculate接口");                            
                    }
                }
            }
            return fun.Invoke();
        }

        /// <summary>
        /// 创建Process的实例,不检查是否实现ICalculate接口
        /// </summary>
        /// <param name="processer">处理器的类型</param>
        /// <returns>处理器的实例</returns>
        public static object Create(Type processer)
        {
            Func<object> fun;
            if (!processerCache.TryGetValue(processer, out fun))
            {
                lock (processerCache)
                {
                    if (!processerCache.TryGetValue(processer, out fun))
                    {
                        processerCache[processer] = fun =
                        Expression.Lambda<Func<object>>(Expression.New(processer),
                        new ParameterExpression[0]).Compile();
                    }
                    return fun.Invoke();
                }
            }
            else
            {
                fun = processerCache[processer];
                return fun.Invoke();
            }
            
        }

        internal static bool CalculateFinder(Type type, object criteriaObj)
        {
            if (type.ToString() == criteriaObj.ToString())
                return true;
            return false;
        }
    }
}
