using Avalon.Framework;
using Avalon.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Avalon.Utility;
using Avalon.NHibernateAccess;

namespace Avalon.Repository.OAuth.Repository
{
    public class ClientAuthorizationRepository : AbstractNoShardRepository<ClientAuthorization>, IClientAuthorizationRepository
    {
        public PagingResult<ClientAuthorization> GetClientAuthorizationList(ClientAuthorizationFilter filter)
        {
            var session = this.GetSession();
            var query = session.QueryOver<ClientAuthorization>();

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.And(o => o.Name.IsLike(filter.Name, MatchMode.Anywhere));

            var count = query.RowCount();
            var list = query
                .OrderBy(o => o.ClientId).Asc()
                .Skip(filter.PageIndex * filter.PageSize)
                .Take(filter.PageSize)
                .List();
            list = list.Distinct().ToList();
            var result = new PagingResult<ClientAuthorization>(count);
            result.AddRange(list);

            return result;
        }
    }
}
