using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Utility;

namespace Avalon.UCenter
{
    public class LogService : IService
    {
        IActiveLogRepository activeLogRepository;
        IRegisterLogRepository registerLogRepository;
        ILoginLogRepository loginLogRepository;
        ILastLoginLogRepository lastLoginLogRepository;

        public LogService(IActiveLogRepository activeLogRepository, 
            IRegisterLogRepository registerLogRepository, 
            ILoginLogRepository loginLogRepository, 
            ILastLoginLogRepository lastLoginLogRepository)
        {
            this.activeLogRepository = activeLogRepository;
            this.registerLogRepository = registerLogRepository;
            this.loginLogRepository = loginLogRepository;
            this.lastLoginLogRepository = lastLoginLogRepository;
        }
        /// <summary>
        /// 一天仅更新一次
        /// </summary>
        void WriteLastLoginLog(long userId, string ip)
        {
            var lastLoginLog = lastLoginLogRepository.Get(userId);
            if (lastLoginLog == null)
            {
                lastLoginLog = new LastLoginLog()
                {
                    UserId = userId,
                    LastLoginIp = ip
                };
                lastLoginLogRepository.Create(lastLoginLog);
            }
            else
            {
                lastLoginLog.LastLoginIp = ip;
                lastLoginLog.LastLoginTime = NetworkTime.Now;
                lastLoginLogRepository.Update(lastLoginLog);
            }
        }
        /// <summary>
        /// 每 day + userid + ip + platcode 只有一笔数据
        /// </summary>
        public void WriteLoginLog(long userId, long platCode, string ip, string browser, string extendField)
        {
            var ipInt32 = UserUtil.GetIpInt(ip);

            //判断是否存在数据
            if (loginLogRepository.HasLog(userId, platCode, ipInt32))
                return;

            var loginLog = new LoginLog()
            {
                BrowserCode = browser,
                ExtendField = extendField,
                LoginIp = ip,
                LoginIPInt = UserUtil.GetIpInt(ip),
                ProductCode = UserUtil.GetProductCode(platCode),
                TerminalCode = UserUtil.GetTerminalCode(platCode),
                UserId = userId
            };
            loginLogRepository.Create(loginLog);
            WriteLastLoginLog(userId, ip);

            //写入存在数据
            loginLogRepository.SetLog(userId, platCode, ipInt32);
        }
        /// <summary>
        /// 每 userid + platcode 只有一笔数据
        /// </summary>
        public void WriteActiveLog(long userId, long activePlatCode)
        {
            int productCode = UserUtil.GetProductCode(activePlatCode);
            int terminalCode = UserUtil.GetTerminalCode(activePlatCode);

            if (productCode == 0 && terminalCode == 0)
                return;

            var spec = activeLogRepository.CreateSpecification().Where(
                o => o.UserId == userId &&
                o.ProductCode == productCode &&
                o.TerminalCode == terminalCode
                );

            var count = activeLogRepository.Count(spec);

            if (count == 0)
            {
                var activeLog = new ActiveLog()
                {
                    ProductCode = productCode,
                    TerminalCode = terminalCode,
                    UserId = userId
                };
                activeLogRepository.Create(activeLog);
            }
        }
        /// <summary>
        /// 注册日志(每个UserId只记录一笔)
        /// </summary>
        public void WriteRegisterLog(UserInner userInner, long platCode, string ip, string browser, bool auto, string fromUrl, string extendField)
        {
            var registerLog = new RegisterLog()
            {
                BrowserCode = browser,
                ExtendField = extendField,
                IsAutoReg = auto,
                RegIp = ip,
                RegIpInt = UserUtil.GetIpInt(ip),
                ProductCode = UserUtil.GetProductCode(platCode),
                TerminalCode = UserUtil.GetTerminalCode(platCode),
                UserName = userInner.UserName,
                FromUrl = fromUrl,
                OriginCode = UserUtil.GetOriginCode(platCode)
            };
            registerLogRepository.Create(registerLog);

            //先插入一条最后登录的日志
            var lastLoginLog = new LastLoginLog()
            {
                UserId = userInner.UserId,
                LastLoginIp = ip
            };
            lastLoginLogRepository.Create(lastLoginLog);
        }
    }
}
