using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 规约元数据
    /// </summary>
    public class ClassJoinDefineMetadata
    {
        public ClassJoinDefineMetadata()
        {
            JoinCache = new ClassJoinCacheDefineMetadata();
            JoinCascade = new ClassJoinCascadeDefineMetadata();
        }

        public MethodJoinType JoinType
        {
            get;
            set;
        }

        public string JoinName
        {
            get;
            set;
        }

        public MethodInfo Method
        {
            get;
            set;
        }

        internal IClassJoinDataProcesser DataProcesser
        {
            get;
            set;
        }

        public ClassJoinCacheDefineMetadata JoinCache { get; private set; }

        public ClassJoinCascadeDefineMetadata JoinCascade { get; private set; }
    }

    public enum MethodJoinType
    {
        PropertyGet,
        PropertySet,
        Method
    }
}
