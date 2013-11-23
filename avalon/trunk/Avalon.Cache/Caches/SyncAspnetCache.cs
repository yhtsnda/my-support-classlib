using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;

namespace Avalon.Utility
{
    public class SyncAspnetCache : AspnetCache
    {
        ILog log = LogManager.GetLogger("avalon");

        string currentIPAddress;

        public string DefaultUrlFormat { get; set; }

        public string DefaultHost { get; set; }

        public int RequestTimeout { get; set; }

        public string CurrentHost { get; set; }

        public List<SyncItem> IpAddresses { get; set; }

        public SyncAspnetCache()
        {
            IpAddresses = new List<SyncItem>();
            RequestTimeout = 10000;
        }

        protected virtual string GetHost(SyncItem item)
        {
            if (!String.IsNullOrEmpty(item.Host))
                return item.Host;
            if (String.IsNullOrEmpty(DefaultHost))
                throw new ConfigurationException(String.Format("无法为IP为{0}设置HOST", item.IPAddress));
            return DefaultHost;
        }

        protected virtual string GetUrlFormat(SyncItem item)
        {
            if (!String.IsNullOrEmpty(item.UrlFormat))
                return item.UrlFormat;
            if (String.IsNullOrEmpty(DefaultUrlFormat))
                throw new ConfigurationException(String.Format("无法为IP为{0}设置URL-FORMAT", item.IPAddress));
            return DefaultUrlFormat;
        }

        protected override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            base.InitSetting(settingNodes);

            this.TrySetSetting(settingNodes, ConfigurationName, "urlFormat", (o) => o.DefaultUrlFormat);
            this.TrySetSetting(settingNodes, ConfigurationName, "host", (o) => o.DefaultHost);
            this.TrySetSetting(settingNodes, ConfigurationName, "requestTimeout", (o) => o.RequestTimeout);
            this.TrySetSetting(settingNodes, ConfigurationName, "currentHost", (o) => o.CurrentHost);

            IEnumerable<SettingNode> addNodes = ToolSettingUtil.TryGetNodes(settingNodes, ConfigurationName, "add");
            if (addNodes.Count() == 0)
                throw new MissConfigurationException(settingNodes, "add");

            foreach (SettingNode node in addNodes)
            {
                if (!node.Contains("ip"))
                    throw new MissConfigurationException(node, "ip");

                IpAddresses.Add(new SyncItem()
                {
                    IPAddress = node.Attributes["ip"],
                    Host = node.TryGetValue("host"),
                    UrlFormat = node.TryGetValue("urlFormat"),
                    Disabled = node.TryGetValue("disabled") == "true"
                });
            }
        }

        protected override void InitCache()
        {
            base.InitCache();

            if (String.IsNullOrEmpty(DefaultUrlFormat))
                throw new ArgumentNullException("DefaultUrlFormat");
            if (String.IsNullOrEmpty(DefaultHost))
                throw new ArgumentNullException("DefaultHost");

            if (IpAddresses.Count == 0)
                throw new ArgumentNullException("IpAddresses");

            currentIPAddress = GetCurrentIPAddress(IpAddresses);
            if (String.IsNullOrEmpty(currentIPAddress))
                throw new ConfigurationException("无法获取本地IP，IP列表中必须包含本地的IP。");

        }

        protected override void RemoveInner(Type type, string key)
        {
            base.RemoveInner(type, key);
            BeginSyncCache(key);
        }

        protected virtual void BeginSyncCache(string key)
        {
            log.InfoFormat("调用缓存同步: {0}", key);
            SyncCacheHandler handler = new SyncCacheHandler(SyncCache);
            handler.BeginInvoke(key, null, null);
        }

        void SyncCache(string key)
        {
            Avalon.Utility.SyncCacheHandler.Add(this.CacheName);

            foreach (SyncItem item in IpAddresses)
            {
                if (item.Disabled)
                    continue;

                if (item.IPAddress == currentIPAddress && item.Host == CurrentHost)
                    continue;

                string urlFormat = GetUrlFormat(item);
                string url = String.Format(urlFormat, item.IPAddress, System.Web.HttpUtility.UrlEncode(key), "");

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Host = GetHost(item);
                    request.Timeout = RequestTimeout;
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

        string GetCurrentIPAddress(List<SyncItem> ips)
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

        public class SyncItem
        {
            public string IPAddress { get; set; }

            public string Host { get; set; }

            public string UrlFormat { get; set; }

            public bool Disabled { get; set; }
        }

    }
}
