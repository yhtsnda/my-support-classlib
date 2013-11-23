using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.Utility
{
    public static class UriPath
    {
        public static readonly char PathSeparatorChar = '/';

        public static string Combine(params string[] paths)
        {
            Arguments.NotNull(paths, "paths");

            List<string> items = new List<string>();
            for (var i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                Arguments.NotNull(path, "paths");

                path = path.Trim().Trim(new char[] { PathSeparatorChar });
                if (path.Length > 0)
                    items.Add(path);
            }
            return String.Join(PathSeparatorChar.ToString(), items);
        }

        public static string AppendArguments(string url, string key, object value)
        {
            string v = (string)Convert.ChangeType(value, typeof(string));
            return AppendArguments(url, key, v);
        }

        public static string AppendArguments(string url, string key, string value)
        {
            Arguments.NotNull(url, "url");
            Arguments.NotNull(key, "key");

            if (url.IndexOf("?") == -1)
                url += "?";
            else
                url += "&";

            url += key + "=" + HttpUtility.UrlEncode(value);
            return url;
        }

        public static string AppendArguments(string url, string key, IEnumerable items)
        {
            foreach (var item in items)
            {
                url = AppendArguments(url, key, item);
            }
            return url;
        }

        public static string AppendArguments(string url, object values)
        {
            return AppendArguments(url, ToNameValueCollection(values));
        }

        public static string AppendArguments(string url, NameValueCollection data)
        {
            Arguments.NotNull(url, "url");

            if (data != null && data.Count > 0)
            {
                if (url.IndexOf("?") == -1)
                    url += "?";
                else if (url.IndexOf("&") > -1)
                    url += "&";

                url += ConstructQueryString(data);
            }
            return url;
        }

        public static string ConstructQueryString(NameValueCollection data)
        {
            List<string> items = new List<string>();

            foreach (string name in data)
            {
                var values = data.GetValues(name);
                if (values != null)
                    items.AddRange(values.Select(o => String.Concat(name, "=", HttpUtility.UrlEncode(o))));
            }

            return String.Join("&", items);
        }

        public static NameValueCollection ToNameValueCollection(object values)
        {
            NameValueCollection data = new NameValueCollection();
            if (values != null)
            {
                var ta = TypeAccessor.GetAccessor(values.GetType());
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    var v = ta.GetPropertyGetter(descriptor.Name)(values);
                    if (v==null)                  
                        continue;
                  
                    string value = null;
                    if (descriptor.PropertyType == typeof(Guid))
                    {
                        value = v.ToString();
                    }
                    else
                    {
                        if (v !=null && v.GetType().GetInterface(typeof(ICollection).FullName) != null)
                        {
                            ICollection col = (ICollection)v;
                            int index = 0;
                            foreach (object item in col)
                            {
                                var ikey = descriptor.Name + "[" + index + "]";
                                var ivalue = (string)Convert.ChangeType(item, typeof(string));
                                data.Add(ikey,ivalue);
                                index++;
                            }
                            continue;
                        }
                        value = (string)Convert.ChangeType(v, typeof(string));
                    }
                    data.Add(descriptor.Name, value);
                }
            }
            return data;
        }
    }
}
