using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingSiteCheck.Resource
{
    internal class ResourceCollector : IResourceCollector
    {
        private static readonly string JS_HTML_TAG = "<script type=\"text/javascript\" src=\"{0}\"></script>";
        private static readonly string CSS_HTML_TAG = "<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" />";

        /// <summary>
        /// 随机数生成器
        /// </summary>
        private static Random mRandom = new Random();
        private List<string> mResourceList = new List<string>();


        public void Add(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            mResourceList.Add(path);
        }

        private ResourceType mResourceType = ResourceType.Undefined;
        public ResourceType ResourceType
        {
            get { return mResourceType; }
            set { mResourceType = value; }
        }

        private bool debug = GlobalConfig.Debug;
        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        public string Group { get; set; }

        public string BaseFolder { get; set; }

        public string BuildHtmlTag()
        {
            //没有添加资源，返回空
            if (mResourceList.Count == 0 && string.IsNullOrEmpty(Group))
                return string.Empty;
            //资源类型未定义，自动识别
            if (mResourceType == ResourceType.Undefined)
                mResourceType = GetResourceType();
            //无法确认资源类型，返回空
            if (mResourceType == ResourceType.Undefined)
                return string.Empty;

            if (Debug)
                return BuildDebugResourcePath();
            else
                return BuildResourcePath();
        }

        private ResourceType GetResourceType()
        {
            ResourceType resourceType = ResourceType.Undefined;
            if (mResourceList.Count > 0 && resourceType == ResourceType.Undefined)
            {
                if (System.IO.Path.GetExtension(mResourceList[0]).ToLower().Equals(".js"))
                    resourceType = ResourceType.JS;
                else
                    resourceType = ResourceType.CSS;
            }
            return resourceType;
        }

        private string BuildDebugResourcePath()
        {
            int serverIndex = mRandom.Next(0, GlobalConfig.ServerList.Count);
            StringBuilder urlBuilder = new StringBuilder();
            if (mResourceType == ResourceType.JS)
            {
                foreach (string item in mResourceList)
                {
                    if (!string.IsNullOrEmpty(BaseFolder))
                        urlBuilder.AppendFormat(JS_HTML_TAG, string.Concat(GlobalConfig.ServerList[serverIndex], BaseFolder, "/", item, "?v=", GlobalConfig.Version));
                    else
                        urlBuilder.AppendFormat(JS_HTML_TAG, string.Concat(GlobalConfig.ServerList[serverIndex], item, "?v=", GlobalConfig.Version));
                    urlBuilder.Append(Environment.NewLine);
                }
            }
            else
            {
                foreach (string item in mResourceList)
                {
                    if (!string.IsNullOrEmpty(BaseFolder))
                        urlBuilder.AppendFormat(CSS_HTML_TAG, string.Concat(GlobalConfig.ServerList[serverIndex], BaseFolder, "/", item, "?v=", GlobalConfig.Version));
                    else
                        urlBuilder.AppendFormat(CSS_HTML_TAG, string.Concat(GlobalConfig.ServerList[serverIndex], item, "?v=", GlobalConfig.Version));
                    urlBuilder.Append(Environment.NewLine);
                }
            }
            return urlBuilder.ToString();
        }

        private string BuildResourcePath()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            string resourcePath = GetMergeResourcePath();
            if (mResourceType == ResourceType.JS)
                htmlBuilder.AppendFormat(JS_HTML_TAG, resourcePath);
            else
                htmlBuilder.AppendFormat(CSS_HTML_TAG, resourcePath);
            return htmlBuilder.ToString();
        }

        private string GetMergeResourcePath()
        {
            string serverUrl = string.Empty;
            if (GlobalConfig.ServerList.Count > 0)
            {
                int serverIndex = mRandom.Next(0, GlobalConfig.ServerList.Count);
                serverUrl = GlobalConfig.ServerList[serverIndex];
            }
            string url = string.Empty;
            if (!string.IsNullOrEmpty(Group))
                url = string.Concat(serverUrl, "?g=", Group);
            else if (!string.IsNullOrEmpty(BaseFolder))
                url = string.Concat(serverUrl, "?b=", BaseFolder, "&f=", string.Join(",", mResourceList.ToArray()));
            else
                url = string.Concat(serverUrl, "?f=", string.Join(",", mResourceList.ToArray()));

            url += "&v=" + GlobalConfig.Version;
            //if (Debug)
            //    url += "&debug=1";
            return url;
        }

    }
}
