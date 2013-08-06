using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    /// <summary>
    /// 数据源配置
    /// </summary>
    public class DataSourceConfig
    {
        public string SourceName { get; set; }

        public string SourceType { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public string Database { get; set; }

        public int Port { get; set; }

        public string SqlMode { get; set; }

        public override string ToString()
        {
            return String.Format("[name={0},host={1},prot={2},database={3}]",
                this.SourceName, this.Host, this.Port, this.Database);
        }
    }
}
