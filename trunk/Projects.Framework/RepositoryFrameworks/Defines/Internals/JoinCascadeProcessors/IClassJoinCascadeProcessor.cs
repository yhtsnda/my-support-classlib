using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public interface IClassJoinCascadeProcessor
    {
        void OnCreate(object entity);

        void OnUpdate(object entity);

        void OnDelete(object entity);
    }
}
