using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Graph;

/// <summary>
/// Represents an edge within the <see cref="CallGraph"/>.
/// </summary>
public class CallGraphEdge
{
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
    /// Initializes a new instance of the <see cref="CallGraphEdge"/> class.
    /// </summary>
    /// <param name="source">The source node of the edge.</param>
    /// <param name="target">The target node of the edge.</param>
    /// <param name="isNaf">Whether or not the edge is a NAF edge.</param>
    /// <param name="creatorRule">The rule that created the edge.</param>
    public CallGraphEdge(CallGraphNode? source, CallGraphNode target, bool isNaf, Statement creatorRule)
    {
        Source = source;
        Target = target;
        CreatorRule = creatorRule;
        IsNAF = isNaf;
    }

    /// <summary>
    /// Returns a string representation of the edge.
    /// </summary>
    /// <returns>The string representation of the edge.</returns>
    public override string ToString()
    {
        return $"{Source} -{(IsNAF ? "NAF" : string.Empty)}> {Target}";
    }
}
