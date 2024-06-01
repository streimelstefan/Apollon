//-----------------------------------------------------------------------
// <copyright file="OlonSet.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.OLON
{
    using Apollon.Lib.Graph;

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
        /// <returns>Whether or not the node is part of an olon.</returns>
        public bool IsPartOfOlon(CallGraphNode node)
        {
            return this.Nodes.Contains(node);
        }
    }
}
