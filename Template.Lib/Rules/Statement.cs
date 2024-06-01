//-----------------------------------------------------------------------
// <copyright file="Statement.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Rules
{
    /// <summary>
    /// Represents a Statement.
    /// </summary>
    public class Statement : IEquatable<Statement>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Statement"/> class.
        /// </summary>
        /// <param name="head">The head of the statement.</param>
        /// <param name="body">The body of the statement.</param>
        public Statement(Literal? head, params BodyPart[] body)
        {
            this.Head = head;
            this.Body = body;
        }

        /// <summary>
        /// Gets or sets the Head of the Statement.
        /// </summary>
        public Literal? Head { get; set; }

        /// <summary>
        /// Gets or sets the Body of the Statement.
        /// </summary>
        public BodyPart[] Body { get; set; }

        /// <summary>
        /// Converts the Statement to a string.
        /// </summary>
        /// <returns>A string representing the statement.</returns>
        public override string ToString()
        {
            return $"{this.Head} :- {string.Join(", ", this.Body.Select(literal => literal.ToString()))}.";
        }

        /// <summary>
        /// Checks if the given object is equal to this Statement.
        /// </summary>
        /// <param name="other">The other statement.</param>
        /// <returns>A bool indicating whether or not the statement is equal to another statement.</returns>
        public bool Equals(Statement? other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Head == null && this.Head != other.Head)
            {
                return false;
            }

            if ((other.Head != null && this.Head != null && !other.Head.Equals(this.Head)) || other.Body.Length != this.Body.Length)
            {
                return false;
            }

            // check if the term list is the same
            for (int i = 0; i < this.Body.Length; i++)
            {
                if (!this.Body[i].Equals(other.Body[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Clones the current statement.
        /// </summary>
        /// <returns>The cloned statement.</returns>
        public virtual object Clone()
        {
            Literal? head = null;
            if (this.Head != null)
            {
                head = (Literal)this.Head.Clone();
            }

            return new Statement(head, this.Body.Select(bp => (BodyPart)bp.Clone()).ToArray());
        }
    }
}
