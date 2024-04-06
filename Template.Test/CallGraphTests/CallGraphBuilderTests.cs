using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;
using Template.Lib.CallGraph;

namespace Template.Test;

public class CallGraphBuilderTests
{
    private CallGraphBuilder _callGraphBuilder;

    [SetUp]
    public void Setup()
    {
        _callGraphBuilder = new CallGraphBuilder();
    }

    [Test]
    public void ShouldThrowIfProgramIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _callGraphBuilder.BuildCallGraph(null));
    }

    [Test]
    public void ShouldThrowIfHeadWasNotFound()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), false, false);
        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var fakeliteral = new Literal(new Atom("fake", new Term[] { new Term("V") }), false, false);

        var program = new Program(new Literal[] {literal, literal2}, new Rule[] {new Rule(fakeliteral, literal2)});
        Assert.Throws<ArgumentException>(() => _callGraphBuilder.BuildCallGraph(program));
    }

    [Test]
    public void ShouldThrowIfBodyWasNotFound()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), false, false);
        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var fakeliteral = new Literal(new Atom("fake", new Term[] { new Term("V") }), true, false);

        var program = new Program(new Literal[] { literal, literal2 }, new Rule[] { new Rule(literal, fakeliteral) });
        Assert.Throws<ArgumentException>(() => _callGraphBuilder.BuildCallGraph(program));
    }

    [Test]
    public void ShouldBuildCallGraph()
    {
        var literal = new Literal(new Atom("human", new Term[] { new Term("V") }), false, false);
        var literal2 = new Literal(new Atom("informatiker", new Term[] { new Term("V") }), false, false);

        var program = new Program(new Literal[] { literal, literal2 }, new Rule[] { new Rule(literal, literal2) });
        var graph = _callGraphBuilder.BuildCallGraph(program);

        //Assert.AreEqual(new List<CallGraphNode> { new CallGraphNode(literal), new CallGraphNode(literal2)}, graph.Nodes);
        //Assert.AreEqual(new List<CallGraphEdge> { new CallGraphEdge(new CallGraphNode(literal), new CallGraphNode(literal2), 1, new Rule(literal, literal2)) }, graph.Edges);
        Assert.AreEqual(graph.Nodes.Count, 2);
        Assert.AreEqual(graph.Edges.Count, 1);
    }
}
