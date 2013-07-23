using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Projects.Framework;
using Projects.Framework.Web;
using Projects.UserCenter;
using Projects.OAuth;

namespace Projects.OpenApi.Areas.V1.Controllers
{
    [OpenApi, OAuthAuthorize, AppValidate]
    public class UserController : Controller
    {
        private UserService userService;
        private OAuthService oauthService;

        public UserController(UserService userService, OAuthService oauthService)
        {
            this.userService = userService;
            this.oauthService = oauthService;
        }

        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/reg</remarks>
        [HttpPost]
        [CustomActionName("reg")]
        public object Register(UserRegisterModel model, int appId)
        {
            if (String.IsNullOrEmpty(model.IpAddress))
                model.IpAddress = Projects.Tool.Http.IpAddress.GetIP();
            var result = userService.Register(model);
            if (result.Code == RegisterResultCode.Success)
                return oauthService.CreateServerAccessGrant(appId, result.Data.UserId).ToModel();
            return new OpenApiDataResult(new PlatformException(result.Message), (int)result.Code);
        }

        /// <summary>
        /// 来自第三方的注册
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/reg/third</remarks>
        [HttpPost]
        [CustomActionName("reg/third")]
        public object RegisterFromThrid(MappingRegisterModel model, int appId)
        {
            if (String.IsNullOrEmpty(model.IpAddress))
                model.IpAddress = Projects.Tool.Http.IpAddress.GetIP();
            var result = userService.RegisterWithMapping(model);
            if (result.Code == RegisterResultCode.Success)
                return oauthService.CreateServerAccessGrant(appId, result.Data.UserId).ToModel();
            return new OpenApiDataResult(new PlatformException(result.Message), (int)result.Code);
        }

        /// <summary>
        /// 用户登录
        /// </summary> 
        /// <remarks>/v1/{app}/{terminal}/{source}/user/login</remarks>
        [HttpGet]
        [CustomActionName("login")]
        public object Login(string userName, string password, int appId)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException("用户名不能为空");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("用户密码不能为空");
            //尝试登陆
            var result = userService.Login(userName, password);
            //登陆成功
            if (result.Code == LoginResultCode.Success)
            {
                var code = oauthService.CreateAuthorizationCode(appId, result.Data.UserId);
                return code;
            }
            return new OpenApiDataResult(new PlatformException(result.Message, (int)result.Code));
        }

        /// <summary>
        /// 第三方用户登录
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/login/third</remarks>
        [HttpGet]
        [CustomActionName("login/third")]
        public object LoginWithMapping()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将AuthoCode转换为AccessToken
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/exchange/accesstoken</remarks>
        [HttpGet]
        [CustomActionName("exchange/accesstoken")]
        public object ExchangeAccessToken()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 验证用户名是否存在
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/valid/name</remarks>
        [HttpGet]
        [CustomActionName("valid/name")]
        public object ValidByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据用户的ID获取用户的信息
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/get</remarks>
        [HttpGet]
        [CustomActionName("get")]
        public object GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据用户名获取用户的信息
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/get/name</remarks>
        [HttpGet]
        [CustomActionName("get/name")]
        public object GetUserByName(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据映射用户名获取用户的信息
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/get/mapping</remarks>
        [HttpGet]
        [CustomActionName("get/mapping")]
        public object GetUserByMapping(string key, MappingType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据用户ID列表获取用户信息
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/list</remarks>
        [HttpPost]
        [CustomActionName("list")]
        public object GetUserList(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据用户名列表获取用户信息
        /// </summary>
        /// <remarks>/v1/{app}/{terminal}/{source}/user/list/name</remarks>
        [HttpPost]
        [CustomActionName("list/name")]
        public object GetUserListByName(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}
