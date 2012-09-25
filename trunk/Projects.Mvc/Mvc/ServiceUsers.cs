using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Projects.Tool.Util;

namespace BuildingSiteCheck.Mvc
{
    /// <summary>
    /// 允许调用WebService的用户列表
    /// </summary>
    public class ServiceUserAuth
    {
        private static ServiceUserAuth mServiceUserAuth = new ServiceUserAuth();
        private static ServiceUsers mServiceUsers;

        /// <summary>
        /// 允许调用WebService的用户列表类
        /// </summary>
        public static ServiceUserAuth Instance
        {
            get
            {
                if (mServiceUserAuth == null)
                    mServiceUserAuth = new ServiceUserAuth();
                return mServiceUserAuth;
            }
        }

        /// <summary>
        /// 允许调用WebService的用户列表
        /// </summary>
        protected ServiceUserAuth()
        {
            LoadServiceUsers();
        }

        /// <summary>
        /// 加载服务用户
        /// </summary>
        private void LoadServiceUsers()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/ServiceUsers.xml");

            if (File.Exists(filePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ServiceUsers));
                using (StreamReader sr = new StreamReader(filePath))
                {
                    mServiceUsers = xs.Deserialize(sr) as ServiceUsers;
                }
            }
        }

        /// <summary>
        /// 重新加载
        /// </summary>
        public void ReloadServiceUsers()
        {
            mServiceUsers.Clear();
            LoadServiceUsers();
        }

        /// <summary>
        /// 检查指定的用户对接口是否有访问权限(不需要进行IP验证)
        /// </summary>
        /// <param name="userKey">用户键</param>
        /// <param name="password">密码</param>
        /// <param name="appKey">APP Key</param>
        /// <param name="checkSum">检验码</param>
        /// <returns>检查结果</returns>
        public bool Check(string userKey, string password, string appKey, string checkSum)
        {
            //先检查是否存在用户名,密码和AppKey
            ServiceUser user = mServiceUsers.First(m => m.UserKey == userKey && 
                m.Password == password && m.AppKey == appKey);
            if (user == null)
                return false;
            //检查checkSum是否计算错误
            string encryptSum = StringUtil.Md5WithSalt(String.Format("{0},{1},{2}", 
                userKey, password, appKey));
            return encryptSum.Equals(checkSum);
        }

        /// <summary>
        /// 检查指定的用户对接口是否有访问权限(需要进行IP验证)
        /// </summary>
        /// <param name="userKey">用户键</param>
        /// <param name="password">密码</param>
        /// <param name="appKey">APP Key</param>
        /// <param name="checkSum">检验码</param>
        /// <param name="ip">用户访问的IP</param>
        /// <returns></returns>
        public bool Check(string userKey, string password, string appKey, string checkSum, string ip)
        {
            //先检查是否存在用户名,密码和AppKey
            ServiceUser user = mServiceUsers.First(m => m.UserKey == userKey &&
                m.Password == password && m.AppKey == appKey);
            if (user == null)
                return false;
            //检查checkSum是否计算错误
            string encryptSum = StringUtil.Md5WithSalt(String.Format("{0},{1},{2}",
                userKey, password, appKey));
            //如果检查错误,就直接返回了
            bool result = encryptSum.Equals(checkSum);
            if (!result)
                return result;

            //找到用户后,检查该用户允许访问的IP列表中是否包含指定的IP
            //如果允许所有的IP调用
            if (user.IPList.Contains("*"))
                return true;
            //将IP地址的后一位替换为"*"
            var ipFields = ip.Split('.');
            if (ipFields.Length != 4)
                return false;

            string ipWildCard = ipFields[0] + "." + ipFields[1] + "." + ipFields[2] + ".*";
            if (user.IPList.Contains(ipWildCard))
                return true;
            //将IP地址的后两位替换为"*"
            ipWildCard = ipFields[0] + "." + ipFields[1] + ".*.*";
            if (user.IPList.Contains(ipWildCard))
                return true;
            //找到精准的IP匹配
            if (user.IPList.Contains(ip))
                return true;
            //都没有匹配到
            return false;
        }
    }

    [Serializable]
    public class ServiceUser
    {
        public string UserKey { get; set; }

        public string Password { get; set; }

        public string AppKey { get; set; }

        public string AllowIp { get; set; }

        static List<string> mIPList = null;
        [NonSerialized]
        public List<string> IPList 
        {
            get
            {
                if (mIPList == null)
                {
                    mIPList = new List<string>();
                    var list = JsonConverter.FromJson<List<string>>(AllowIp);
                    //删除所有不符合规范的IP地址
                    foreach (string ip in list)
                    {
                        if (ip == "*")
                        {
                            mIPList.Add(ip);
                            break;
                        }
                        if (IpAddress.ValidateIP(ip, true))
                            mIPList.Add(ip);
                    }
                }
                return mIPList;
            }
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "ServiceUsers")]
    public class ServiceUsers : List<ServiceUser> { }
}