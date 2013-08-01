using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class InsertReplaceStatement : Statement
    {
        private IList<RowExpression> rowListBackup;
        private IList<RowExpression> rowList;

        public InsertReplaceStatement(Identifier table, IList<Identifier> columnList,
            IList<RowExpression> rowList)
        {
            this.Table = table;
            this.ColumnList = columnList;
            this.RowList = rowList;
            this.Select = null;
        }

        public InsertReplaceStatement(Identifier table, IList<Identifier> columnList,
            IQueryExpression select)
        {
            this.Table = table;
            this.ColumnList = columnList;
            this.RowList = null;
            this.Select = select;
        }

        public Identifier Table { get; protected set; }
        public IQueryExpression Select { get; protected set; }
        
        public IList<RowExpression> RowList
        {
            get
            {
                return rowList;
            }
            set
            {
                rowListBackup = rowList;
                rowList = value;
            }
        }
        public IList<Identifier> ColumnList { get; protected set; }

        public void ClearReplaceRowList()
        {
            if (rowListBackup != null)
            {
                this.RowList = rowListBackup;
                rowListBackup = null;
            }
        }
    }
}
