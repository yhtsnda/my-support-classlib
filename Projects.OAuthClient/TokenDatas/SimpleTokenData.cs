using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 凭据数据基础类
    /// </summary>
    public abstract class SimpleTokenData
    {
        public SimpleTokenData(int clientId, string clientSecret)
        {
        }

        public int ClientId { get; set; }
        public string ClientSecret { get; set; }

        public virtual void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.CLIENT_ID, ClientId.ToString());
            data.Add(Protocal.CLIENT_SECRET, ClientSecret);
            data.Add(Protocal.RESPONSE_TYPE, Protocal.TOKEN);
        }
    }
}
