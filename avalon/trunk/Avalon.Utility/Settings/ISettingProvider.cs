using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 通用配置的支持
    /// </summary>
    public interface ISettingProvider
    {
        void Init(SettingNode node);

        ISetting Load(string id, Type settingType);

        T Load<T>(string id) where T : ISetting;

        void Save(ISetting setting);
    }
}
