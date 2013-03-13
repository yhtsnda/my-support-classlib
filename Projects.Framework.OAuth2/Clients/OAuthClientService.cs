using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Projects.Tool;

namespace Projects.Framework.OAuth2
{
    public class OAuthClientService : IService
    {
        //client缓存
        private static CacheDomain<OAuthClient, int> cacheOauthClient = CacheDomain.CreateSingleKey<OAuthClient, int>(o => o.Id, GetOauthClientInner, null, "oauthClient", "cache:oauthClient:{0}");

        private static IOAuthClientRepository repository = DependencyResolver.Resolve<IOAuthClientRepository>();

        private static ISpecification<OAuthClient> GetSpecification()
        {
            return repository.CreateSpecification();
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="id"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static OAuthClient GetClientBy(int id, string secret)
        {
            var client = cacheOauthClient.GetItem(id);
            if (client == null)
                return null;

            return client.Secret == secret ? client : null;
        }

        public static OAuthClient GetClientById(int id)
        {
            return cacheOauthClient.GetItem(id);
        }

        public static void Add(OAuthClient client)
        {
            var spec = GetSpecification().Where(o => o.Secret == client.Secret);
            var result = repository.FindAll(spec);
            if (result.Count > 0)
            {
                throw new ArgumentException("该Secret已经存在，请换一个");
            }
            repository.Create(client);
        }

        public static void Delete(int id)
        {
            var client = GetClientById(id);
            repository.Delete(client);
        }

        public static Guid GetSecret()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 获取所有的client
        /// </summary>
        /// <returns></returns>
        public static IList<OAuthClient> GetList()
        {
            return repository.FindAll(GetSpecification());
        }

        private static OAuthClient GetOauthClientInner(int id)
        {
            return repository.Get(id);
        }
    }
}
