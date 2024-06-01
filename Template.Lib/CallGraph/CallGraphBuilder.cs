//-----------------------------------------------------------------------
// <copyright file="CallGraphBuilder.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Graph;
using Apollon.Lib.Rules;

/// <summary>
/// This class is responsible for building the CallGraph from a given Program.
/// </summary>
public class CallGraphBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CallGraphBuilder"/> class.
    /// </summary>
    /// <param name="equalizer">The equalizer to use to build the callgraph.</param>
    public CallGraphBuilder(IEqualizer<Literal> equalizer)
    {
        this.CallGraph = new CallGraph(equalizer);
    }

    /// <summary>
    /// Gets or sets the CallGraph that is being built.
    /// </summary>
    private CallGraph CallGraph { get; set; }

    /// <summary>
    /// Builds and returns the CallGraph from the given Program.
    /// </summary>
    /// <param name="program">The program whose CallGraph shall be constructed.</param>
    /// <returns>The CallGraph of the Program.</returns>
    public CallGraph BuildCallGraph(Program program)
    {
        if (program == null)
        {
            throw new ArgumentNullException(nameof(program));
        }

        this.CallGraph = new CallGraph(this.CallGraph.Equalizer);

        IEnumerable<Statement> statements = program.RuleTypesAsStatements;
        this.CreateNodes(program.LiteralList);
        this.CreateEdges(statements.ToArray());

        return this.CallGraph;
    }

    /// <summary>
    /// Creates the Nodes of the CallGraph by iterating over the given Literal List. If a Literal is NAF negated, it will be ignored.
    /// </summary>
    /// <param name="literalList">The List of all Literals.</param>
    private void CreateNodes(Literal[] literalList)
    {
        foreach (Literal literal in literalList)
        {
            _ = this.FindOrAddNodeOfLiteral(literal);
        }
    }

    /// <summary>
    /// Creates the Edges of the CallGraph by iterating over the given Rule List. If a Literal is NAF negated, the weight of the Edge will be 1, otherwise 0 will be set.
    /// Edges are directional and go from the Head of the Rule to the Literals in the Body of the Rule. One Node can have multiple Edges. One Node can also have multiple connections to itself.
    /// </summary>
    /// <param name="ruleList">The List of Rules with which the Edges shall be created.</param>
    /// <exception cref="ArgumentException">Is Thrown if a Rule is invalid due to it having an unknown body or head.</exception>
    private void CreateEdges(Statement[] ruleList)
    {
        foreach (Statement rule in ruleList)
        {
            CallGraphNode? head = null;
            if (rule.Head != null)
            {
                head = this.FindOrAddNodeOfLiteral(rule.Head);
            }

            foreach (BodyPart bodyPart in rule.Body)
            {
                if (bodyPart.Literal == null)
                {
                    continue;
                }

                CallGraphNode target = this.FindOrAddNodeOfLiteral(bodyPart.Literal);

                _ = this.CallGraph.AddEdge(head, target, bodyPart.Literal.IsNAF, rule);
            }
        }
    }

    private CallGraphNode FindOrAddNodeOfLiteral(Literal literal)
    {
        Literal useLiteral = literal;
        if (useLiteral.IsNAF)
        {
            useLiteral = new Literal(literal.Atom, false, literal.IsNegative);
        }

        CallGraphNode? node = this.CallGraph.GetNode(useLiteral);

        node ??= this.CallGraph.AddNode(useLiteral);

        return node;
    }
}
