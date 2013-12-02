using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class Passport91Login : BaseLogin
    {
        public long Passport91Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string CookieOrdernumberMaster { get; set; }

        public string CookieOrdernumberSlave { get; set; }

        public string CookieCheckcode { get; set; }

        public string CookieSiteflag { get; set; }

        public Passport91LoginData ToData()
        {
            return new Passport91LoginData()
            {
                CookieCheckcode = CookieCheckcode,
                CookieOrdernumberMaster = CookieOrdernumberMaster,
                CookieOrdernumberSlave = CookieOrdernumberSlave,
                CookieSiteflag = CookieSiteflag,
                IpAddress = IpAddress,
                Passport91Id = Passport91Id,
                Password = Password,
                UserName = UserName
            };
        }
    }
}
