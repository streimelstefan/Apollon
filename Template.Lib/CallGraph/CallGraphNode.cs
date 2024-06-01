//-----------------------------------------------------------------------
// <copyright file="CallGraphNode.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Graph
{
    /// <summary>
    /// Represents a Node within the <see cref="CallGraph"/>.
    /// </summary>
    public class CallGraphNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallGraphNode"/> class.
        /// </summary>
        /// <param name="literal">The literal the represents this node.</param>
        public CallGraphNode(Literal literal)
        {
            this.Literal = literal;
        }

        /// <summary>
        /// Gets or sets the <see cref="Literal"/> that represents the Node.
        /// </summary>
        public Literal Literal { get; set; }

        /// <summary>
        /// Returns the string representation of the Node.
        /// </summary>
        /// <returns>The string representation of the Node.</returns>
        public override string ToString()
        {
            return this.Literal.ToString();
        }
    }
}
