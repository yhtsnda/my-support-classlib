using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Threading;
using Avalon.Lock;
using Avalon.Utility;
using Avalon.Access;

namespace Avalon.RedisProvider
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

        public bool AcquireLock(string key, int millisecondsTimeout)
        {
            var locker = GetLocker(key);

            //先获取本地锁
            lockTaken = System.Threading.Monitor.TryEnter(locker, millisecondsTimeout);
            if (!lockTaken)
                return false;

            try
            {
                redisClient = RedisManager.Instance.CreateRedisClient(connName);
                //redis获取不到锁将抛出异常
                redisLocker = new InnerRedisLock(redisClient, key, TimeSpan.FromMilliseconds(millisecondsTimeout)); //redisClient.AcquireLock(key, TimeSpan.FromMilliseconds(millisecondsTimeout));
                return true;
            }
            catch (Exception ex)
            {
                System.Threading.Monitor.Exit(locker);

                if (redisLocker != null)
                    redisLocker.Dispose();
            }
            return false;
        }

        public void ReleaseLock(string key)
        {
            if (redisLocker != null)
                redisLocker.Dispose();

            var locker = GetLocker(key);

            if (lockTaken)
                System.Threading.Monitor.Exit(locker);

            RemoveLocker(key);
            Dispose();
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

        public void Dispose()
        {
            if (redisClient != null)
            {
                redisClient.Dispose();
                redisClient = null;
            }
        }
    }
}
