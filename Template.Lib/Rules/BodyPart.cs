//-----------------------------------------------------------------------
// <copyright file="BodyPart.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Rules
{
    using Apollon.Lib.Rules.Operations;

    /// <summary>
    /// Represents a body part of a rule.
    /// </summary>
    public class BodyPart : IEquatable<BodyPart>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BodyPart"/> class.
        /// </summary>
        /// <param name="literal">The literal that is contained in the body part.</param>
        /// <param name="operation">The operation that is contained in the body part.</param>
        /// <exception cref="ArgumentException">Is thrown when the literal and operation are both null.</exception>
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

            this.Literal = literal;
            this.Operation = operation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyPart"/> class.
        /// </summary>
        /// <param name="literal">The literal that is contained in the body part.</param>
        /// <param name="operation">The operation that is contained in the body part.</param>
        /// <param name="term">The Term of a ForAll Operation.</param>
        /// <param name="child">Child of the BodyPart if one exists.</param>
        /// <exception cref="ArgumentException">Is thrown when the body part is a forall and no literal or child is set.</exception>
        /// <exception cref="ArgumentException">Is thrown when the body part is a forall and operation is set.</exception>
        /// <exception cref="ArgumentException">Is thrown when operation is set and something else is set.</exception>
        /// <exception cref="ArgumentException">Is thrown when all parameters are null.</exception>
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

            if (operation == null && literal == null && term == null && child == null)
            {
                throw new ArgumentException("All parameters are not allowed to be null.");
            }

            this.Literal = literal;
            this.Operation = operation;
            this.ForAll = term;
            this.Child = child;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyPart"/> class.
        /// </summary>
        /// <param name="term">The forall term that is contained in the bodypart.</param>
        /// <param name="literal">The literal that is contained in the bodypart.</param>
        public BodyPart(Term term, Literal literal)
        {
            this.ForAll = term;
            this.Literal = literal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyPart"/> class.
        /// </summary>
        /// <param name="variable">The variable that is contained in the bodypart.</param>
        /// <param name="child">The child that is contained in the bodypart.</param>
        public BodyPart(Term variable, BodyPart child)
        {
            this.ForAll = variable;
            this.Child = child;
        }

        /// <summary>
        /// Gets the literal of the body part.
        /// </summary>
        public Literal? Literal { get; private set; }

        /// <summary>
        /// Gets the operation of the body part.
        /// </summary>
        public Operation? Operation { get; private set; }

        /// <summary>
        /// Gets or sets the ForAll term of the body part if one exists.
        /// </summary>
        public Term? ForAll { get; set; }

        /// <summary>
        /// Gets the child of the body part if one exists.
        /// </summary>
        public BodyPart? Child { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the body part is a literal.
        /// </summary>
        public bool IsLiteral => this.Literal != null;

        /// <summary>
        /// Gets a value indicating whether the body part is an operation.
        /// </summary>
        public bool IsOperation => this.Operation != null;

        /// <summary>
        /// Gets a value indicating whether the body part is a forall.
        /// </summary>
        public bool IsForAll => this.ForAll != null;

        /// <summary>
        /// Gets a value indicating whether the body part has a child.
        /// </summary>
        public bool HasChild => this.Child != null;

        /// <summary>
        /// Checks whether the current body part is equal to the given body part.
        /// </summary>
        /// <param name="other">The other bodypart that should be compared to.</param>
        /// <returns>Returns a value indicating whether the current body part is equal to the given body part.</returns>
        public bool Equals(BodyPart? other)
        {
            return other != null
&& (this == other
|| ((!other.IsLiteral || this.IsLiteral)
&& (!other.IsOperation || this.IsOperation)
&& (this.Literal != null ? this.Literal.Equals(other.Literal) : this.Operation != null && this.Operation.Equals(other.Operation))));
        }

        /// <summary>
        /// Converts the body part to a string.
        /// </summary>
        /// <returns>Returns a string representing a body part.</returns>
        public override string ToString()
        {
            return this.ForAll != null
                ? $"forall({this.ForAll}, {(this.Child == null ? this.Literal : this.Child)})"
                : this.Literal != null ? this.Literal.ToString() : this.Operation != null ? this.Operation.ToString() : string.Empty;
        }

        /// <summary>
        /// Clones the current object.
        /// </summary>
        /// <returns>A cloned object of the current bodypart.</returns>
        public object Clone()
        {
            Literal? literal = null;
            Operation? operation = null;
            Term? forall = null;
            BodyPart? child = null;

            if (this.Literal != null)
            {
                literal = (Literal)this.Literal.Clone();
            }

            if (this.Operation != null)
            {
                operation = (Operation)this.Operation.Clone();
            }

            if (this.Child != null)
            {
                child = (BodyPart)this.Child.Clone();
            }

            if (this.ForAll != null)
            {
                forall = (Term)this.ForAll.Clone();
            }

            return new BodyPart(literal, operation, forall, child);
        }
    }
}
