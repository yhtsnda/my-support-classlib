using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Accumlator;

namespace Projects.Framework
{
    public class AccumlatorInterceptor : IRepositoryFrameworkInterceptor
    {
        static IAccumlator accumlator = Accumlator.GetInstance();
        #region IRepositoryFrameworkInterceptor 成员

        public void PreUpdate(object entity)
        {

        }

        public void PreCreate(object entity)
        {

        }

        public void PreDelete(object entity)
        {

        }

        public void PostUpdate(object entity)
        {
            var type = entity.GetType().Name;
            accumlator.IncrementInt64(type + "-PostUpdate");
        }

        public void PostCreate(object entity)
        {
            var type = entity.GetType().Name;
            accumlator.IncrementInt64(type + "-PostCreate");
        }

        public void PostDelete(object entity)
        {
            var type = entity.GetType().Name;
            accumlator.IncrementInt64(type + "-PostDelete");
        }

        public void PostLoad(object entity)
        {
            var type = entity.GetType().Name;
            accumlator.IncrementInt64(type + "-PostLoad");
        }

        #endregion
    }
}
