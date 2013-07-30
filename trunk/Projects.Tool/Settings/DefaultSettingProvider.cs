using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Settings
{
    internal class DefaultSettingProvider : ISettingProvider
    {
        Dictionary<string, ISetting> settings = new Dictionary<string, ISetting>();

        public void Init(SettingNode node)
        {
        }

        public ISetting Load(string id, Type settingType)
        {
            Arguments.NotNull(id, "id");
            return settings.TryGetValue(id);
        }

        public void Save(ISetting setting)
        {
            Arguments.NotNull(setting.Id, "Id");
            settings[setting.Id] = setting;
        }

        public T Load<T>(string id) where T : ISetting
        {
            Arguments.NotNull(id, "id");
            return (T)Load(id, typeof(T));
        }
    }
}
