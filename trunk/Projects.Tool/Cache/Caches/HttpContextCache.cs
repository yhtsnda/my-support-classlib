using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Tool
{
    public class HttpContextCache : AbstractCache
    {
        HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        bool Enabled
        {
            get { return Context != null; }
        }

        protected override void SetInner<T>(string key, T value, DateTime expiredTime)
        {
            if (Enabled)
                Context.Items[key] = value;
        }

        protected override T GetInner<T>(string key)
        {
            //Context.Items 为 hashtable
            if (Enabled)
            {
                var data = Context.Items[key];
                if (data == null)
                    return default(T);

                return (T)data;
            }
            return default(T);
        }

        protected override void RemoveInner(string key)
        {
            if (Enabled)
                Context.Items.Remove(key);
        }
    }
}
