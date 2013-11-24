using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 级联更新时的处理器
    /// </summary>
    public interface IClassJoinCascadeProcessor
    {
        void OnCreate(object entity);

        void OnUpdate(object entity);

        void OnDelete(object entity);
    }
}
