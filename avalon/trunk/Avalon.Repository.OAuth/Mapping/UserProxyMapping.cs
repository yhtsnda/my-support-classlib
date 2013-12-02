using FluentNHibernate.Mapping;
using Avalon.Framework;
using Avalon.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Repository.OAuth.Mapping
{
    public class UserProxyDefine : ClassDefine<UserProxy>
    {
        public UserProxyDefine()
        {
            Id(o => o.UserId);
            Map(o => o.UserName);
            Map(o => o.Password);
        }
    }
}
