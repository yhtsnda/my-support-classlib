using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public class UserProxy
    {
        public virtual long UserId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }
    }
}
