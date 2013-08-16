using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class PolyadicOperatorExpression : AbstractExpression
    {
        protected List<IExpression> operands;
        protected ExpressionPrecedence precedence;

        public int Arity
        {
            get
            {
                if (operands == null)
                    return -1;
                return operands.Count;
            }
        }

        public PolyadicOperatorExpression(ExpressionPrecedence precedence)
            : this(precedence, true)
        {
        }

        public PolyadicOperatorExpression(ExpressionPrecedence precedence, bool leftCombine)
            : this(precedence, 4)
        {
        }

        public PolyadicOperatorExpression(ExpressionPrecedence precedence, int initArity)
        {
            this.precedence = precedence;
            this.operands = new List<IExpression>(initArity);
        }

        public PolyadicOperatorExpression AppendOperand(IExpression operand)
        {
            if (operands == null)
                return this;
            if (operand is PolyadicOperatorExpression)
            {
                operands.AddRange(((PolyadicOperatorExpression)operand).operands);
            }
            else
            {
                operands.Add(operand);
            }
            return this;
        }

        public IExpression GetOperand(int index)
        {
            if (index >= operands.Count)
                throw new IndexOutOfRangeException("only contains " + operands.Count + " operands");
            return operands.ElementAt(index);
        }

        public override ExpressionPrecedence GetPrecedence()
        {
            return precedence;
        }

        public abstract string GetOperator();

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return UnEvaluatable;
        }
    }
}
