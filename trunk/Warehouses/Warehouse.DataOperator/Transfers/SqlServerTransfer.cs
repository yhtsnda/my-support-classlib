using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    /// <summary>
    /// SQL Server数据库传输类
    /// </summary>
    public class SqlServerTransfer : IServerTransfer
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
