using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Projects.Framework;
using Projects.Framework.Web;
using Projects.UserCenter;
using Projects.OpenApi.Areas.V1.Models;

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
        [HttpPost]
        public object SaveClientApp(ClientAppModel model)
        {
            var client = clientAppService.GetClientApp(model.Code);
            if (client != null)
                throw new ArgumentException("client app has been exists!");

            client = clientAppService.GetClientAppByName(model.Title);
            if (client != null)
                throw new ArgumentException("client app title has been exists!");

            client = new ClientApp(model.Code, model.Title, model.Description);
            clientAppService.CreateClientApp(client);

            return new
            {
                Code = client.Code,
                Title = client.Title,
                Desc = client.Description,
                Secret = client.SecretCode
            };
        }

        /// <summary>
        /// 启用应用
        /// </summary>
        /// <remarks>/v1/client/enabled</remarks>
        [HttpGet]
        public object EnabledClientApp(int clientId)
        {
            var client = clientAppService.GetClientApp(clientId);
            if(client == null)
                throw new ArgumentException("client app not exists!");

            client.SwitchClientApp(ClientAppStatus.Enabled);
            clientAppService.ModifyClientAppInfo(client);

            return new
            {
                Code = client.Code,
                Status = client.Status
            };
        }

        /// <summary>
        /// 禁用应用
        /// </summary>
        /// <remarks>/v1/client/disabled</remarks>
        [HttpGet]
        public object DisabledClientApp(int clientId)
        {
            var client = clientAppService.GetClientApp(clientId);
            if (client == null)
                throw new ArgumentException("client app not exists!");

            client.SwitchClientApp(ClientAppStatus.Disabled);
            clientAppService.ModifyClientAppInfo(client);

            return new
            {
                Code = client.Code,
                Status = client.Status
            };
        }

        /// <summary>
        /// 限制应用
        /// </summary>
        /// <remarks>/v1/client/limited</remarks>
        [HttpGet]
        public object LimitedClientApp(int clientId)
        {
            var client = clientAppService.GetClientApp(clientId);
            if (client == null)
                throw new ArgumentException("client app not exists!");

            client.SwitchClientApp(ClientAppStatus.Limited);
            clientAppService.ModifyClientAppInfo(client);

            return new
            {
                Code = client.Code,
                Status = client.Status
            };
        }

        /// <summary>
        /// 获取应用
        /// </summary>
        /// <remarks>/v1/client/get</remarks>
        [HttpGet]
        public object GetClientApp(int clientId)
        {
            var client = clientAppService.GetClientApp(clientId);
            if (client == null)
                throw new ArgumentException("client app not exists!");

            return new
            {
                Code = client.Code,
                Title = client.Title,
                Desc = client.Description,
                Secret = client.SecretCode,
                Status = client.Status
            };
        }

        /// <summary>
        /// 获取应用列表
        /// </summary>
        /// <remarks>/v1/client/list</remarks>
        [HttpGet]
        public object GetClientAppList(bool showAll = false)
        {
            var list = clientAppService.GetClientAppList(showAll ? false : true);
            return list.Select(item => new
            {
                Code = item.Code,
                Title = item.Title,
                Desc = item.Description,
                Secret = item.SecretCode,
                Status = item.Status
            });
        }

        /// <summary>
        /// 刷新应用秘钥
        /// </summary>
        /// <remarks>/v1/client/refresh</remarks>
        [HttpGet]
        public object RefreshClientAppSecret(int clientId)
        {
            var client = clientAppService.GetClientApp(clientId);
            if (client == null)
                throw new ArgumentException("client app not exists!");

            if (client.Status == ClientAppStatus.Disabled)
                throw new ArgumentException("client app has been disabled, can't change secret code");

            client.ChangeSecretCode();
            clientAppService.ModifyClientAppInfo(client);

            return new
            {
                Code = client.Code,
                Secret = client.SecretCode
            };
        }
    }
}
