using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib.CallGraph;

/// <summary>
/// This class is responsible for building the CallGraph from a given Program.
/// </summary>
public class CallGraphBuilder
{
    /// <summary>
    /// The CallGraph that is being built.
    /// </summary>
    private CallGraph CallGraph { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CallGraphBuilder"/> class.
    /// </summary>
    public CallGraphBuilder()
    {
        CallGraph = new CallGraph();
    }

    /*
     * Der Call Graph ist ein gewichteter gerichteter Graph. Wobei jeder Knoten des Graphen ein positives Literal eines Programms ist. x

        Die Kanten gehen von Regelkopf literal aus uns zeigen auf die Literale, die benötigt werden um diese Regel überprüfen. x

        Kanten haben entweder eine Gewichtung von 1 oder 0. 1 Für Literale, die mit not negiert werden. 0 für normale positive Literale.  x

        Kanten merken sich auch, von welcher Regel sie erstellt worden sind. Dazu sollte am besten eine UUID an alle Regeln vergeben werden. x 

        Ein positives Literal sind alle Literale, die kein not vor sich stehen haben. Also auch Literale, die mit - beginnen.   x
     */
    /// <summary>
    /// Builds and returns the CallGraph from the given Program.
    /// </summary>
    /// <param name="program">The program whose CallGraph shall be constructed.</param>
    /// <returns>The CallGraph of the Program.</returns>
    public CallGraph BuildCallGraph(Program program)
    {
        if (program == null) throw new ArgumentNullException(nameof(program));

        CallGraph = new CallGraph();

        CreateNodes(program.LiteralList);
        CreateEdges(program.RuleList);

        return CallGraph;
    }

    /// <summary>
    /// Creates the Nodes of the CallGraph by iterating over the given Literal List. If a Literal is NAF negated, it will be ignored.
    /// </summary>
    /// <param name="literalList">The List of all Literals.</param>
    private void CreateNodes(Literal[] literalList)
    {
        foreach(var literal in literalList)
        {
            if (!literal.IsNAF)
            {
                CallGraph.AddNode(new CallGraphNode (literal));
            } // not really sure what to do with the negated literals
        }
    }

    /// <summary>
    /// Creates the Edges of the CallGraph by iterating over the given Rule List. If a Literal is NAF negated, the weight of the Edge will be 1, otherwise 0 will be set.
    /// Edges are directional and go from the Head of the Rule to the Literals in the Body of the Rule. One Node can have multiple Edges. One Node can also have multiple connections to itself.
    /// </summary>
    /// <param name="ruleList">The List of Rules with which the Edges shall be created.</param>
    /// <exception cref="ArgumentException">Is Thrown if a Rule is invalid due to it having an unknown body or head.</exception>
    private void CreateEdges(Rule[] ruleList)
    {
        foreach(var rule in ruleList)
        {
            var head = CallGraph.GetNode(rule.Head) ?? throw new ArgumentException("Rule is not valid since Head was not found.");

            foreach(var literal in rule.Body)
            {
                int weight = literal.IsNAF ? 1 : 0;
                var target = CallGraph.GetNode(literal) ?? throw new ArgumentException("Rule is not valid since Body was not found.");


                CallGraph.AddEdge(new CallGraphEdge(head, target, weight, rule));
            }
        }
    }

    
}
