using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Demo
{
    public class MemberRelation
    {
        protected MemberRelation()
        {
        }

        public MemberRelation(int userId, int friendId, string FriendName)
        {
            this.UserId = userId;
            this.FriendId = friendId;
            this.FriendName = FriendName;
        }

        public virtual int Id { get; protected set; }

        public virtual int UserId { get; protected set; }

        public virtual int FriendId { get; protected set; }

        public virtual string FriendName { get; protected set; }
    }
}
