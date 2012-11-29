using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;

namespace Warehouse.Utility
{
    /// <summary>
    /// 操作结果键
    /// </summary>
    public enum ResultKey
    {
        NotSet,
        OK,
        Failure,
        Exception
    }

    public static class ResultKeyExtend
    {
        public string ToMessage(this ResultKey key)
        {
            return ResultMessage.Instance.GetString(key.ToString());
        }
    }

    /// <summary>
    /// 操作结果的消息,统一从Resource文件中获取
    /// </summary>
    internal class ResultMessage
    {
        private static ResultMessage mInstance;
        private ResourceManager mResourceManager;

        /// <summary>
        /// 操作结果的消息,统一从Resource文件中获取
        /// </summary>
        public static ResultMessage Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new ResultMessage();
                return mInstance;
            }
        }

        /// <summary>
        /// 从资源文件中获取指定的字符串
        /// </summary>
        /// <param name="key">字符串的键</param>
        /// <returns>如找不到返回"未找到资源"</returns>
        public string GetString(string key)
        {
            try
            {
                string result = mResourceManager.GetString(key);
                return String.IsNullOrEmpty(result) ? "未找到资源" : result;
            }
            catch
            {
                return "查找资源时发生错误";
            }
        }

        protected ResultMessage()
        {
            mResourceManager = new ResourceManager("Warehouse.Utility.Messages",
                Assembly.GetExecutingAssembly());
        }
    }
}
