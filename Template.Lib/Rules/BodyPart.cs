using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules
{
    public class BodyPart : IEquatable<BodyPart>
    {

        public Literal? Literal { get; private set; }

        public Operation? Operation { get; private set; }

        public bool IsLiteral
        {
            get
            {
                return Literal != null;
            }
        }

        public bool IsOperation
        {
            get
            {
                return Operation != null;
            }
        }

        public BodyPart(Literal? literal, Operation? operation) 
        {
            if (literal == null && operation == null) 
            {
                throw new ArgumentException("Literal and operation cannot be null at the same time.");
            }
            if (literal != null && operation != null)
            {
                throw new ArgumentException("Literal and operation cannot be set at the same time.");
            }

            Literal = literal;
            Operation = operation;
        }

        public bool Equals(BodyPart? other)
        {
            if (other == null) return false;
            if (this == other) return true;

            if (other.IsLiteral && !IsLiteral) return false;
            if (other.IsOperation && !IsOperation) return false;

            if (Literal != null)
            {
                return Literal.Equals(other.Literal);
            }

            if (Operation != null)
            {
                return Operation.Equals(other.Operation);
            }

            return false;
        }

        public override string ToString()
        {
            if (Literal != null)
            {
                return Literal.ToString();
            }
            if (Operation != null)
            {

                return Operation.ToString();
            }

            return "";
        }
    }
}
