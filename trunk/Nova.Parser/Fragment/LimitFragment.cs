using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LimitFragment : IFragment
    {
        private ParamMarker offsetP, sizeP;
        private decimal offset, size;

        public LimitFragment(decimal offset, decimal size)
        {
            if (offset == null || size == null)
                throw new ArgumentNullException();
            this.offset = offset;
            this.size = size;
        }

        public LimitFragment(decimal offset, ParamMarker sizeP)
        {
            if (offset == null || sizeP == null)
                throw new ArgumentNullException();
            this.offset = offset;
            this.sizeP = sizeP;
        }

        public LimitFragment(ParamMarker offsetP, decimal size)
        {
            if (offsetP == null || size == null)
                throw new ArgumentNullException();
            this.offsetP = offsetP;
            this.size = size;
        }

        public LimitFragment(ParamMarker offsetP, ParamMarker sizeP)
        {
            if (offsetP == null || sizeP == null)
                throw new ArgumentNullException();
            this.offsetP = offsetP;
            this.sizeP = sizeP;
        }

        public object GetOffset()
        {
            if (offset == null)
                return offsetP;
            return offset;
        }

        public object GetSize()
        {
            if (size == null)
                return sizeP;
            return size;
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LimitFragment>(this);
        }
    }
}
