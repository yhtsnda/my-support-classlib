using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.Configure
{
    public interface IConfigure
    {
        /// <summary>
        /// 标记配置的唯一键
        /// </summary>
        string ConfigKey
        {
            get;
            set;
        }

        void Load();

        void Save();
    }
}
