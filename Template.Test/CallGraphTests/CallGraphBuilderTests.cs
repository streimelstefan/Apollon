namespace Template.Test;
using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Docu;
using Apollon.Lib.Graph;
using Apollon.Lib.Rules;
using NUnit.Framework;
using System.Linq;

public class CallGraphBuilderTests
{
    private CallGraphBuilder? callGraphBuilder;

    [SetUp]
    public void Setup()
    {
        this.callGraphBuilder = new CallGraphBuilder(new LiteralParamCountEqualizer());
    }

    [Test]
    public void ShouldBuildCallGraph()
    {
        Literal literal = new(new Atom("human", new AtomParam[] { new(null, new Term("V")) }), false, false);
        Literal literal2 = new(new Atom("informatiker", new AtomParam[] { new(null, new Term("V")) }), false, false);

        Program program = new(new Literal[] { literal, literal2 }, new Rule[] { new(literal, new BodyPart(literal2, null)) }, new Constraint[0], new Documentation[0]);
        CallGraph? graph = this.callGraphBuilder?.BuildCallGraph(program);

        // Assert.AreEqual(new List<CallGraphNode> { new CallGraphNode(literal), new CallGraphNode(literal2)}, graph.Nodes);
        // Assert.AreEqual(new List<CallGraphEdge> { new CallGraphEdge(new CallGraphNode(literal), new CallGraphNode(literal2), 1, new Rule(literal, literal2)) }, graph.Edges);
        Assert.AreEqual(graph?.Nodes.Count, 2);
        Assert.AreEqual(graph?.Edges.Count, 1);
    }

    [Test]
    public void ShouldBuildCallGraphCorrectly()
    {
        Literal[] literals = new Literal[] { new(new Atom("atom", new AtomParam[] { }), false, false) };
        Rule[] rules = new Rule[] { new(new Literal(new Atom("atom", new AtomParam[] { new(null, new Term("V")) }), false, false), new BodyPart[] { new(new Literal(new Atom("atom", new AtomParam[] { }), false, false), null) }) };
        Program prgram = new(literals, rules, new Constraint[0], new Documentation[0]);

        CallGraph? graph = this.callGraphBuilder?.BuildCallGraph(prgram);

        Assert.AreEqual(graph?.Nodes.Count, 2);
        Assert.AreEqual(graph?.Edges.Count, 1);

        Literal testLiteral = new(new Atom("atom", new AtomParam[] { }), false, false);
        Literal testLiteral2 = new(new Atom("atom", new AtomParam[] { new(null, new Term("V")) }), false, false);
        Assert.NotNull(graph?.GetNode(testLiteral));
        Assert.NotNull(graph?.GetNode(testLiteral2));

        CallGraphEdge? edge = graph?.Edges.First();

        Assert.NotNull(edge);
        Assert.IsFalse(edge?.IsNAF);
        Assert.AreEqual(edge?.CreatorRule, rules[0]);
        Assert.IsTrue(edge?.Source.Literal.Equals(testLiteral2));
        Assert.IsTrue(edge?.Target.Literal.Equals(testLiteral));
    }

    [Test]
    public void ShouldBuildCallGraphCorrectlyWithNAF()
    {
        Literal[] literals = new Literal[] { new(new Atom("atom", new AtomParam[] { }), false, false) };
        Rule[] rules = new Rule[] { new(new Literal(new Atom("atom", new AtomParam[] { new(null, new Term("V")) }), false, false), new BodyPart[] { new(new Literal(new Atom("atom", new AtomParam[] { }), true, false), null) }) };
        Program prgram = new(literals, rules, new Constraint[0], new Documentation[0]);

        CallGraph? graph = this.callGraphBuilder?.BuildCallGraph(prgram);

        Assert.AreEqual(graph?.Nodes.Count, 2);
        Assert.AreEqual(graph?.Edges.Count, 1);

        Literal testLiteral = new(new Atom("atom", new AtomParam[] { }), false, false);
        Literal testLiteral2 = new(new Atom("atom", new AtomParam[] { new(null, new Term("V")) }), false, false);
        Assert.NotNull(graph?.GetNode(testLiteral));
        Assert.NotNull(graph?.GetNode(testLiteral2));

        CallGraphEdge? edge = graph?.Edges.First();

        Assert.NotNull(edge);
        Assert.IsTrue(edge?.IsNAF);
        Assert.AreEqual(edge?.CreatorRule, rules[0]);
        Assert.IsTrue(edge?.Source.Literal.Equals(testLiteral2));
        Assert.IsTrue(edge?.Target.Literal.Equals(testLiteral));
    }
}
