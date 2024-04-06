using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;
using Template.Lib.CallGraph;

namespace Template.Test;

[TestFixture]
public class CallGraphTests
{
    private CallGraph _graph;

    [SetUp]
    public void Setup()
    {
        _graph = new CallGraph();
    }
    

    [Test]
    public void ShouldSetHead()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node = new CallGraphNode(literal);
        _graph.AddNode(node);

        Assert.AreEqual(literal, _graph.Root.Literal);
    }

    [Test]
    public void ShouldNotSetHead()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node = new CallGraphNode(literal);
        _graph.AddNode(node);

        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node2 = new CallGraphNode(literal2);
        _graph.AddNode(node2);

        Assert.AreNotEqual(literal2, _graph.Root);
    }

    [Test]
    public void ShouldAddEdge()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), false, false);
        var node = new CallGraphNode(literal);
        _graph.AddNode(node);

        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node2 = new CallGraphNode(literal2);
        _graph.AddNode(node2);

        _graph.AddEdge(new CallGraphEdge(node, node2, 1, new Rule(literal, literal2)));

        Assert.AreEqual(1, _graph.Edges.Count);
    }

    [Test]
    public void ShouldThrowIfNodeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _graph.AddNode(null));
    }

    [Test]
    public void ShouldThrowIfEdgeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _graph.AddEdge(null));
    }

    [Test]
    public void ShouldThrowIfEdgeSourceIsNotInGraph()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node = new CallGraphNode(literal);

        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node2 = new CallGraphNode(literal2);
        _graph.AddNode(node2);

        Assert.Throws<ArgumentException>(() => _graph.AddEdge(new CallGraphEdge(node, node2, 1, new Rule(literal, literal2))));
    }

    [Test]
    public void ShouldThrowIfEdgeTargetIsNotInGraph()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node = new CallGraphNode(literal);
        _graph.AddNode(node);

        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node2 = new CallGraphNode(literal2);

        Assert.Throws<ArgumentException>(() => _graph.AddEdge(new CallGraphEdge(node, node2, 1, new Rule(literal, literal2))));
    }

    [Test]
    public void ShouldThrowIfNodeAlreadyExists()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node = new CallGraphNode(literal);
        _graph.AddNode(node);

        Assert.Throws<ArgumentException>(() => _graph.AddNode(node));
    }

    [Test]
    public void ShouldThrowIfEdgeAlreadyExists()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), false, false);
        var node = new CallGraphNode(literal);
        _graph.AddNode(node);

        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node2 = new CallGraphNode(literal2);
        _graph.AddNode(node2);

        var edge = new CallGraphEdge(node, node2, 1, new Rule(literal, literal2));
        _graph.AddEdge(edge);

        Assert.Throws<ArgumentException>(() => _graph.AddEdge(edge));
    }
}
