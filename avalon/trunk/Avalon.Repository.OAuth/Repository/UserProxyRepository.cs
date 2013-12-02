using Avalon.Framework;
using Avalon.OAuth;
using Avalon.UCenter;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Repository.OAuth.Repository
{
    public class UserProxyRepository : AbstractNoShardRepository<UserProxy>, IUserProxyRepository
    {
        UserService userService;

        UserService UserService
        {
            get
            {
                if (userService == null)
                    userService = Avalon.Framework.DependencyResolver.Resolve<UserService>();
                return userService;
            }
        }

        public virtual ValidResult ValidPassword(string userName, string password, long platCode, string browser = null, string ipAddress = null, string extendFiled = null)
        {
            var login = new UserLogin()
            {
                UserName = userName,
                Password = password,
                PlatCode = platCode,
                Browser = browser,
                IpAddress = ipAddress,
                ExtendField = extendFiled
            };
            var result = UserService.TryLogin(login);
            return ToResult(result);
        }

        public virtual ValidResult ValidThirdToken(string accessToken, int mappingType, long platCode, string browser = null, string ipAddress = null, string extendFiled = null)
        {
            var login = new ThirdLogin()
            {
                AccessToken = accessToken,
                MappingType = (MappingType)mappingType,
                PlatCode = platCode,
                Browser = browser,
                IpAddress = ipAddress,
                ExtendField = extendFiled
            };
            var result = UserService.TryLogin(login);
            return ToResult(result);
        }

        public virtual ValidResult ValidPassport91(long password91Id, string userName, string password, string cookieOrdernumberMaster, string cookieOrdernumberSlave, string cookieCheckcode, string cookieSiteflag, long platCode, string browser = null, string ipAddress = null, string extendFiled = null)
        {
            if (String.IsNullOrEmpty(ipAddress))
                ipAddress = "0.0.0.0";

            var login = new Passport91Login()
            {
                Passport91Id = password91Id,
                UserName = userName,
                Password = password,
                CookieOrdernumberMaster = cookieOrdernumberMaster,
                CookieOrdernumberSlave = cookieOrdernumberSlave,
                CookieCheckcode = cookieCheckcode,
                CookieSiteflag = cookieSiteflag,
                PlatCode = platCode,
                Browser = browser,
                IpAddress = ipAddress,
                ExtendField = extendFiled
            };

            var result = UserService.TryLogin(login);
            return ToResult(result);
        }

        ValidResult ToResult(ResultWrapper<LoginResultCode, UserInner> result)
        {
            return new ValidResult()
            {
                Code = (int)result.Code,
                Message = result.Message,
                UserId = result.Data.GetOrDefault(o => o.UserId)
            };
        }
    }
}
