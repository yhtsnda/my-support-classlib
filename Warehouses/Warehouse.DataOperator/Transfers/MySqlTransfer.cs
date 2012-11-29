using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    /// <summary>
    /// MySQL数据传输操作类
    /// </summary>
    public class MySqlTransfer : IServerTransfer
    {
        public void Clone()
        {
            throw new NotImplementedException();
        }

        public void Clone(System.Collections.IEnumerable tables)
        {
            throw new NotImplementedException();
        }

        public void Generate(IPolicy policy)
        {
            throw new NotImplementedException();
        }

        public void Transmission(string source, string target, IPolicy policy)
        {
            throw new NotImplementedException();
        }
    }
}
