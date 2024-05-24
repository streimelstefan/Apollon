using Apollon.Lib.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules.Operations
{
    public class Operation : IEquatable<Operation>, ICloneable
    {
        public Term? OutputtingVariable { get; set; } 

        public AtomParam Variable { get; set; }

        public Operator Operator { get; set; }

        public AtomParam Condition { get; private set; }

        public bool IsNAF { get; set; }

        public Operation(AtomParam variable, Operator @operator, AtomParam condition)
        {
            if (variable.Term != null && !variable.Term.IsVariable)
            {
                throw new ArgumentException("Term needs to be a variable.", nameof(variable));
            }

            Variable = variable;
            Operator = @operator;
            Condition = condition;
        }

        public Operation(Term outputtingVariable, AtomParam variable, Operator @operator, Term condition)
        {
            if (!variable.IsTerm)
            {
                throw new ArgumentException("variable needs to be a term.");
            }
            if (!outputtingVariable.IsVariable)
            {
                throw new ArgumentException("outputting variables needs to be a variable.");
            }

            OutputtingVariable = outputtingVariable;
            Variable = variable;
            Operator = @operator;
            Condition = new AtomParam(condition);
        }

        public override string ToString()
        {
            var operationString = Operator.ToFriendlyString();
            if (OutputtingVariable == null)
            {
                return $"{Variable} {operationString} {Condition}";
            } else
            {
                return $"{(IsNAF ? "not " : string.Empty)}{OutputtingVariable} is {Variable} {operationString} {Condition}";
            }
        }

        public bool Equals(Operation? other)
        {
            if (other == null) return false;
            if (other == this) return true;

            return this.Variable.Equals(other.Variable) &&
                this.Operator == other.Operator &&
                this.Condition.Equals(other.Condition);
        }

        public object Clone()
        {
            if (this.OutputtingVariable == null)
            {
                return new Operation((AtomParam)Variable.Clone(), Operator, (AtomParam)Condition.Clone());
            } else
            {
                var op = new Operation((Term)OutputtingVariable.Clone(), (AtomParam)Variable.Clone(), Operator, (Term)Condition.Term.Clone());
                op.IsNAF = this.IsNAF;
                return op;
            }
        }
    }
}
