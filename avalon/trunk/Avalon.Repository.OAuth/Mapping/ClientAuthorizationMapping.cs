using FluentNHibernate.Mapping;
using Avalon.Framework;
using Avalon.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.NHibernateAccess;

namespace Avalon.Repository.OAuth.Mapping
{
    public class ClientAuthorizationMapping : ClassMap<ClientAuthorization>
    {
        public ClientAuthorizationMapping()
        {
            Table("oa_clientauthorization");
            Id(o => o.ClientId).GeneratedBy.Native();
            Map(o => o.Name);
            Map(o => o.Description);
            Map(o => o.Secret);
            Map(o => o.Status).CustomType<ClientAuthorizeStatus>();
            Map(o => o.CreateTime).CustomType("timestamp");
            Map(o => o.UpdateTime).CustomType("timestamp");
            Map(o => o.RedirectPath);
            Map(o => o.Remark);
            Map(o => o.Scopes).CustomType<JsonListUserType<string>>();
            Map(o => o.VerifyCodeType).CustomType<VerifyCodeType>();
        }
    }


    public class ClientAuthorizationDefine : ClassDefine<ClientAuthorization>
    {
        public ClientAuthorizationDefine()
        {
            Id(o => o.ClientId);
            Map(o => o.Name);
            Map(o => o.Description);
            Map(o => o.Secret);
            Map(o => o.Status);
            Map(o => o.CreateTime);
            Map(o => o.UpdateTime);
            Map(o => o.Remark);
            Map(o => o.Scopes);
            Map(o => o.RedirectPath);
            Map(o => o.VerifyCodeType);

            Cache();
        }
    }

}
