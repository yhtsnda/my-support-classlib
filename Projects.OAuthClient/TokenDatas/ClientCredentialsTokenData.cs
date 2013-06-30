using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 客户端证书模式的凭据数据
    /// </summary>
    public class ClientCredentialsTokenData : SimpleTokenData
    {
        public ClientCredentialsTokenData(int clientId, string clientSecret)
            : base(clientId, clientSecret)
        {
        }

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.GRANT_TYPE, Protocal.CLIENT_CREDENTIALS);
            base.UpdateDatas(data);
        }
    }
}
