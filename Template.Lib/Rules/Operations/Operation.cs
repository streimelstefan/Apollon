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

        public Term Variable { get; private set; }

        public Operator Operator { get; set; }

        public Atom Condition { get; private set; }

        public Operation(Term variable, Operator @operator, Atom condition)
        {
            if (!variable.IsVariable)
            {
                throw new ArgumentException("Term needs to be a variable.", nameof(variable));
            }

            Variable = variable;
            Operator = @operator;
            Condition = condition;
        }

        public override string ToString()
        {
            var operationString = this.Operator == Operator.Equals ? "=" : "!=";

            return $"{Variable} {operationString} {Condition}";
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
            return new Operation((Term)Variable.Clone(), Operator, (Atom)Condition.Clone());
        }
    }
}
