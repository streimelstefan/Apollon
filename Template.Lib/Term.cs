//-----------------------------------------------------------------------
// <copyright file="Term.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib
{
    /// <summary>
    /// This class represents a Term in a Statement.
    /// </summary>
    public class Term : IEquatable<Term>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Term"/> class.
        /// </summary>
        /// <param name="value">The string a term should represent.</param>
        public Term(string value)
            : this(value, new PVL())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Term"/> class.
        /// </summary>
        /// <param name="value">The string a term should represent.</param>
        /// <param name="pVL">An already existing PVL.</param>
        public Term(string value, PVL pVL)
        {
            this.Value = value;
            this.ProhibitedValues = new PVL();
            this.ProhibitedValues = pVL;
        }

        /// <summary>
        /// Gets or sets the List of all Prohibited Values.
        /// </summary>
        public PVL ProhibitedValues { get; set; } // Is this the correct place to put this?

        /// <summary>
        /// Gets a value indicating whether or not the Term is a Variable.
        /// </summary>
        public bool IsVariable => char.IsUpper(this.Value[0]);

        /// <summary>
        /// Gets or sets the Value of the Term.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Converts the Term to a String.
        /// </summary>
        /// <returns>A string representing the Term.</returns>
        public override string ToString()
        {
            return this.ProhibitedValues.Empty() ? this.Value : $"{this.Value} - {{{this.ProhibitedValues}}}";
        }

        /// <summary>
        /// Checks if the Term is equal to another Term.
        /// </summary>
        /// <param name="other">The Term of which equality should be checked for.</param>
        /// <returns>Returns a boolean indicating whether the Term is equal to another Term.</returns>
        public bool Equals(Term? other)
        {
            return other != null && this.Value == other.Value && this.ProhibitedValues.Equals(other.ProhibitedValues);
        }

        /// <summary>
        /// Checks whether the Term is negatively constrained.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the Term is negatively constrained.</returns>
        public bool IsNegativelyConstrained()
        {
            return !this.ProhibitedValues.Empty(); // It is negatively constrained if it has prohibited values
        }

        /// <summary>
        /// Clones the current Term.
        /// </summary>
        /// <returns>An object with the current term cloned.</returns>
        public object Clone()
        {
            return new Term((string)this.Value.Clone(), (PVL)this.ProhibitedValues.Clone());
        }
    }
}
