using Avalon.Framework;
using Avalon.MongoAccess;
using Avalon.OAuth;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Repository.OAuth.Mapping
{
    public class AuthorizationCodeMapping : MongoMap<AuthorizationCode>
    {
        public AuthorizationCodeMapping()
        {
            RegisterClassMap(cm =>
            {
                cm.AutoMap();
                cm.MapIdField(c => c.Code);
                cm.GetMemberMap(c => c.CreateTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
                cm.GetMemberMap(c => c.ExpireTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
            });
        }
    }

    public class AuthorizationCodeDefine : ClassDefine<AuthorizationCode>
    {
        public AuthorizationCodeDefine()
        {
            Table("oa_authorizationcode");

            Id(o => o.Code);
            Map(o => o.AppId);
            Map(o => o.CreateTime);
            Map(o => o.ExpireTime);
            Map(o => o.UserId);
        }
    }
}
