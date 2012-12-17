using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Accesses.Builders
{
    public interface IStorageMappingResolver<TEntity> where TEntity : class, new()
    {
        string ResolveTableName();
        bool IsAutoIdentityField(string propertyName);
        string ResolveFieldName(string propertyName);
    }
}
