using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public interface IOriginalObjectProvider
    {
        bool IsOriginal(object entity);

        TEntity GetOriginal<TEntity>(TEntity entity);
    }

    internal class EmptyOriginalObjectProvider : IOriginalObjectProvider
    {
        public bool IsOriginal(object entity)
        {
            return true;
        }

        public TEntity GetOriginal<TEntity>(TEntity entity)
        {
            return entity;
        }
    }

}
