using FluentNHibernate.Mapping;
using Projects.Framework;
using Projects.Framework.OAuth2;

namespace Nd.Repository.OAuth2
{
    public class OAuthClientMapping : ClassMap<OAuthClient>
    {
        public OAuthClientMapping()
        {
            Cache.NonStrictReadWrite();
            Table("oauth2_client");

            Id(o => o.Id);
            Map(o => o.Name);
            Map(o => o.Key);
            Map(o => o.Secret);
            Map(o => o.Description);

        }
    }

    public class OAuthClientDefine : ClassDefine<OAuthClient>
    {
        public OAuthClientDefine()
        {
            Id(o => o.Id);
            Map(o => o.Name);
            Map(o => o.Description);
            Map(o => o.Key);
            Map(o => o.Secret);

        }
    }
}
