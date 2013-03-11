using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Threading;
using Projects.Tool.Lock;

namespace Projects.Tool.RedisProvider
{
    public class RedisLock : ILock, IDisposable
    {
        IRedisClient redisClient;
        IDisposable redisLocker;
        bool lockTaken;
        static string connName;
        static Dictionary<string, object> lockDict = new Dictionary<string, object>();

        static RedisLock()
        {
            string conn = ToolSection.Instance.TryGetValue("synclock/redisProvider/connectionName");
            if (String.IsNullOrEmpty(conn))
                throw new MissConfigurationException(new List<SettingNode>() { ToolSection.Instance.RootNode }, "synclock/redisProvider/connectionName");

            connName = conn;
        }

        public bool AcquireLock(string key, int timeout)
        {
            var locker = GetLocker(key);
            Monitor.TryEnter(locker, TimeSpan.FromSeconds(timeout), ref lockTaken);
            //进程锁获取失败
            if (!lockTaken)
                return false;

            try
            {
                redisClient = RedisClientProvider.CreateRedisClient(connName);
                redisLocker = redisClient.AcquireLock(key, TimeSpan.FromSeconds(timeout));
                return redisLocker != null;
            }
            catch (TimeoutException ex)
            {
                //redis锁获取失败
                if (lockTaken)
                    Monitor.Exit(locker);

                if (redisLocker != null)
                    redisLocker.Dispose();

                return false;
            }
            catch (Exception ex)
            {
                //redis锁获取失败
                if (lockTaken)
                    Monitor.Exit(locker);

                if (redisLocker != null)
                    redisLocker.Dispose();

                throw ex;
            }

        }

        public void ReleaseLock(string key)
        {
            if (redisLocker != null)
                redisLocker.Dispose();

            var locker = GetLocker(key);

            if (lockTaken)
                Monitor.Exit(locker);

            RemoveLocker(key);
        }


        void IDisposable.Dispose()
        {
            if (redisClient != null)
                redisClient.Dispose();
        }

        object GetLocker(string key)
        {
            object locker = null;
            if (!lockDict.TryGetValue(key, out locker))
            {
                lock (lockDict)
                {
                    if (!lockDict.TryGetValue(key, out locker))
                    {
                        locker = new object();
                        lockDict[key] = locker;
                    }
                }
            }
            return locker;
        }

        void RemoveLocker(string key)
        {
            lock (lockDict)
            {
                lockDict.Remove(key);
            }
        }
    
    }
}
