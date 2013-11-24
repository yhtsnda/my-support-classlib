using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal enum RaiseType
    {
        Unknown,
        PreCreate,
        PreUpdate,
        PreDelete,
        PostCreate,
        PostUpdate,
        PostDelete,
        PostLoad
    }

    internal class UniqueRaise : IDisposable
    {
        const string UniqueRaiseKey = "_UniqueRaiseKey_";
        string key = null;

        public bool NotRaised
        {
            get;
            private set;
        }

        public bool AutoDisposed
        {
            get;
            private set;
        }

        public UniqueRaise(RaiseType raiseType, ClassDefineMetadata metadata, object entity, bool autoDisposed)
        {
            key = String.Format("{0}:{1}:{2}", metadata.EntityType.FullName, EntityUtil.GetId(entity), raiseType);
            NotRaised = !WorkbenchUtil<string, bool>.GetValue(UniqueRaiseKey, key);
            if (NotRaised)
                WorkbenchUtil<string, bool>.SetValue(UniqueRaiseKey, key, true);

            AutoDisposed = autoDisposed;
        }

        public void Dispose()
        {
            if (NotRaised)
                WorkbenchUtil<string, bool>.SetValue(UniqueRaiseKey, key, false);
        }
    }
}
