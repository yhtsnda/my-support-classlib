using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

using Projects.Demo;
using Projects.Framework;
using Projects.Framework.NHibernateAccess;

namespace Projects.Demo
{
    public class MemberMapping : ClassMap<Member>
    {
        public MemberMapping()
        {
            Table("demo_member");

            Id(o => o.MemberId).GeneratedBy.Native();
            Map(p => p.RealName);
            Map(p => p.Account);
            Map(p => p.EMail);
            Map(p => p.CreateTime);
        }
    }

    public class MemberDefine : ClassDefine<Member>
    {
        public MemberDefine()
        {
            Id(o => o.MemberId);
            Map(p => p.RealName);
            Map(p => p.Account);
            Map(p => p.EMail);
            Map(p => p.CreateTime);

            Cache();

            JoinMany(o => o.FriendRelations, (o, spec) => spec.Where(j => j.UserId == o.MemberId));
        }
    }
}
