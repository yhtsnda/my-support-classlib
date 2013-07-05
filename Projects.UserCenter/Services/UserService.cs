using Projects.Tool;
using Projects.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    public class UserService : IService
    {
        private IUserRepository userRepository;
        private MappingService mappingService;
        private LogService logService;

        public UserService(IUserRepository userRepository,
            MappingService mappingService,
            LogService logService)
        {
            this.userRepository = userRepository;
            this.mappingService = mappingService;
            this.logService = logService;
        }

        #region 获取用户信息
        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUser(int userId)
        {
            return userRepository.Get(userId);
        }

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUserByName(string userName)
        {
            var spec = userRepository.CreateSpecification().Where(o => o.UserName == userName);
            return userRepository.FindOne(spec);
        }

        /// <summary>
        /// 根据用户ID获取用户列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<User> GetUserList(IEnumerable<int> ids)
        {
            return userRepository.GetList(ids);
        }

        /// <summary>
        /// 更加用户名获取用户列表
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public IList<User> GetUserListByName(IEnumerable<string> names)
        {
            var spec = userRepository.CreateSpecification().Where(o => names.Contains(o.UserName));
            return userRepository.FindAll(spec);
        }
        #endregion

        #region 登录&注册
        /// <summary>
        /// 正常登录
        /// </summary>
        /// <returns>操作结果</returns>
        public ResultWrapper<LoginResultCode, User> Login(string userName, string password)
        {
            Arguments.NotNullOrWhiteSpace(userName, "userName");
            Arguments.NotNullOrWhiteSpace(password, "password");

            var spec = userRepository.CreateSpecification().Where(o => o.UserName == userName);
            var user = userRepository.FindOne(spec);
            if (user == null)
                return new ResultWrapper<LoginResultCode, User>(LoginResultCode.UserNotFound,
                    LoginResultCode.UserNotFound.ToMessage());
            if(user.ValidatePassword(password))
            {
                user.Password = "";
                return new ResultWrapper<LoginResultCode, User>(LoginResultCode.Success,
                    user, LoginResultCode.Success.ToMessage());
            }
            return new ResultWrapper<LoginResultCode, User>(LoginResultCode.WrongPassword,
                LoginResultCode.WrongPassword.ToMessage());
        }

        /// <summary>
        /// 使用Mapping的信息进行登录
        /// </summary>
        /// <returns></returns>
        public ResultWrapper<LoginResultCode, User> LoginWithMapping(string mappingKey, 
            string password, MappingType type)
        {
            if (type == MappingType.None)
                return Login(mappingKey, password);

            var mapping = mappingService.GetMappingByMappingKey(mappingKey, type);
            if (mapping == null)
                return new ResultWrapper<LoginResultCode, User>(LoginResultCode.MappingNotFound,
                    LoginResultCode.Success.ToMessage());
            var user = userRepository.Get(mapping.LocalUserId);
            if (type == MappingType.Mobile || type == MappingType.IdCard)
            {

                if (user != null && user.ValidatePassword(password))
                {
                    user.Password = "";
                    return new ResultWrapper<LoginResultCode, User>(LoginResultCode.Success,
                        user, LoginResultCode.Success.ToMessage());
                }
            }
            if (type == MappingType.SinaWeibo || type == MappingType.Tencent)
            {
                if (user != null)
                {
                    user.Password = "";
                    return new ResultWrapper<LoginResultCode, User>(LoginResultCode.Success,
                        user, LoginResultCode.Success.ToMessage());
                }
            }
            return new ResultWrapper<LoginResultCode, User>(LoginResultCode.WrongPassword,
                LoginResultCode.WrongPassword.ToMessage());
        }

        /// <summary>
        /// 正常注册
        /// </summary>
        /// <param name="model">注册模型</param>
        /// <returns>注册结果</returns>
        public ResultWrapper<RegisterResultCode, User> Register(UserRegisterModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注册并设置映射信息
        /// </summary>
        /// <param name="model">注册模型</param>
        /// <returns>注册结果</returns>
        public ResultWrapper<RegisterResultCode, User> RegisterWithMapping(MappingRegisterModel model)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 用户信息验证
        /// <summary>
        /// 验证用户名是否存在
        /// </summary>
        /// <returns></returns>
        public ResultWrapper<RegisterResultCode> ValidateUserName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 验证昵称是否存在
        /// </summary>
        /// <returns></returns>
        public ResultWrapper<RegisterResultCode> ValidateNickName()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 用户信息修改
        public ResultWrapper<ModifyResultCode> ModifyPassword(int userId, string oldPwd, string newPwd)
        {
            var user = userRepository.Get(userId);
            if (user.Status == UserStatus.Disabled)
                return CreateModifyResult(ModifyResultCode.UserHasDisabled, "该用户已经被禁用,不能修改密码");

            throw new NotImplementedException();
        }

        public ResultWrapper<ModifyResultCode> ModifyUser()
        {
            throw new NotImplementedException();
        }

        public ResultWrapper<ModifyResultCode> DeleteUser()
        {
            throw new NotImplementedException();
        }
        #endregion


        private ResultWrapper<ModifyResultCode> CreateModifyResult(ModifyResultCode code, string message)
        {
            return new ResultWrapper<ModifyResultCode>(code, message);
        }
    }
}
