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

        public bool Check(string userKey, string password, string appKey, string checkSum)
        {
            //先检查是否存在用户名,密码和AppKey
            ServiceUser user = mServiceUsers.First(m => m.UserKey == userKey && m.Password == password && m.AppKey == appKey);
            if (user == null)
                return false;
            //检查checkSum是否计算错误
            string encryptSum = StringUtil.Md5WithSalt(String.Format("{0},{1},{2}", 
                userKey, password, appKey));
            return encryptSum.Equals(checkSum);
        }
    }

    [Serializable]
    public class ServiceUser
    {
        public string UserKey { get; set; }

        public string Password { get; set; }

        public string AppKey { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ServiceUsers")]
    public class ServiceUsers : List<ServiceUser> { }
}