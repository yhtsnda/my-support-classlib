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
            get { return Context.IsAvailable(); }
        }

        protected override void SetInner<T>(string key, T value, DateTime expiredTime)
        {
            if (Enabled)
                Context.Items[key] = value;
        }

        protected override void RemoveInner(Type type, string key)
        {
            if (Enabled)
                Context.Items.Remove(key);
        }

        protected override object GetInner(Type type, string key)
        {
            if (Enabled)
                return Context.Items[key];
            return null;
        }
    }
}
