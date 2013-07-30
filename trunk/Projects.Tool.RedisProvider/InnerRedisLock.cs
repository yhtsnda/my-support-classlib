using System;
using ServiceStack.Redis;
using ServiceStack.Common;

namespace Projects.Tool.RedisProvider
{
    internal class InnerRedisLock : IDisposable
    {
        private readonly IRedisClient redisClient;
        private readonly string key;

        public InnerRedisLock(IRedisClient redisClient, string key, TimeSpan timeOut)
        {
            this.redisClient = redisClient;
            this.key = key;

            ExecExtensions.RetryUntilTrue(() =>
            {
                var realSpan = timeOut == TimeSpan.Zero ? new TimeSpan(365, 0, 0, 0) : timeOut; //if nothing is passed in the timeout hold for a year
                var expireTime = NetworkTime.Now.Add(realSpan);
                var lockString = ((long)(expireTime.ToUnixTime() + 1)).ToString();
                var nx = redisClient.SetEntryIfNotExists(key, lockString);
                if (nx)
                    return true;

                var lockExpireString = redisClient.Get<string>(key);
                long lockExpireTime;
                if (!long.TryParse(lockExpireString, out lockExpireTime))
                    return false;
                if (lockExpireTime > NetworkTime.Now.ToUnixTime())
                    return false;

                return redisClient.GetAndSetEntry(key, lockString) == lockExpireString;
            }, timeOut
            );
        }

        public void Dispose()
        {
            redisClient.Remove(key);
        }
    }
}
