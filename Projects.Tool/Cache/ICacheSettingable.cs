using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public interface ICacheSettingable: ICache
    {
        void InitSetting(IEnumerable<SettingNode> settingNodes);
    }
}
