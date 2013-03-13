using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Framework.OAuth2
{
    public enum ResultCode
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
        /// 服务端错误
        /// </summary>
        ServerError = 50000

    }
}
