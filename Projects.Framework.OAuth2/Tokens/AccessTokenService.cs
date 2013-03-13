using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Projects.Tool;

namespace Projects.Framework.OAuth2
{
    public class AccessTokenService : IService
    {
        private static CacheDomain<AccessToken, string> cacheAccessToken =
            CacheDomain.CreateSingleKey<AccessToken, string>(o => o.Token, 
            GetAccessTokenInner, null, 
            "accessToken", 
            "accessToken:{0}");

        private static IAccessTokenRepository repository = DependencyResolver.Resolve<IAccessTokenRepository>();
        /// <summary>
        /// accessToken的过期时间
        /// </summary>
        public const int EXPIRE_TIME = 7200;

        private static ISpecification<AccessToken> GetSpecification()
        {
            return repository.CreateSpecification();
        }

        public static void CreateToken(AccessToken token)
        {
            repository.Create(token);
        }

        public static void UpdateToken(AccessToken token)
        {
            repository.Update(token);
            cacheAccessToken.RemoveCache(token.Token);
        }

        /// <summary>
        /// 生成一个token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static AccessToken CreateAccessToken(int userId, int clientId)
        {
            var token = GetTokenBy(clientId, userId);
            if (token == null)
            {
                token = new AccessToken
                {
                    ClientId = clientId,
                    UserId = userId,
                    Token = Guid.NewGuid().ToString("N"),
                    IssueTime = NetworkTime.Now,
                    Expire = EXPIRE_TIME
                };

                CreateToken(token);
            }
            else
            {
                token.IssueTime = NetworkTime.Now;
                UpdateToken(token);
            }

            return token;
        }

        /// <summary>
        /// 延长accessToken的有效时间
        /// </summary>
        /// <param name="accessToken"></param>
        public static void UpdateIssueDate(string accessToken)
        {
            var token = GetTokenBy(accessToken);
            token.IssueTime = NetworkTime.Now;
            UpdateToken(token);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="accessToken"></param>
        public static void Logout(string accessToken)
        {
            var token = GetTokenBy(accessToken);
            token.IssueTime = NetworkTime.Now.AddDays(-1);
            UpdateToken(token);
        }

        /// <summary>
        /// 通过accessToken获取userId
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static int GetUserIdByAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ApiException("accessToken为空", ResultCode.Token_IsNull);

            var token = GetTokenBy(accessToken);
            //检查token是否存在
            if (token == null)
                throw new ApiException("不存在该accessToken：" + accessToken, ResultCode.Token_NotExist);
            //检查token是否过期
            if (NetworkTime.Now > token.IssueTime.AddSeconds(token.Expire))
                throw new ApiException("该accessToken已经过期", ResultCode.Token_Expired);
            //超过一半，自动续期
            if (NetworkTime.Now >= token.IssueTime.AddSeconds(token.Expire / 2))
            {
                token.IssueTime = NetworkTime.Now;
                UpdateToken(token);
            }

            return token.UserId;
        }

        public static AccessToken GetTokenBy(string accessToken)
        {
            return cacheAccessToken.GetItem(accessToken);
        }

        private static AccessToken GetTokenBy(int clientId, int userId)
        {
            var spec = GetSpecification().Where(o => o.ClientId == clientId && o.UserId == userId);
            return repository.FindOne(spec);
        }

        private static AccessToken GetAccessTokenInner(string accessToken)
        {
            var spec = GetSpecification().Where(o => o.Token == accessToken);
            return repository.FindOne(spec);
        }
    }
}
