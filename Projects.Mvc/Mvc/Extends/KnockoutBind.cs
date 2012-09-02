using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace System.Web.Mvc
{
    public class KnockoutBind : Dictionary<string, object>
    {
        private const string DATA_BIND_KEY = "data-bind";

        public KnockoutBind(object binds, object htmlAttributes)
        {
            ProcessBind(binds);
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    var key = descriptor.Name.Replace('_', '-');
                    if (key == DATA_BIND_KEY)
                        MergeBind(descriptor.GetValue(htmlAttributes));
                    else
                        Add(key, descriptor.GetValue(htmlAttributes));
                }
            }
        }

        public KnockoutBind(object binds) : this(binds, null)
        {

        }

        public KnockoutBind(object binds, IDictionary<string,object> htmlAttributes)
        {
            ProcessBind(binds);
            if (htmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> pair in htmlAttributes)
                {
                    var key = pair.Key.Replace('_', '-');
                    if (key == DATA_BIND_KEY)
                        MergeBind(pair.Value);
                    else
                        Add(key, pair.Value);
                }
            }
        }

        public void AddBind(string bindName, object bindValue)
        {
            MergeBind(bindName + ": " + bindValue.ToString());
        }

        private void MergeBind(object bindValue)
        {
            var bindString = this.TryGetValue(DATA_BIND_KEY);
            if (bindString == null)
                Add(DATA_BIND_KEY, bindValue);
            else
                this[DATA_BIND_KEY] = bindString + ", " + bindValue.ToString();
        }

        private void ProcessBind(object binds)
        {
            if (binds != null)
            {
                List<string> strs = new List<string>();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(binds))
                {
                    strs.Add(descriptor.Name + ": " + descriptor.GetValue(binds).ToString());
                }
                if (strs.Count > 0)
                    MergeBind(String.Join(", ", strs));
            }
        }
    }
}
