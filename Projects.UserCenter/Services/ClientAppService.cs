using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Framework;

namespace Projects.UserCenter
{
    /// <summary>
    /// 客户端服务
    /// </summary>
    public class ClientAppService
    {
        private IClientAppRepository clientRepository;

        public ClientAppService(IClientAppRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        /// <summary>
        /// 创建接入应用信息
        /// </summary>
        /// <param name="client">客户端信息</param>
        public ClientApp CreateClientApp(ClientApp client)
        {
            clientRepository.Create(client);
            return client;
        }

        /// <summary>
        /// 修改接入应用的信息
        /// </summary>
        /// <param name="client"></param>
        public void ModifyClientAppInfo(ClientApp client)
        {
            clientRepository.Update(client);
        }

        /// <summary>
        /// 刷新应用的应用秘钥
        /// </summary>
        /// <param name="clientId">应用ID</param>
        /// <returns>新的应用秘钥</returns>
        public string RefreshClientAppSecrect(int clientId)
        {
            var clientApp = clientRepository.Get(clientId);
            if (clientApp == null)
                throw new ArgumentNullException("指定的应用不存在");
            if (clientApp.Status == ClientAppStatus.Disabled)
                throw new ArgumentNullException("指定的应用已被禁用,无法修改秘钥");

            clientApp.ChangeSecretCode();
            clientRepository.Update(clientApp);
            return clientApp.SecretCode;
        }

        /// <summary>
        /// 获取应用信息
        /// </summary>
        /// <param name="clientId">应用ID</param>
        /// <returns>应用对象</returns>
        public ClientApp GetClientApp(int clientId)
        {
            return clientRepository.Get(clientId);
        }


        /// <summary>
        /// 获取应用信息
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <returns>应用对象</returns>
        public ClientApp GetClientAppByName(string appName)
        {
            var spec = clientRepository.CreateSpecification().Where(o => o.Title == appName);
            return clientRepository.FindOne(spec);
        }

        /// <summary>
        /// 获取接入的所有应用
        /// </summary>
        /// <param name="onlyEnabled">只获取启用应用</param>
        /// <returns>应用列表</returns>
        public IList<ClientApp> GetClientAppList(bool onlyEnabled = true)
        {
            var spec = clientRepository.CreateSpecification();
            if (onlyEnabled)
                return clientRepository.FindAll(spec.Where(o => o.Status == ClientAppStatus.Enabled));
            return clientRepository.FindAll(spec);
        }
    }
}
