﻿using NHibernate.Proxy;
using Projects.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.NHibernateAccess
{
    internal class NHibernateOriginalObjectProvider : IOriginalObjectProvider
    {
        public TEntity GetOriginal<TEntity>(TEntity entity)
        {
            if (entity is INHibernateProxy)
            {
                var proxy = (INHibernateProxy)entity;
                return (TEntity)proxy.HibernateLazyInitializer.GetImplementation();
            }
            return entity;
        }

        public bool IsOriginal(object entity)
        {
            return !(entity is INHibernateProxy);
        }
    }
}
