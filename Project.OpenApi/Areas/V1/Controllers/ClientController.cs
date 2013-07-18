using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Projects.Framework;
using Projects.Framework.Web;
using Projects.UserCenter;

namespace Projects.OpenApi.Areas.V1.Controllers
{
    [OpenApi, OAuthAuthorize]
    public class ClientController : Controller
    {
        private ClientAppService clientAppService;

        public ClientController(ClientAppService clientAppService)
        {
            this.clientAppService = clientAppService;
        }

        /// <summary>
        /// 保存应用信息
        /// </summary>
        /// <remarks>/v1/client/save</remarks>
        public object SaveClientApp()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 启用应用
        /// </summary>
        /// <remarks>/v1/client/enabled</remarks>
        public object EnabledClientApp(int clientId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 禁用应用
        /// </summary>
        /// <remarks>/v1/client/disabled</remarks>
        public object DisabledClientApp(int clientId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 限制应用
        /// </summary>
        /// <remarks>/v1/client/limited</remarks>
        public object LimitedClientApp(int clientId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取应用
        /// </summary>
        /// <remarks>/v1/client/get</remarks>
        public object GetClientApp(int clientId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取应用列表
        /// </summary>
        /// <remarks>/v1/client/list</remarks>
        public object GetClientAppList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 刷新应用秘钥
        /// </summary>
        /// <remarks>/v1/client/refresh</remarks>
        public object RefreshClientAppSecret(int clientId)
        {
            throw new NotImplementedException();
        }
    }
}
