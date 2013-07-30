using Projects.Tool.Diagnostics;
using Projects.Tool.Util;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Tool.RedisProvider
{
    public class RedisHttpCommand : HttpCommand, IWorkbenchModule
    {
        Dictionary<string, Type> dic;
        object syncObj = new object();

        public override string BathPath
        {
            get { return "redis"; }
        }

        public override string HelpContent
        {
            get
            {
                return @"
get key         Get the value of a key
dbsize          Return the number of keys in the selected database
keys pattern    Find all keys matching the given pattern
hkeys key       Get all the fields in a hash
hlen key        Get the number of fields in a hash
hgetall key     Get all the fields and values in a hash
eget type id    Get entity by id
edel type id    Del entity by id
";
            }
        }

        public override void InitCommands()
        {
            RegisterCommand("eget", ProcessCommandEGet);
            RegisterCommand("edel", ProcessCommandEDel);
            RegisterCommand("get", (value, app) => Exec((client) => client.GetValue(value), app));
            RegisterCommand("dbsize", (value, app) => Exec((client) => client.DbSize.ToString(), app));
            RegisterCommand("keys", (value, app) => Exec((client) => ListToString(client.SearchKeys(value)), app));
            RegisterCommand("hkeys", (value, app) => Exec((client) => ListToString(client.GetHashKeys(value)), app));
            RegisterCommand("hlen", (value, app) => Exec((client) => client.GetHashCount(value).ToString(), app));
            RegisterCommand("hgetall", (value, app) => Exec((client) => DictionaryToString(client.GetAllEntriesFromHash(value)), app));
        }

        public override void InitRequests()
        {
        }

        protected override void ProcessCommandOptions(HttpApplication app)
        {
        }

        public void BeginRequest(HttpApplication app)
        {
            Process(app);
        }

        public void EndRequest(HttpApplication app)
        {
        }

        void ProcessCommandEGet(string value, HttpApplication app)
        {
            ICache cache;
            string key;
            Type type;
            if (ProcessCommandForEntity(value, app, out cache, out type, out key))
            {
                var entity = cache.Get(type, key);
                WriteContent(JsonConverter.ToJson(entity), app);
            }
        }

        void ProcessCommandEDel(string value, HttpApplication app)
        {
            ICache cache;
            string key;
            Type type;
            if (ProcessCommandForEntity(value, app, out cache, out type, out key))
            {
                cache.Remove(type, key);
            }
        }

        void ProcessCommandGet(string value, HttpApplication app)
        {

        }

        bool ProcessCommandForEntity(string value, HttpApplication app, out ICache cache, out Type type, out string key)
        {
            var vs = value.Split(' ');
            cache = null;
            key = null;
            type = GetType(vs[0]);
            if (type == null)
            {
                WriteContent("无法获取类型 " + type, app);
            }
            else
            {
                cache = CacheManager.GetCacher(type);
                if (cache is RedisCache || cache is RedisHashCache)
                {
                    key = "#" + type.FullName + ":" + vs[1];
                    return true;
                }
                else
                {
                    WriteContent(String.Format("类型 {0} 的缓存类型为 {1}", type.FullName, cache.GetType()), app);
                }
            }
            return false;
        }

        void Exec(Func<IRedisClient, string> action, HttpApplication app)
        {
            using (var client = CreateRedisClient())
            {
                try
                {
                    var value = action(client);
                    if (value == null)
                        value = "(nil)";
                    WriteContent(value, app);
                }
                catch (Exception ex)
                {
                    WriteContent(ex.Message, app);
                }
            }
        }

        string ListToString(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list == null || list.Count == 0)
            {
                sb.Append("(empty list or set)");
            }
            else
            {
                var count = list.Count;
                var pad = count.ToString().Length;
                for (var i = 0; i < count; i++)
                {
                    if (i != 0)
                        sb.AppendLine();
                    sb.AppendFormat("{0}) {1}", (i + 1).ToString().PadLeft(pad), HttpUtility.HtmlEncode(list[i]));
                }
            }
            return sb.ToString();
        }

        string DictionaryToString(Dictionary<string, string> dic)
        {
            var keyPad = dic.Keys.Max(o => o.Length);
            var numPad = dic.Count.ToString().Length;
            StringBuilder sb = new StringBuilder();
            if (dic == null || dic.Count == 0)
            {
                sb.Append("(empty list or set)");
            }
            else
            {
                var index = 0;
                foreach (var item in dic)
                {
                    if (index > 0)
                        sb.AppendLine();
                    sb.AppendFormat("{0}) {1}   {2}", (index + 1).ToString().PadLeft(numPad), item.Key.PadRight(keyPad), HttpUtility.HtmlEncode(item.Value));
                    index++;
                }
            }
            return sb.ToString();
        }

        IRedisClient CreateRedisClient()
        {
            return RedisClientProvider.CreateRedisClient("redisconn");
        }

        Type GetType(string name)
        {
            if (dic == null)
            {
                lock (syncObj)
                {
                    if (dic == null)
                    {
                        dic = new Dictionary<string, Type>(StringComparer.CurrentCultureIgnoreCase);
                        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            if (!assembly.IsDynamic && assembly.FullName.StartsWith("Nd"))
                            {
                                foreach (var type in assembly.GetExportedTypes())
                                {
                                    if (type.IsClass && !type.IsAbstract)
                                        dic[type.Name] = type;
                                }
                            }
                        }
                    }
                }
            }
            var output = dic.TryGetValue(name);
            if (output == null)
                output = Type.GetType(name);
            return output;
        }

        public static void Setup()
        {
            WorkbenchModule.Register(typeof(RedisHttpCommand));
        }

        public void Init()
        {
            InitCommands();
            InitRequests();
        }
    }
}
