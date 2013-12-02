using FluentNHibernate.Mapping;
using MongoDB.Bson.Serialization.Options;
using Avalon.Framework;
using Avalon.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.MongoAccess;

namespace Avalon.Repository.OAuth.Mapping
{
    public class AccessGrantMapping : MongoMap<AccessGrant>
    {
        public AccessGrantMapping()
        {
            RegisterClassMap(cm =>
            {
                cm.AutoMap();
                cm.MapIdField(c => c.AccessToken);
                cm.GetMemberMap(c => c.CreateTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
                cm.GetMemberMap(c => c.ExpireTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
                cm.GetMemberMap(c => c.RefreshExpireTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
            });
        }
    }

    public class AccessGrantDefine : ClassDefine<AccessGrant>
    {
        public AccessGrantDefine()
        {
            Table("oa_accessgrant");

            Id(o => o.AccessToken);
            Map(o => o.Scope);
            Map(o => o.RefreshToken);
            Map(o => o.ExpireTime);
            Map(o => o.CreateTime);
            Map(o => o.ClientId);
            Map(o => o.ClientCode);
            Map(o => o.UserId);
            Map(o => o.GrantType);
            Map(o => o.RefreshExpireTime);
        }
    }
}
