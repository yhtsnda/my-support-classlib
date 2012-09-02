using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projects.Tool
{
    /// <summary>
    /// 静态缓存
    /// </summary>
    public class StaticCache : AbstractCache
    {
        private static Hashtable mStaticDic = Hashtable.Synchronized(new Hashtable());

        protected override void SetInner<T>(string key, T value, DateTime expiredTime)
        {
            mStaticDic[key] = value;
        }

        protected override T GetInner<T>(string key)
        {
            return (T) mStaticDic[key];
        }

        protected override void RemoveInner(string key)
        {
            mStaticDic.Remove(key);
        }
    }
}
