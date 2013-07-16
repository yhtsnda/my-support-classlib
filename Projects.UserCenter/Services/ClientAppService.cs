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
        }

        /// <summary>
        /// 将制定的应用无效化
        /// </summary>
        /// <param name="clientId">应用ID</param>
        public void DisabledClientApp(int clientId)
        {
        }

        /// <summary>
        /// 将指定的应用有效
        /// </summary>
        /// <param name="clientId">应用ID</param>
        public void EnabledClientApp(int clientId)
        {

        }

        /// <summary>
        /// 将指定的应用设置为限制
        /// </summary>
        /// <param name="clientId">应用ID</param>
        public void LimitedClientApp(int clientId)
        {

        }

        /// <summary>
        /// 刷新应用的应用秘钥
        /// </summary>
        /// <param name="clientId">应用ID</param>
        /// <returns>新的应用秘钥</returns>
        public string RefreshClientAppSecrect(int clientId)
        {
        }

        public ClientApp GetClientApp(int clientId)
        {

        }

        public ClientApp GetClientAppByName(string appName)
        {

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
