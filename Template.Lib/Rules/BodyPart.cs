using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules
{
    public class BodyPart : IEquatable<BodyPart>, ICloneable
    {

        public Literal? Literal { get; private set; }

        public Operation? Operation { get; private set; }

        public Term? ForAll { get; set; }

        public BodyPart? Child {  get; private set; }

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

        public bool IsForAll
        {
            get
            {
                return ForAll != null;
            }
        }

        public bool HasChild
        {
            get
            {
                return Child != null;
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

        public BodyPart(Literal? literal, Operation? operation, Term? term, BodyPart? child)
        {
            if (term != null && literal == null && child == null)
            {
                throw new ArgumentException("If body part is forall either literal or child need to be set.");
            }
            if (term != null && operation != null)
            {
                throw new ArgumentException("If body part is forall operation is not allowed to be set.");
            }
            if (operation != null && (literal != null || term != null || child != null))
            {
                throw new ArgumentException("If operation is set nothing else is allowed to be set.");
            }
            if (operation == null && literal == null && term == null &&  child == null)
            {
                throw new ArgumentException("All parameters are not allowed to be null.");
            }

            Literal = literal;
            Operation = operation;
            ForAll = term;
            Child = child;
        }

        public BodyPart(Term term, Literal literal)
        {
            ForAll = term;
            Literal = literal;
        }

        public BodyPart(Term variable, BodyPart child)
        {
            ForAll = variable;
            Child = child;
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
            if (ForAll != null)
            {
                return $"forall({ForAll}, {(Child == null ? Literal : Child)})";
            }
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

        public object Clone()
        {
            Literal? literal = null;
            Operation? operation = null;
            Term? forall = null;
            BodyPart? child = null;

            if (Literal != null)
            {
                literal = (Literal)Literal.Clone();
            }
            if (Operation != null)
            {
                operation = (Operation)Operation.Clone();
            }
            if (Child != null)
            {
                child = (BodyPart)Child.Clone();
            }
            if (ForAll != null) 
            {
                forall = (Term)ForAll.Clone();
            }

            return new BodyPart(literal, operation, forall, child);
        }
    }
}
