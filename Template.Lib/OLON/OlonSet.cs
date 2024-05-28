using Apollon.Lib.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.OLON
{
    /// <summary>
    /// Represents a set of nodes that are part of an OLON.
    /// </summary>
    public class OlonSet
    {
        /// <summary>
        /// Gets all nodes that are part of the OLON.
        /// </summary>
        public HashSet<CallGraphNode> Nodes { get; private set; } = new HashSet<CallGraphNode>();

        /// <summary>
        /// Checks if a given node is part of the OLON.
        /// </summary>
        /// <param name="node">The node to check.</param>
        /// <returns>Whether or not the node is part of an olon</returns>
        public bool IsPartOfOlon(CallGraphNode node)
        {
            return Nodes.Contains(node);
        }

    }
}
