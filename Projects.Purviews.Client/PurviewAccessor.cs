using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;

namespace Projects.Purviews
{
    public class PurviewAccessor
    {
        private static readonly string PurviewUrl;

        static PurviewAccessor()
        {
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["PurviewUrl"]))
                throw new ConfigurationErrorsException("Missing ConfigSetting PurviewUrl!");
            PurviewUrl = ConfigurationManager.AppSettings["PurviewUrl"];
        }

        public static bool CheckAction(int userId, string instanceKey, string actionKey)
        {
            using(var wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string result = wc.DownloadString(
                    String.Format("{0}CheckAction?userId={1}&instanceKey={2}&actionKey={3}",
                    PurviewUrl,userId,instanceKey,actionKey));
                return InvokeResult<bool>(result);
            }
        }

        public static IList<bool> CheckAction(int userId, string instanceKey, IList<string> actionKeys)
        {
            string strPurviewKeys = string.Join("&", actionKeys.Select(o => string.Format("actionKeys={0}", o)));
            using (var wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                string result = wc.DownloadString(String.Format("{0}CheckActions?userId={1}&instanceKey={2}&{3}",
                    PurviewUrl, userId, instanceKey, strPurviewKeys));
                return InvokeResult<List<bool>>(result);
            }
        }

        public static bool CheckResource(long userId, string instanceKey, string resourceKey)
        {
            using (var wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                string result = wc.DownloadString(String.Format("{0}CheckAction?userId={1}&instanceKey={2}&resourceKey={3}",
                    PurviewUrl, userId, instanceKey, resourceKey));
                return InvokeResult<bool>(result);
            }
        }

        public static IList<bool> CheckResource(long userId, string instanceKey, IList<string> resourceKeys)
        {
            string strResourceKeys = string.Join("&", resourceKeys.Select(o => string.Format("resourceKeys={0}", o)));
            using (var wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                string result = wc.DownloadString(String.Format("{0}CheckActionss?userId={1}&instanceKey={2}&{3}",
                    PurviewUrl, userId, instanceKey, strResourceKeys));
                return InvokeResult<List<bool>>(result);
            }
        }

        public static bool CheckAccess(long userId, string instanceKey, string purviewKey, string resourceKey)
        {
            using (var wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                string result = wc.DownloadString(String.Format("{0}CheckAccess?userId={1}&instanceKey={2}&purviewKey={3}&resourceKey={4}",
                     PurviewUrl, userId, instanceKey, purviewKey, resourceKey));
                return InvokeResult<bool>(result);
            }
        }

        public static IList<bool> CheckAccess(long userId, string instanceKey, IList<PurviewResourcePair> pairs)
        {
            string strPair = string.Join("&", pairs.Select((o, i) => string.Format("pairs[{2}].PurviewKey={0}&pairs[{2}].ResourceKey={1}", o.PurviewKey, o.ResourceKey, i)));
            using (var wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                string result = wc.DownloadString(String.Format("{0}CheckAccesss?userId={1}&instanceKey={2}&{3}",
                    PurviewUrl, userId, instanceKey, strPair));
                return InvokeResult<List<bool>>(result);
            }
        }

        private static T InvokeResult<T>(string message)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Result<T> result = serializer.Deserialize<Result<T>>(message);
            if(result.Code != 0)
                throw new Exception(result.Code + ":" + result.Message);
            return result.Data;
        }

        private class Result<T>
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }

        public class PurviewResourcePair
        {
            public string PurviewKey { get; set; }
            public string ResourceKey { get; set; }
        }
    }
}
