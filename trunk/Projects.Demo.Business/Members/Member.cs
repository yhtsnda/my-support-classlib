using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Demo
{
    public partial class Member
    {
        protected Member()
        {
            this.CreateTime = DateTime.Now;
        }

        public Member(string realName, string email, string account) 
            : this()
        {
            this.RealName = realName;
            this.EMail = email;
            this.Account = account;
        }

        public virtual int MemberId { get; protected set; }

        public virtual string RealName { get; protected set; }

        public virtual string EMail { get; protected set; }

        public virtual string Account { get; protected set; }

        public virtual IList<MemberRelation> FriendRelations { get; protected set; }

        public virtual DateTime CreateTime { get; protected set; }
    }
}
