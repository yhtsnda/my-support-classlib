using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    public interface IQuerySourceProvider
    {
        bool IsSource(Type entityType);

        string GetIdentityName(Type entityType);

        string GetTableName(Type entityType);

        string GetColumnName(Type entityType, PropertyInfo property);
    }
}
