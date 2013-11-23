using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 缓存器实现者接口
    /// </summary>
    public interface ICacheImplementor : ICache
    {
        bool IsLocal { get; }

        bool IsInited { get; }

        int ExpiredSeconds { get; set; }

        void InitSetting(IEnumerable<SettingNode> settingNodes);

        void InitCache();

        void SetInnerCache();
    }
}
