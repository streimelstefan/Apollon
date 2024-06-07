//-----------------------------------------------------------------------
// <copyright file="Operation.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Rules.Operations
{
    using Apollon.Lib.Atoms;

    /// <summary>
    /// A class representing an operation.
    /// </summary>
    public class Operation : IEquatable<Operation>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Operation"/> class.
        /// </summary>
        /// <param name="variable">The variable contained in this operation.</param>
        /// <param name="operator">The operator that is used in the operation.</param>
        /// <param name="condition">The condition that is used in the operation.</param>
        /// <exception cref="ArgumentException">Is thrown when the Term is not a variable.</exception>
        public Operation(AtomParam variable, Operator @operator, AtomParam condition)
        {
            if (variable.Term != null && !variable.Term.IsVariable)
            {
                throw new ArgumentException("Term needs to be a variable.", nameof(variable));
            }

            this.Variable = variable;
            this.Operator = @operator;
            this.Condition = condition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Operation"/> class.
        /// </summary>
        /// <param name="outputtingVariable">The variable that is outputted by the operation.</param>
        /// <param name="variable">The variable contained in this operation.</param>
        /// <param name="operator">The operator that is used in the operation.</param>
        /// <param name="condition">The condition that is used in the operation.</param>
        /// <exception cref="ArgumentException">Is thrown when the Term is not a variable.</exception>
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

            this.OutputtingVariable = outputtingVariable;
            this.Variable = variable;
            this.Operator = @operator;
            this.Condition = new AtomParam(condition);
        }

        /// <summary>
        /// Gets or sets the variable that is outputted by the operation.
        /// </summary>
        public Term? OutputtingVariable { get; set; }

        /// <summary>
        /// Gets or sets the variable that is used in the operation.
        /// </summary>
        public AtomParam Variable { get; set; }

        /// <summary>
        /// Gets or sets the operator that is used in the operation.
        /// </summary>
        public Operator Operator { get; set; }

        /// <summary>
        /// Gets the condition that is used in the operation.
        /// </summary>
        public AtomParam Condition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation is a NAF operation.
        /// </summary>
        public bool IsNAF { get; set; }

        /// <summary>
        /// Converts the operation to a string.
        /// </summary>
        /// <returns>A string representing the operation.</returns>
        public override string ToString()
        {
            string operationString = this.Operator.ToFriendlyString();
            return this.OutputtingVariable == null
                ? $"{this.Variable} {operationString} {this.Condition}"
                : $"{(this.IsNAF ? "not " : string.Empty)}{this.OutputtingVariable} is {this.Variable} {operationString} {this.Condition}";
        }

        /// <summary>
        /// Checks if the operation is equal to another operation.
        /// </summary>
        /// <param name="other">The other operation.</param>
        /// <returns>A boolean indicating whether this operation is equal to the given operation.</returns>
        public bool Equals(Operation? other)
        {
            return other != null
&& (other == this
|| (this.Variable.Equals(other.Variable) &&
                this.Operator == other.Operator &&
                this.Condition.Equals(other.Condition)));
        }

        /// <summary>
        /// Clones the operation.
        /// </summary>
        /// <returns>A cloned object of this operation.</returns>
        public object Clone()
        {
            if (this.OutputtingVariable == null)
            {
                return new Operation((AtomParam)this.Variable.Clone(), this.Operator, (AtomParam)this.Condition.Clone());
            }
            else
            {
                Operation op = new((Term)this.OutputtingVariable.Clone(), (AtomParam)this.Variable.Clone(), this.Operator, (Term)this.Condition.Term!.Clone())
                {
                    IsNAF = this.IsNAF,
                };
                return op;
            }
        }
    }
}
