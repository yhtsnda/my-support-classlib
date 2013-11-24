using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 使用关联时用于获取原始数据的处理器
    /// </summary>
    internal interface IClassJoinDataProcessor
    {
        object Process(object entity);
    }
}
