using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;
using Projects.Demo;
using Projects.Framework.Shards;
using Projects.Tool;
using Projects.Repository;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Projects.Demo
{
    public class MemberRelationRepository : AbstractShardRepository<MemberRelation>, IMemberRelationRepository
    {
    }
}
