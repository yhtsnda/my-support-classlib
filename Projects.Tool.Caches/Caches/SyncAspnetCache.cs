using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;

namespace Projects.Tool
{
    public class SyncAspnetCache : AspnetCache
    {
        List<SyncItem> ipAddresses;
        string localIPAddress;
        string defaultUrlFormat;
        string defaultHost;
        int requestTimeOut;

        protected virtual string GetHost(SyncItem item)
        {
            if (!String.IsNullOrEmpty(item.Host))
                return item.Host;
            if (String.IsNullOrEmpty(defaultHost))
                throw new ConfigurationException(String.Format("无法为IP为{0}设置HOST", item.IPAddress));
            return defaultHost;
        }

        protected virtual string GetUrlFormat(SyncItem item)
        {
            if (!String.IsNullOrEmpty(item.UrlFormat))
                return item.UrlFormat;
            if (String.IsNullOrEmpty(defaultUrlFormat))
                throw new ConfigurationException(String.Format("无法为IP为{0}设置URL-FORMAT", item.IPAddress));
            return defaultUrlFormat;
        }

        public override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            base.InitSetting(settingNodes);

            defaultUrlFormat = TryGetValue(settingNodes, "urlFormat");
            if (defaultUrlFormat == null)
                throw new MissConfigurationException(settingNodes, "urlFormat");

            defaultHost = TryGetValue(settingNodes, "host");

            string requestTimeOutStr = TryGetValue(settingNodes, "requestTimeout");
            requestTimeOut = String.IsNullOrEmpty(requestTimeOutStr) ? 10000 : Int32.Parse(requestTimeOutStr);

            IEnumerable<SettingNode> addNodes = TryGetNodes(settingNodes, "add");
            if (addNodes.Count() == 0)
                throw new MissConfigurationException(settingNodes, "add");

            ipAddresses = new List<SyncItem>();
            foreach (SettingNode node in addNodes)
            {
                if (!node.Contains("ip"))
                    throw new MissConfigurationException(node, "ip");

                ipAddresses.Add(new SyncItem()
                {
                    IPAddress = node.Attributes["ip"],
                    Host = node.TryGetValue("host"),
                    UrlFormat = node.TryGetValue("urlFormat"),
                    Disabled = node.TryGetValue("disabled") == "true"
                });
            }
            localIPAddress = GetLocalIPAddress(ipAddresses);
            if (String.IsNullOrEmpty(localIPAddress))
                throw new ConfigurationException("无法获取本地IP，IP列表中必须包含本地的IP。");
        }

        protected override void SetInner<T>(string key, T value, DateTime expiredTime)
        {
            base.SetInner<T>(key, value, expiredTime);
            //  BeginSyncCache(key);
        }

        protected override void RemoveInner(Type type, string key)
        {
            base.RemoveInner(type, key);
            BeginSyncCache(key);
        }

        protected virtual void BeginSyncCache(string key)
        {
            SyncCacheHandler handler = new SyncCacheHandler(SyncCache);
            handler.BeginInvoke(key, null, null);
        }

        void SyncCache(string key)
        {
            Projects.Tool.Util.SyncCacheHandler.Add(this.CacheName);

            ILog log = LogManager.GetLogger("Projects.Tool");
            foreach (SyncItem item in ipAddresses)
            {
                if (!item.Disabled && item.IPAddress != localIPAddress)
                {
                    string urlFormat = GetUrlFormat(item);
                    string url = String.Format(urlFormat, item.IPAddress, key, "");

                    log.Debug("正在同步缓存: " + url);
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Host = GetHost(item);
                        request.Timeout = requestTimeOut;
                        request.Method = "GET";

                        using (WebResponse response = request.GetResponse())
                        {
                        }

                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("同步缓存{0}发生错误:{1}", url, ex.Message);
                        throw ex;
                    }
                }
            }
        }

        string GetLocalIPAddress(List<SyncItem> ips)
        {
            System.Net.IPHostEntry iPEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            var lips = iPEntry.AddressList.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToList();
            if (lips.Count == 1)
                return lips.First().ToString();

            foreach (var item in lips)
            {
                string ip = item.ToString();
                if (ips.Any(o => o.IPAddress == ip))
                    return ip;
            }
            return null;
        }

        delegate void SyncCacheHandler(string key);

        protected class SyncItem
        {
            public string IPAddress { get; set; }

            public string Host { get; set; }

            public string UrlFormat { get; set; }

            public bool Disabled { get; set; }
        }

    }
}
