using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public class ClassJoinCascadeDefineMetadata
    {
        internal Func<IClassJoinCascadeProcessor> ProcessorHandler;

        public ClassJoinCascadeDefineMetadata()
        {
            CascataType = CascadeType.None;
        }

        public CascadeType CascataType { get; set; }

        public IClassJoinCascadeProcessor Processor { get { return ProcessorHandler(); } }


    }
}
