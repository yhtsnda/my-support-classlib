using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class ThirdTokenAuthorizeData : AuthorizeData
    {
        public ThirdTokenAuthorizeData(int clientId, string redirectUri, long platCode,
            string accessToken, int mappingType,
            string state = null, string scope = null, string browser = null, string ipAddress = null, string extendField = null)
            : base(clientId, redirectUri, platCode, state, scope, browser, ipAddress, extendField)
        {
            AccessToken = accessToken;
            MappingType = mappingType;
        }

        public string AccessToken;

        public int MappingType;

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add("accesstoken", AccessToken);
            data.Add("mappingtype", MappingType.ToString());
            data.Add(Protocal.account_type, "third_token");

            base.UpdateDatas(data);
        }
    }

    public enum MappingType
    {
        /// <summary>
        /// 新浪微博
        /// </summary>
        SinaWeibo = 10,

        /// <summary>
        /// 腾讯微博
        /// </summary>
        Tencent = 20
    }
}
