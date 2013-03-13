using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Demo
{
    public class MemberSearchFilter
    {
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Account { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
