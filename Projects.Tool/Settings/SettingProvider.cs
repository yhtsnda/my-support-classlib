﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web.Hosting;

namespace Projects.Tool.Settings
{
    public static class SettingProvider
    {
        static ISettingProvider current;
        static object syncObj = new object();

        public static ISettingProvider Current
        {
            get
            {
                if (current == null)
                {
                    lock (syncObj)
                    {
                        if (current == null)
                        {
                            var node = ToolSection.Instance.TryGetNode("setting");
                            if (node != null)
                            {
                                var providerTypeStr = node.TryGetValue("provider");
                                if (!String.IsNullOrEmpty(providerTypeStr))
                                {
                                    var providerType = Type.GetType(providerTypeStr);
                                    if (providerType == null)
                                        throw new ConfigurationException(String.Format("无法加载类型为 {0}  的对象", providerTypeStr));

                                    current = (ISettingProvider)Projects.Tool.Reflection.FastActivator.Create(providerType);
                                    current.Init(node);
                                }
                            }
                            else
                                current = new DefaultSettingProvider();
                        }
                    }

                }
                return current;
            }
            set
            {
                current = value;
            }
        }

        public static string SiteIdentity
        {
            get
            {
                var ips = Dns.GetHostAddresses(Dns.GetHostName()).Where(o => o.AddressFamily == AddressFamily.InterNetwork);
                var id = String.Join("_", ips.Select(o => GetAddress(o)).Distinct());
                return HostingEnvironment.SiteName + ":" + id;
            }
        }

        static string GetAddress(IPAddress address)
        {
            var ips = address.ToString().Split('.');
            return ips[2] + "." + ips[3];
        }
    }
}
