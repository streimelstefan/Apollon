//-----------------------------------------------------------------------
// <copyright file="StatementGroup.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.DualRules
{
    using Apollon.Lib.Rules;

    /// <summary>
    /// Groups statements according to their head literal based on the variable count.
    /// </summary>
    public class StatementGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatementGroup"/> class.
        /// </summary>
        /// <param name="statement">All the statements that belong to this group.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the head of the statment is null.</exception>
        public StatementGroup(Statement statement)
        {
            if (statement.Head == null)
            {
                throw new ArgumentNullException(nameof(statement), "The head of a statement is not allowed to be null in a statement group");
            }

            this.Statements = new List<Statement>();
            this.ReferenceLiteral = statement.Head;
            this.Statements.Add(statement);
        }

        /// <summary>
        /// Gets the statements that belong to this group.
        /// </summary>
        public List<Statement> Statements { get; private set; }

        /// <summary>
        /// Gets the reference literal that represents the head of the first statement that was inserted
        /// into this group. It should be used to see if a given statement belongs in this group or not.
        /// </summary>
        public Literal ReferenceLiteral { get; private set; }
    }
}
