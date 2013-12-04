using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class UserLogin : BaseLogin
    {
        public string Voucher { get; set; }

        public string Password { get; set; }
    }
}
