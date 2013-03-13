using FluentNHibernate.Mapping;
using Projects.Framework;
using Projects.Framework.OAuth2;

namespace Nd.Repository.OAuth2
{
    public class AccessTokenMapping : ClassMap<AccessToken>
    {
        public AccessTokenMapping()
        {
            Cache.NonStrictReadWrite();
            Table("oauth2_accesstoken");

            Id(o => o.Id);
            Map(o => o.UserId);
            Map(o => o.ClientId);
            Map(o => o.Token);
            Map(o => o.Expire);
            Map(o => o.IssueTime);
        }
    }

    public class AccessTokenDefine : ClassDefine<AccessToken>
    {
        public AccessTokenDefine()
        {
            Id(o => o.Id);
            Map(o => o.ClientId);
            Map(o => o.UserId);
            Map(o => o.Token);
            Map(o => o.IssueTime);
            Map(o => o.Expire);
        }
    }
}
