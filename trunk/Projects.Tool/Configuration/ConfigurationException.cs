using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException()
        {
        }

        public ConfigurationException(string message)
            : base(message)
        {
        }
    }

    public class MissConfigurationException : ConfigurationException
    {
        string message;

        public MissConfigurationException(SettingNode settingNode, string path)
            : this(new List<SettingNode>() { settingNode }, path)
        {
        }

        public MissConfigurationException(IEnumerable<SettingNode> settingNodes, string path)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("在以下路径找未找到需要的数据:");
            foreach (SettingNode node in settingNodes)
            {
                builder.AppendLine(node.Path + path + ";");
            }
            message = builder.ToString();
        }

        public override string Message
        {
            get
            {
                return message;
            }
        }
    }
}
