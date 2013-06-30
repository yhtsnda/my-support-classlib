using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;

namespace Projects.OAuth
{
    public interface IUserProxyRepository : INoShardRepository<UserProxy>
    {
        ValidResult ValidPassword(string userName, string password, long platCode, string browser = null, string ipAddress = null, string extendFiled = null);

        ValidResult ValidThirdToken(string accessToken, int mappingType, long platCode, string browser = null, string ipAddress = null, string extendFiled = null);
    }

    public class ValidResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }
    }
}
