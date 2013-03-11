using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projects.Tool
{
    public class StaticCache : AbstractCache
    {
        static Hashtable staticDic = Hashtable.Synchronized(new Hashtable());

        protected override void SetInner<T>(string key, T value, DateTime expiredTime)
        {
            staticDic[key] = value;
        }

        protected override T GetInner<T>(string key)
        {
            var data = staticDic[key];
            if (data == null)
                return default(T);

            return (T)data;
        }

        protected override void RemoveInner(string key)
        {
            staticDic.Remove(key);
        }
    }
}
