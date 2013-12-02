using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public interface IVerifyCodeRepository : IRepository<VerifyCode>
    {
        bool Valid(string sessionId, string verifyCode);

        void SaveVerifyCode(string sessionId, string verifyCode);
    }
}
