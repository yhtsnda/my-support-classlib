using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Type;

namespace Avalon.NHibernateAccess.Projections
{
    /// <summary>
    /// ArithmeticOperatorProjection
    /// </summary>
    public class ArithmeticOperatorProjection : OperatorProjection
    {
        /// <summary>
        /// Initializes a new instance of the ArithmeticOperatorProjection class
        /// </summary>
        /// <param name="op"></param>
        /// <param name="returnType"></param>
        /// <param name="args"></param>
        public ArithmeticOperatorProjection(string op, IType returnType, params IProjection[] args)
            : base(op, returnType, args)
        {
            if (args.Length < 2)
                throw new ArgumentOutOfRangeException("args", args.Length, "Requires at least 2 projections");
        }

        /// <summary>
        /// AllowedOperators
        /// </summary>
        public override string[] AllowedOperators
        {
            get { return new[] { "+", "-", "*", "/", "%" }; }
        }
    }

    /// <summary>
    /// BitwiseOperatorProjection
    /// </summary>
    public class BitwiseOperatorProjection : OperatorProjection
    {
        /// <summary>
        /// Initializes a new instance of the BitwiseOperatorProjection class
        /// </summary>
        /// <param name="op"></param>
        /// <param name="returnType"></param>
        /// <param name="args"></param>
        public BitwiseOperatorProjection(string op, IType returnType, params IProjection[] args)
            : base(op, returnType, args)
        {
            if (args.Length < 2)
                throw new ArgumentOutOfRangeException("args", args.Length, "Requires at least 2 projections");
        }

        /// <summary>
        /// AllowedOperators
        /// </summary>
        public override string[] AllowedOperators
        {
            get { return new[] { "&", "|", "^" }; }
        }
    }
}
