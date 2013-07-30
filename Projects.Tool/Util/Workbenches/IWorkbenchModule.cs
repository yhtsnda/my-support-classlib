using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Tool.Util
{
    public interface IWorkbenchModule
    {
        void Init();

        void BeginRequest(HttpApplication app);

        void EndRequest(HttpApplication app);
    }
}
