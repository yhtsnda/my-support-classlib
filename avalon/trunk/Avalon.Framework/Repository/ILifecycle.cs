using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 对象生命周期接口
    /// </summary>
    public interface ILifecycle
    {
        /// <summary>
        /// Called when an entity is creating;
        /// </summary>
        /// <returns></returns>
        void OnSaving(bool creating);

        /// <summary>
        /// Called when an entity is saved(createed or updated);
        /// </summary>
        void OnSaved();

        /// <summary>
        /// Called when an entity is loaded;
        /// </summary>
        void OnLoaded();
    }
}
