using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Utils
{
    public abstract class AbstractServerDetector
    {
        protected static DateTime ServerInitTime;

        static AbstractServerDetector()
        {
            ServerInitTime = DateTime.Now;
        }

        public virtual int DueSecond { get { return 200; } }

        protected abstract void OnDetect();

        public void Detect()
        {
            if (DateTime.Now.AddSeconds(-DueSecond) > ServerInitTime)
            {
                OnDetect();
            }
        }
    }
}
