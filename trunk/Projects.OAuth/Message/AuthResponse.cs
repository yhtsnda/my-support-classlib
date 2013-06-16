using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 授权响应
    /// </summary>
    public class AuthResponse
    {
        public string Code { get; set; }

        public string State { get; set; }
    }
}
