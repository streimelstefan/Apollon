using Apollon.Lib.CallGraph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;
using Template.Lib.Graph;
using Template.Lib.Rules;

namespace Template.Test;

public class CallGraphBuilderTests
{
    private CallGraphBuilder? _callGraphBuilder;

    [SetUp]
    public void Setup()
    {
        _callGraphBuilder = new CallGraphBuilder(new LiteralTermCountEqualizer());
    }

    [Test]
    public void ShouldBuildCallGraph()
    {
        var literal = new Literal(new Atom("human", new Term[] { new Term("V") }), false, false);
        var literal2 = new Literal(new Atom("informatiker", new Term[] { new Term("V") }), false, false);

        var program = new Program(new Literal[] { literal, literal2 }, new Rule[] { new Rule(literal, literal2) });
        var graph = _callGraphBuilder?.BuildCallGraph(program);

        //Assert.AreEqual(new List<CallGraphNode> { new CallGraphNode(literal), new CallGraphNode(literal2)}, graph.Nodes);
        //Assert.AreEqual(new List<CallGraphEdge> { new CallGraphEdge(new CallGraphNode(literal), new CallGraphNode(literal2), 1, new Rule(literal, literal2)) }, graph.Edges);
        Assert.AreEqual(graph?.Nodes.Count, 2);
        Assert.AreEqual(graph?.Edges.Count, 1);
    }

    [Test]
    public void ShouldBuildCallGraphCorrectly()
    {
        var literals = new Literal[] { new Literal(new Atom("atom", new Term[] { }), false, false) };
        var rules = new Rule[] { new Rule(new Literal(new Atom("atom", new Term[] { new Term("V") }), false, false), new Literal[] { new Literal(new Atom("atom", new Term[] { }), false, false) }) };
        var prgram = new Program(literals, rules);

        var graph = _callGraphBuilder?.BuildCallGraph(prgram);

        Assert.AreEqual(graph?.Nodes.Count, 2);
        Assert.AreEqual(graph?.Edges.Count, 1);

        var testLiteral = new Literal(new Atom("atom", new Term[] { }), false, false);
        var testLiteral2 = new Literal(new Atom("atom", new Term[] { new Term("V") }), false, false);
        Assert.NotNull(graph?.GetNode(testLiteral));
        Assert.NotNull(graph?.GetNode(testLiteral2));

        var edge = graph?.Edges.First();

        Assert.NotNull(edge);
        Assert.IsFalse(edge?.IsNAF);
        Assert.AreEqual(edge?.CreatorRule, rules[0]);
        Assert.IsTrue(edge?.Source.Literal.Equals(testLiteral2));
        Assert.IsTrue(edge?.Target.Literal.Equals(testLiteral));
    }

    [Test]
    public void ShouldBuildCallGraphCorrectlyWithNAF()
    {
        var literals = new Literal[] { new Literal(new Atom("atom", new Term[] { }), false, false) };
        var rules = new Rule[] { new Rule(new Literal(new Atom("atom", new Term[] { new Term("V") }), false, false), new Literal[] { new Literal(new Atom("atom", new Term[] { }), true, false) }) };
        var prgram = new Program(literals, rules);

        var graph = _callGraphBuilder?.BuildCallGraph(prgram);

        Assert.AreEqual(graph?.Nodes.Count, 2);
        Assert.AreEqual(graph?.Edges.Count, 1);

        var testLiteral = new Literal(new Atom("atom", new Term[] { }), false, false);
        var testLiteral2 = new Literal(new Atom("atom", new Term[] { new Term("V") }), false, false);
        Assert.NotNull(graph?.GetNode(testLiteral));
        Assert.NotNull(graph?.GetNode(testLiteral2));

        var edge = graph?.Edges.First();

        Assert.NotNull(edge);
        Assert.IsTrue(edge?.IsNAF);
        Assert.AreEqual(edge?.CreatorRule, rules[0]);
        Assert.IsTrue(edge?.Source.Literal.Equals(testLiteral2));
        Assert.IsTrue(edge?.Target.Literal.Equals(testLiteral));
    }
}
