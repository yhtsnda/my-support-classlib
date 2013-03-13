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
    public class MemberRelationMapping : ClassMap<MemberRelation>
    {
        public MemberRelationMapping()
        {
            Table("demo_memberrelation");

            Id(o => o.Id).GeneratedBy.Native();
            Map(p => p.UserId);
            Map(p => p.FriendId);
            Map(p => p.FriendName);
        }
    }

    public class MemberRelationDefine : ClassDefine<MemberRelation>
    {
        public MemberRelationDefine()
        {
            Id(o => o.Id);
            Map(p => p.UserId);
            Map(p => p.FriendId);
            Map(p => p.FriendName);

            Cache();
        }
    }
}
