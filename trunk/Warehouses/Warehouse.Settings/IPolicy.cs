using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Warehouse.Utility;

namespace Warehouse.Settings
{
    /// <summary>
    /// 数据在存储介质中的存放策略,非泛型接口
    /// </summary>
    public interface IPolicy
    {
        PolicyConfig GetPolicyConfig();

    }

    /// <summary>
    /// 数据在介质中存放的策略,泛型接口
    /// </summary>
    public interface IPolicy<TEntity> where TEntity : class
    {
        PolicyConfig GetPolicyConfig();
        ResultKey Storage(List<TEntity> datas);
        IList<TEntity> Obtain<TEntity>();
    }
}
