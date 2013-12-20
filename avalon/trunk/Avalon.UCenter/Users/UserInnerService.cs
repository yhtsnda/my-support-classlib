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
        Regex regIdCard = new Regex(@"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        IUserInnerRepository userInnerRepository;
        LogService logService;
        PlatformService platService;

        public UserInnerService(IUserInnerRepository userInnerRepository, 
            LogService logService,
            PlatformService platService)
        {
            this.userInnerRepository = userInnerRepository;
            this.logService = logService;
            this.platService = platService;
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        public void CreateUserInner(UserInner userInner)
        {
            userInnerRepository.Create(userInner);
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        public void UpdateUserInner(UserInner userInner)
        {
            userInnerRepository.Update(userInner);
        }
        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        public UserInner GetUserInner(int userId)
        {
            return userInnerRepository.Get(userId);
        }
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        public UserInner GetUserInnerByName(string userName)
        {
            var spec = userInnerRepository.CreateSpecification().Where(o => o.UserName == userName);
            return userInnerRepository.FindOne(spec);
        }
        /// <summary>
        /// 根据用户手机号码获取用户信息
        /// </summary>
        public UserInner GetUserInnerByMobile(string mobile)
        {
            var spec = userInnerRepository.CreateSpecification().Where(o => o.Mobile == mobile);
            return userInnerRepository.FindOne(spec);
        }
        /// <summary>
        /// 根据用户的身份证号码获取用户信息
        /// </summary>
        public UserInner GetUserInnerByIdCard(string idCard)
        {
            var spec = userInnerRepository.CreateSpecification().Where(o => o.IdCard == idCard);
            return userInnerRepository.FindOne(spec);
        }
        /// <summary>
        /// 获取多个用户信息
        /// </summary>
        public IList<UserInner> GetUserInnerList(IEnumerable<long> userIds)
        {
            return userInnerRepository.GetList(userIds);
        }
        public bool ContainsUserName(string userName)
        {
            return GetUserInnerByName(userName) != null;
        }
        /// <summary>
        /// 尝试进行登录
        /// </summary>
        /// <param name="voucher">登录凭证</param>
        /// <param name="password">密码(加密后)</param>
        /// <param name="appCode">应用编码</param>
        public ResultWrapper<LoginResultCode, UserInner> TryLogin(string voucher, string password, string appCode)
        {
            //基本参数校验
            if (String.IsNullOrEmpty(voucher))
                return CreateResult(LoginResultCode.EmptyUserName);
            if (string.IsNullOrEmpty(password))
                return CreateResult(LoginResultCode.EmptyPassword);
            if (String.IsNullOrEmpty(appCode))
                return CreateResult(LoginResultCode.EmptyAppCode);
            //获取平台应用
            var plat = platService.GetPlatform(appCode);
            if (plat == null || plat.Status == PlatformStatus.NotAvailable)
                return CreateResult(LoginResultCode.InvalidAppCode);

            UserInner userInner = null;
            //优先判断用户名
            if (plat.UsedVoucher.Any(o => o == LoginVouchers.UserName))
                userInner = GetUserInnerByName(voucher);
            //再判断手机号码
            if (userInner == null && 
                plat.UsedVoucher.Any(o=>o == LoginVouchers.Mobile) &&
                regMobile.IsMatch(voucher))
                userInner = GetUserInnerByMobile(voucher);
            //最后判断身份证
            if (userInner == null &&
                plat.UsedVoucher.Any(o => o == LoginVouchers.IdCard) &&
                regIdCard.IsMatch(voucher))
                userInner = GetUserInnerByIdCard(voucher);

            //校验用户是否存在
            if (userInner == null)
                return CreateResult(LoginResultCode.UserNotFound);
            //判断用户是否允许在该平台登录
            if(!userInner.AllowLoginApps.Any(o=>o == plat.Id))
                return CreateResult(LoginResultCode.NotAllowLoginApp);
            //校验密码
            if (!userInner.ValidPassword(password))
                return CreateResult(LoginResultCode.WrongPassword);
            return new ResultWrapper<LoginResultCode, UserInner>(LoginResultCode.Success, userInner);
        }
        /// <summary>
        /// 登录成功的流程
        /// </summary>
        internal void OnLoginSuccess(int userId, string platCode, string ip, string browser, string extendField)
        {
            logService.WriteLoginLog(userId, platCode, ip, browser, extendField);
            logService.WriteActiveLog(userId, platCode);
        }
        /// <summary>
        /// 注册成功后,记录日志
        /// </summary>
        internal void OnRegisterSuccess(UserInner userInner, long platCode, string ip, string browser, bool auto, string fromUrl, string extendField)
        {
            logService.WriteRegisterLog(userInner, platCode, ip, browser, auto, fromUrl, extendField);
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
