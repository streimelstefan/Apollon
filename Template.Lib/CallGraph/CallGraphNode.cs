using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Graph
{
    /// <summary>
    /// Represents a Node within the <see cref="CallGraph"/>.
    /// </summary>
    public class CallGraphNode
    {

        /// <summary>
        /// The <see cref="Literal"/> that represents the Node.
        /// </summary>
        public Literal Literal { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallGraphNode"/> class.
        /// </summary>
        /// <param name="literal">The literal the represents this node.</param>
        public CallGraphNode(Literal literal)
        {
            Literal = literal;
        }

        /// <summary>
        /// Returns the string representation of the Node.
        /// </summary>
        /// <returns>The string representation of the Node.</returns>
        public override string ToString()
        {
            return Literal.ToString();
        }
    }
}
