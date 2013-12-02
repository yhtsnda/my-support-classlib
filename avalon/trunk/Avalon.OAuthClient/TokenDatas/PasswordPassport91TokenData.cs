using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class PasswordPassport91TokenData : LoginTokenData
    {
        public PasswordPassport91TokenData(int clientId, string clientSecret, long platCode,
            long passport91Id, string userName, string password, string cookieOrdernumberMaster, string cookieOrdernumberSlave, string cookieCheckcode, string cookieSiteflag,
            string browser = null, string ipAddress = null, string extendField = null)
            : base(clientId, clientSecret, platCode, browser, ipAddress, extendField)
        {
            Passport91Id = passport91Id;
            UserName = userName;
            Password = password;
            CookieOrdernumberMaster = cookieOrdernumberMaster;
            CookieOrdernumberSlave = cookieOrdernumberSlave;
            CookieCheckcode = cookieCheckcode;
            CookieSiteflag = cookieSiteflag;
        }

        public long Passport91Id;

        public string UserName;

        public string Password;

        public string CookieOrdernumberMaster;

        public string CookieOrdernumberSlave;

        public string CookieCheckcode;

        public string CookieSiteflag;

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add("passport91id", Passport91Id.ToString());
            data.Add("username", UserName);
            data.Add("password", Password);
            data.Add("cookieordernumbermaster", CookieOrdernumberMaster);
            data.Add("cookieordernumberslave", CookieOrdernumberSlave);
            data.Add("cookiecheckcode", CookieCheckcode);
            data.Add("cookiesiteflag", CookieSiteflag);
            data.Add(Protocal.grant_type, Protocal.password);
            data.Add(Protocal.account_type, "passport91");

            base.UpdateDatas(data);
        }
    }
}
