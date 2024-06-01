//-----------------------------------------------------------------------
// <copyright file="CallGraphEdge.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Graph;
using Apollon.Lib.Rules;

/// <summary>
/// Represents an edge within the <see cref="CallGraph"/>.
/// </summary>
public class CallGraphEdge
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CallGraphEdge"/> class.
    /// </summary>
    /// <param name="source">The source node of the edge.</param>
    /// <param name="target">The target node of the edge.</param>
    /// <param name="isNaf">Whether or not the edge is a NAF edge.</param>
    /// <param name="creatorRule">The rule that created the edge.</param>
    public CallGraphEdge(CallGraphNode? source, CallGraphNode target, bool isNaf, Statement creatorRule)
    {
        this.Source = source;
        this.Target = target;
        this.CreatorRule = creatorRule;
        this.IsNAF = isNaf;
    }

    /// <summary>
    /// Gets or sets the source of the edge.
    /// </summary>
    public CallGraphNode? Source { get; set; }

    /// <summary>
    /// Gets or sets the target of the edge.
    /// </summary>
    public CallGraphNode Target { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the edge is a NAF edge.
    /// </summary>
    public bool IsNAF { get; set; }

    /// <summary>
    /// Gets or sets the statement that created the edge.
    /// </summary>
    public Statement CreatorRule { get; set; }

    /// <summary>
    /// Returns a string representation of the edge.
    /// </summary>
    /// <returns>The string representation of the edge.</returns>
    public override string ToString()
    {
        return $"{this.Source} -{(this.IsNAF ? "NAF" : string.Empty)}> {this.Target}";
    }
}
