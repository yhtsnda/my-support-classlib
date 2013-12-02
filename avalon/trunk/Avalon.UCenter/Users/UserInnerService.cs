using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Avalon.Framework;

namespace Avalon.UCenter
{
    public class UserInnerService : IService
    {
        Regex regMobile = new Regex(@"^(13[0-9]|14[0-9]|15[0-9]|18[0-9])\d{8}$", RegexOptions.Compiled);
        IUserInnerRepository userInnerRepository;
        LogService logService;

        public UserInnerService(IUserInnerRepository userInnerRepository, LogService logService)
        {
            this.userInnerRepository = userInnerRepository;
            this.logService = logService;
        }

        public UserInner GetUserInner(long userId)
        {
            return userInnerRepository.Get(userId);
        }

        public IList<UserInner> GetUserInnerList(IEnumerable<long> userIds)
        {
            return userInnerRepository.GetList(userIds);
        }

        public UserInner GetUserInner(string userName)
        {
            var spec = userInnerRepository.CreateSpecification().Where(o => o.UserName == userName);
            return userInnerRepository.FindOne(spec);
        }

        public UserInner GetUserInnerByNickName(string nickName)
        {
            var spec = userInnerRepository.CreateSpecification().Where(o => o.NickName == nickName);
            return userInnerRepository.FindOne(spec);
        }

        public bool ContainsUserName(string userName)
        {
            return GetUserInner(userName) != null;
        }

        public bool ContainsNickName(string nickName)
        {
            return GetUserInnerByNickName(nickName) != null;
        }

        public void CreateUserInner(UserInner userInner)
        {
            userInnerRepository.Create(userInner);
        }

        public void UpdateUserInner(UserInner userInner)
        {
            userInnerRepository.Update(userInner);
        }

        public ResultWrapper<LoginResultCode, UserInner> TryLogin(string userName, string password, long platcode = 0)
        {
            //基本参数校验
            if (String.IsNullOrEmpty(userName))
                return CreateResult(LoginResultCode.EmptyUserName);

            if (string.IsNullOrEmpty(userName))
                return CreateResult(LoginResultCode.EmptyPassword);

            if (platcode.ToString().IndexOf("3001") >= 0)
                return CreateResult(LoginResultCode.NeedToUpgrade);


            //校验用户是否存在
            var userInner = GetUserInner(userName);
            if (userInner == null)
                return CreateResult(LoginResultCode.UserNotFound);

            //校验密码
            if (!userInner.ValidPassword(password))
                return CreateResult(LoginResultCode.WrongPassword);

            //校验升级
            if (userInner.UpgradeStatus != UpgradeStatus.Upgraded)
                return CreateResult(LoginResultCode.NeedToUpgrade, "亲~因91UP调整，您需到www.91up.com升级帐号后才能继续使用。那边有更多题库哦~");

            return new ResultWrapper<LoginResultCode, UserInner>(LoginResultCode.Success, userInner);
        }

        /// <summary>
        /// 登录成功的流程
        /// </summary>
        internal void OnLoginSuccess(long userId, long platCode, string ip, string browser, string extendField)
        {
            logService.WriteLoginLog(userId, platCode, ip, browser, extendField);
            logService.WriteActiveLog(userId, platCode);
        }

        internal void OnRegisterSuccess(UserInner userInner, long platCode, string ip, string browser, bool auto, string fromUrl, string extendField)
        {
            logService.WriteRegisterLog(userInner, platCode, ip, browser, auto, fromUrl, extendField);
        }

        internal void OnUpgradeSuccess(UserInner userInner)
        {
            logService.WriteUpgradeLog(userInner);
        }

        ResultWrapper<LoginResultCode, UserInner> CreateResult(LoginResultCode code, string message = null)
        {
            return new ResultWrapper<LoginResultCode, UserInner>(code, message ?? code.ToMessage());
        }

        ResultWrapper<RegisterResultCode, UserInner> CreateResult(RegisterResultCode code, string message = null)
        {
            return new ResultWrapper<RegisterResultCode, UserInner>(code, message ?? code.ToMessage());
        }
    }
}
