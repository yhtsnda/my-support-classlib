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

        [OpenApi]
        public object Register(UserRegisterModel model)
        {
            throw new NotImplementedException();
        }

        [OpenApi]
        public object RegisterFromThrid(MappingRegisterModel model)
        {
            throw new NotImplementedException();
        }

        public object 
    }
}
