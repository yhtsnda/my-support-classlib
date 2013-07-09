using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Projects.Framework;
using Projects.Framework.Web;
using Projects.UserCenter;

namespace Projects.OpenApi.Controllers.V1.User
{
    public class UserController : Controller
    {
        public UserController()
        {

        }

        [OpenApi, OAuthAuthorize, HttpPost]
        [CustomActionName("reg")]
        public object Register(UserRegisterModel model)
        {
            throw new NotImplementedException();
        }

        [OpenApi, HttpPost]
        [CustomActionName("reg/third")]
        public object RegisterFromThrid(MappingRegisterModel model)
        {
            throw new NotImplementedException();
        }

        [OpenApi, OAuthAuthorize, HttpGet]
        [CustomActionName("login")]
        public object Login()
        {
            throw new NotImplementedException();
        }

        [OpenApi, OAuthAuthorize, HttpGet]
        [CustomActionName("login/third")]
        public object LoginWithMapping()
        {
            throw new NotImplementedException();
        }

        [OpenApi, OAuthAuthorize, HttpGet]
        [CustomActionName("valid/name")]
        public object ValidByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        [OpenApi, OAuthAuthorize, HttpGet]
        [CustomActionName("get")]
        public object GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        [OpenApi, OAuthAuthorize, HttpGet]
        [CustomActionName("get/name")]
        public object GetUserByName(string userName)
        {
            throw new NotImplementedException();
        }

        [OpenApi, OAuthAuthorize, HttpPost]
        [CustomActionName("list")]
        public object GetUserList(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        [OpenApi, OAuthAuthorize, HttpPost]
        [CustomActionName("list/name")]
        public object GetUserListByName(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}
