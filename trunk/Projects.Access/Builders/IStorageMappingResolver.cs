using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Accesses.Builders
{
    public interface IStorageMappingResolver<TEntity> where TEntity : class, new()
    {
        string ResolveTableName<TEntity>();
        bool IsAutoIdentityField<TEntity>(string propertyName);
        string ResolveFieldName<TEntity>(string propertyName);
    }
}
