using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 提供工作台临时对象访问的工具
    /// </summary>
    public class WorkbenchUtil<TKey, TValue>
    {
        public static bool Contains(string dicKey)
        {
            return Workbench.Current.Items.Contains(dicKey);
        }

        public static Dictionary<TKey, TValue> GetDictionary(string dicKey)
        {
            Dictionary<TKey, TValue> dic = (Dictionary<TKey, TValue>)Workbench.Current.Items[dicKey];
            if (dic == null)
            {
                dic = new Dictionary<TKey, TValue>();
                Workbench.Current.Items[dicKey] = dic;
            }
            return dic;
        }

        public static TValue GetValue(string dicKey, TKey dataKey)
        {
            return GetDictionary(dicKey).TryGetValue(dataKey);
        }

        public static void SetValue(string dicKey, TKey dataKey, TValue dataValue)
        {
            GetDictionary(dicKey)[dataKey] = dataValue;
        }

        public static IEnumerable<TKey> GetKeys(string dicKey)
        {
            return GetDictionary(dicKey).Keys.ToList();
        }

        public static IEnumerable<TValue> GetValues(string dicKey)
        {
            return GetDictionary(dicKey).Values.ToList();
        }

        public static void Clear(string dicKey)
        {
            GetDictionary(dicKey).Clear();
        }
    }
}
