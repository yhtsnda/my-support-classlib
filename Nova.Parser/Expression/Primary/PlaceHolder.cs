using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class PlaceHolder : PrimaryExpression
    {
        public PlaceHolder(string name, string nameup)
        {
            this.Name = name;
            this.NameUp = nameup;
        }

        public string Name { get; protected set; }
        public string NameUp { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<PlaceHolder>(this);
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return base.EvaluationInternal(parameters);
        }

        public override IExpression SetCacheEvalRst()
        {
            return null;
        }

        public override object Evaluation<K, V>(IDictionary<K, V> parameters)
        {
            return null;
        }
    }
}
