using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    public class UserProxy
    {
        public virtual int UserId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }
    }
}
