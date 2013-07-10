using Projects.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    public class LogService : IService
    {
        private ILoginLogRepository loginLogRepository;
        private ILoginStateLogRepository loginStateRepository;
        private IRegisterLogRepository registerLogRepository;

        public LogService(ILoginLogRepository loginLogRepository,
            ILoginStateLogRepository loginStateRepository,
            IRegisterLogRepository registerLogRepository)
        {
            this.loginLogRepository = loginLogRepository;
            this.loginStateRepository = loginStateRepository;
            this.registerLogRepository = registerLogRepository;
        }

        /// <summary>
        /// 保存注册日志
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="regPlatCode">注册平台</param>
        /// <param name="autoLogin">注册后是否自动登录</param>
        public void SaveRegisterLog(int userId, string userName, long regPlatCode, bool autoLogin = false)
        {
            RegisterLog log = new RegisterLog(userId, userName, regPlatCode);
            registerLogRepository.Create(log);

            if (autoLogin)
                SaveLoginLog(userId, userName, regPlatCode);
        }

        /// <summary>
        /// 保存登录日志
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="regPlatCode">注册平台</param>
        public void SaveLoginLog(int userId, string userName, long regPlatCode)
        {
            LoginLog login = new LoginLog(userId, userName, regPlatCode);
            loginLogRepository.Create(login);

            var stateLog = loginStateRepository.Get(userId);
            if (stateLog == null)
            {
                stateLog = new LoginStateLog(userId);
                loginStateRepository.Create(stateLog);
            }
            else
            {
                stateLog.ChangeLoginState();
                loginStateRepository.Update(stateLog);
            }
        }
    }
}
