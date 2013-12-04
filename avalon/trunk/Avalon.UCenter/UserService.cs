using Avalon.Framework;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class UserService : IService
    {
        UserInnerService userInnerService;
        VerifyCodeService verifyCodeService;
        MappingService mappingService;
        ThirdPartyService thirdPartService;

        public UserService(UserInnerService userInnerService, 
            MappingService mappingService, 
            VerifyCodeService verifyCodeService, 
            ThirdPartyService thirdPartService)
        {
            this.userInnerService = userInnerService;
            this.verifyCodeService = verifyCodeService;
            this.mappingService = mappingService;
            this.thirdPartService = thirdPartService;
        }

        #region login
        /// <summary>
        /// 使用用户中心进行登录
        /// </summary>
        public ResultWrapper<LoginResultCode, UserInner> TryLogin(UserLogin login)
        {
            var result = userInnerService.TryLogin(login.Voucher, login.Password, login.AppCode);
            //登录成功
            if (result.Code == LoginResultCode.Success)
                userInnerService.OnLoginSuccess(result.Data.UserId, login.AppCode, login.IpAddress, login.Browser, login.ExtendField);
            return result;
        }
        /// <summary>
        /// 第三方登录
        /// </summary>
        public ResultWrapper<LoginResultCode, UserInner> TryLogin(ThirdLogin login)
        {
            var userKey = thirdPartService.GetUserKey(login.AccessToken, login.MappingType);
            var mapping = mappingService.GetMapping(userKey, login.MappingType);
            if (mapping == null)
                return CreateLoginResult(LoginResultCode.BindingNotFound);

            var userInner = userInnerService.GetUserInner(mapping.LocalUserId);
            //登录成功
            userInnerService.OnLoginSuccess(userInner.UserId, login.PlatCode, login.IpAddress, login.Browser, login.ExtendField);

            return CreateLoginResult(userInner);
        }
        /// <summary>
        /// 第三方帐号绑定登录
        /// </summary>
        public ResultWrapper<LoginResultCode, UserInner> TryLoginForThirdBind(UserLogin login, string accessToken, MappingType mappingType)
        {
            var result = TryLogin(login);
            if (result.Code == LoginResultCode.Success)
            {
                var userKey = thirdPartService.GetUserKey(accessToken, mappingType);
                mappingService.CreateMapping(userKey, mappingType, result.Data);
            }
            return result;
        }
        #endregion

        #region register

        /// <summary>
        /// 注册（本地帐号不存在）
        /// </summary>
        /// <remarks>
        /// 流程：
        /// 1、验证码校验
        /// 2、用户名在用户中心不存在
        /// </remarks>
        public ResultWrapper<RegisterResultCode, UserInner> TryRegister(UserRegister register)
        {
            return TryRegister(register, (userName) =>
            {
                if (userInnerService.ContainsUserName(userName))
                    return RegisterResultCode.RepeatedUserName;

                return RegisterResultCode.Success;
            });
        }

        /// <summary>
        /// 第三方帐号注册绑定
        /// </summary>
        public ResultWrapper<RegisterResultCode, UserInner> TryRegisterForThirdBind(UserRegister register, string accessToken, MappingType source)
        {
            var result = TryRegister(register);

            if (result.Code == RegisterResultCode.Success)
            {
                var userKey = thirdPartService.GetUserKey(accessToken, source);
                mappingService.CreateMapping(userKey, source, result.Data);
            }
            return result;
        }

        /// <summary>
        /// 升级（要求本地帐号存在）
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public ResultWrapper<RegisterResultCode, UserInner> TryRegisterForPassport91Bind(UserRegister register)
        {
            return TryRegister(register, (userName) =>
            {
                if (!userInnerService.ContainsUserName(userName))
                    return RegisterResultCode.EmptyUserForBind;

                return RegisterResultCode.Success;
            });
        }

        ResultWrapper<RegisterResultCode, UserInner> TryRegister(UserRegister register, Func<string, RegisterResultCode> checkLocalUserNameFunc)
        {
            Arguments.NotNull(register, "register");

            if (!verifyCodeService.ValidVerifyCode(register.SessionId, register.VerifyCode))
                return CreateRegisterResult(RegisterResultCode.InvalidVerifyCode);

            //基本的校验
            RegisterResultCode resultCode;
            var flag = UserValid.ValidUserName(register.UserName, out resultCode) &&
                UserValid.ValidPassword(register.Password, out resultCode) &&
                UserValid.ValidNickName(register.NickName, out resultCode) &&
                UserValid.ValidEmail(register.Email, out resultCode);
            if (!flag)
                return CreateRegisterResult(resultCode);

            //本地校验
            resultCode = checkLocalUserNameFunc(register.UserName);
            if (resultCode != RegisterResultCode.Success)
                return CreateRegisterResult(resultCode);

            //检测昵称
            if (userInnerService.ContainsUserName(register.NickName))
                return CreateRegisterResult(RegisterResultCode.RepeatedNickName);

            //如果 Email 未填并且 UserName 为Email则赋值。
            if (String.IsNullOrEmpty(register.Email) && UserValid.IsEmail(register.UserName))
                register.Email = register.UserName;

            //91注册
            var result = passport91Service.Register(register);

            resultCode = result.Code.ToRegisterResultCode();
            if (resultCode != RegisterResultCode.Success)
                return CreateRegisterResult(resultCode, resultCode == RegisterResultCode.ApiException ? result.ToString() : resultCode.ToMessage());

            var userInner = mappingService.EnsureUserMapping(result.Data, register.UserName, register.NickName, register.Password, register.Email, register.PlatCode, register.IpAddress, register.Browser, false, register.FromUrl, register.ExtendFiled);

            return CreateRegisterResult(userInner);
        }

        #endregion

        public UserInner GetUser(long userId)
        {
            return userInnerService.GetUserInner(userId);
        }

        public UserInner GetUser(string userName)
        {
            return userInnerService.GetUserInner(userName);
        }

        public UserInner GetUserByNickName(string nickName)
        {
            return userInnerService.GetUserInnerByNickName(nickName);
        }

        public IList<UserInner> GetUserList(IEnumerable<long> userIds)
        {
            return userInnerService.GetUserInnerList(userIds);
        }

        public ResultWrapper<ModifyPasswordResultCode> ModifyPassword(string userName, string passwordOld, string passwordNew)
        {
            var result = passport91Service.ModifyPassword(userName, passwordOld, passwordNew);
            var cCode = result.Code.ToModifyPasswordResultCode();
            if (cCode == ModifyPasswordResultCode.Success)
            {
                //修改本地密码
                var user = GetUser(userName);
                user.Password = passwordNew;
                userInnerService.UpdateUserInner(user);
                return CreateModifyPasswordResultForVoid(ModifyPasswordResultCode.Success);

            }
            return CreateModifyPasswordResultForVoid(cCode, cCode == ModifyPasswordResultCode.ApiException ? result.ToString() : cCode.ToMessage());
        }

        /// <summary>
        /// 验证用户名是否可用
        /// </summary>
        public ResultWrapper<RegisterResultCode> ValidUserName(string userName, bool checkLocalExists = true)
        {
            RegisterResultCode code = UserValid.ValidUserName(userName);
            if (code != RegisterResultCode.Success)
                return CreateRegisterResultForVoid(code);

            if (checkLocalExists && userInnerService.ContainsUserName(userName))
                return CreateRegisterResultForVoid(RegisterResultCode.RepeatedUserName);

            var result = passport91Service.ValidUserName(userName);
            var cCode = result.Code.ToRegisterResultCode();
            return CreateRegisterResultForVoid(cCode, cCode == RegisterResultCode.ApiException ? result.ToString() : cCode.ToMessage());
        }

        /// <summary>
        /// 验证昵称是否可用
        /// </summary>
        public ResultWrapper<RegisterResultCode> ValidNickName(string nickName)
        {
            RegisterResultCode code = UserValid.ValidNickName(nickName);
            if (code != RegisterResultCode.Success)
                return CreateRegisterResultForVoid(code);

            if (userInnerService.ContainsNickName(nickName))
                return CreateRegisterResultForVoid(RegisterResultCode.RepeatedNickName);

            return CreateRegisterResultForVoid(RegisterResultCode.Success);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nickName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ResultWrapper<RegisterResultCode> UpdateUser(long userId, string nickName, string email)
        {
            var user = GetUser(userId);
            Arguments.NotNull(user, "user");

            if (user.NickName != nickName)
            {
                var result = ValidNickName(nickName);
                if (result.Code != RegisterResultCode.Success)
                    return result;
            }

            if (!string.IsNullOrEmpty(email))
            {
                var emailValidResult = UserValid.ValidEmail(email);
                if (emailValidResult != RegisterResultCode.Success)
                    return CreateRegisterResultForVoid(emailValidResult);
            }           

            if (!string.IsNullOrEmpty(email))
                user.Email = email;

            user.NickName = nickName;
            userInnerService.UpdateUserInner(user);

            return CreateRegisterResultForVoid(RegisterResultCode.Success);
        }

        public void WriteLoginLog(long userId, long platCode, string ip, string browser, string extendField)
        {
            userInnerService.OnLoginSuccess(userId, platCode, ip, browser, extendField);
        }


        #region helper
        ResultWrapper<LoginResultCode, UserInner> CreateLoginResult(LoginResultCode code, string message = null)
        {
            return new ResultWrapper<LoginResultCode, UserInner>(code, message ?? code.ToMessage());
        }

        ResultWrapper<LoginResultCode, UserInner> CreateLoginResult(UserInner userInner)
        {
            return new ResultWrapper<LoginResultCode, UserInner>(LoginResultCode.Success, userInner);
        }

        ResultWrapper<RegisterResultCode, UserInner> CreateRegisterResult(RegisterResultCode code, string message = null)
        {
            return new ResultWrapper<RegisterResultCode, UserInner>(code, message ?? code.ToMessage());
        }

        ResultWrapper<RegisterResultCode, UserInner> CreateRegisterResult(UserInner userInner)
        {
            return new ResultWrapper<RegisterResultCode, UserInner>(RegisterResultCode.Success, userInner);
        }

        ResultWrapper<RegisterResultCode> CreateRegisterResultForVoid(RegisterResultCode code, string message = null)
        {
            return new ResultWrapper<RegisterResultCode>(code, message ?? code.ToMessage());
        }

        ResultWrapper<ModifyPasswordResultCode> CreateModifyPasswordResultForVoid(ModifyPasswordResultCode code, string message = null)
        {
            return new ResultWrapper<ModifyPasswordResultCode>(code, message ?? code.ToMessage());
        }
        #endregion
    }
}
