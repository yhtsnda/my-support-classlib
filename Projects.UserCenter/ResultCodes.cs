using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    public enum LoginResultCode
    {
    }

    public enum RegisterResultCode
    {
    }

    public enum ModifyResultCode
    {
        Success = 10000,
        Error = 10001,
        UserHasDisabled = 10002,
        PasswordNoMatch = 10003
    }
}
