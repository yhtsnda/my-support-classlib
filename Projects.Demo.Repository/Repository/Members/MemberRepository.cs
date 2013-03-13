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
    public class MemberRepository : AbstractShardRepository<Member>, IMemberRepository
    {
        ISession GetSession()
        {
            return this.GetSession(ShardParams.Empty);
        }

        public PagingResult<Member> GetPagingMemberList(MemberSearchFilter filter)
        {
            var session = GetSession();
            var query = session.QueryOver<Member>();

            if (!String.IsNullOrEmpty(filter.Account))
                query.And(o => o.Account.IsLike(filter.Account, MatchMode.Anywhere));
            query.And(o => o.CreateTime >= filter.BeginDate && o.CreateTime <= filter.EndDate);

            var list = query
                .OrderBy(o => o.MemberId).Desc()
                .Skip(filter.PageIndex * filter.PageSize)
                .Take(filter.PageSize)
                .List();

            PagingResult<Member> result = new PagingResult<Member>(query.RowCount());
            result.AddRange(list);
            return result;
        }
    }
}
