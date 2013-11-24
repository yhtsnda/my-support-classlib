using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class ODataStringBuilder
    {
        public ODataStringBuilder()
        {
            Filter = new StringBuilder();
            Orderby = new StringBuilder();
            Select = new StringBuilder();
        }

        public StringBuilder Filter { get; private set; }

        public StringBuilder Orderby { get; private set; }

        public int Skip { get; set; }

        public int Top { get; set; }

        public StringBuilder Select { get; private set; }
    }
}
