using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class PasswordThirdTokenTokenData : LoginTokenData
    {
        public PasswordThirdTokenTokenData(int clientId, string clientSecret, long platCode,
            string accessToken, int mappingType,
            string browser = null, string ipAddress = null, string extendField = null)
            : base(clientId, clientSecret, platCode, browser, ipAddress, extendField)
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
            data.Add(Protocal.grant_type, Protocal.thirdtoken);
            data.Add(Protocal.account_type, "third_token");

            base.UpdateDatas(data);
        }
    }
}
