using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class ApiException : Exception
    {
        public ApiResultCode ApiResultCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ApiException()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ApiException(string message, ApiResultCode code = ApiResultCode.ServerError)
            : base(message)
        {
            this.ApiResultCode = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ApiException(string message, Exception innerException, ApiResultCode code = ApiResultCode.ServerError)
            : base(message, innerException)
        {
            this.ApiResultCode = code;
        }
    }

    public enum ApiResultCode
    {
        /// <summary>
        /// token到期
        /// </summary>
        Token_Expired = 10000,
        /// <summary>
        /// token不存在
        /// </summary>
        Token_NotExist = 10001,

        /// <summary>
        /// token为空
        /// </summary>
        Token_IsNull = 10002,

        /// <summary>
        /// token无效
        /// </summary>
        Token_UnValid = 10003,

        /// <summary>
        /// 非法邮箱
        /// </summary>
        InvalidEmail = 11003,

        /// <summary>
        /// 重复邮箱
        /// </summary>
        RepeatedEmail = 12002,

        /// <summary>
        /// 重复昵称
        /// </summary>
        RepeatedNickName = 12003,
        /// <summary>
        /// 重复密码
        /// </summary>
        RepeatedPassword = 12004,
        /// <summary>
        /// 不能更新密码
        /// </summary>
        CanNotUpdatePassword = 12005,
        /// <summary>
        /// 客户端id不存在或者client_secret有误
        /// </summary>
        Client_Error = 20001,
        /// <summary>
        /// 手机端只支持password方式授权
        /// </summary>
        GrantType_Error = 30001,

        /// <summary>
        /// 输入的帐号不存在
        /// </summary>
        User_NotFound = 40001,

        /// <summary>
        /// 账号或密码错误
        /// </summary>
        User_WrongPassword = 40002,
        /// <summary>
        /// 第三方账号未绑定
        /// </summary>
        Third_NotBinding = 40003,
        /// <summary>
        /// 第三方accessToken过期或者无效
        /// </summary>
        Third_AccessTokenError = 40004,
        /// <summary>
        /// 用户已经结束竞赛
        /// </summary>
        Race_HaveFinish = 60000,
        /// <summary>
        /// 用户未报名该项目
        /// </summary>
        Project_UnEnroll = 70001,
        /// <summary>
        /// 服务端错误
        /// </summary>
        ServerError = 50000,
        /// <summary>
        /// 昵称长度超出
        /// </summary>
        InvalidNickNameLength = 11007,
    }
}
