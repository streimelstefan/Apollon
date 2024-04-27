using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;
using Apollon.Lib.Graph;
using Apollon.Lib.Rules;
using Apollon.Lib.Atoms;

namespace Template.Test;

[TestFixture]
public class CallGraphTests
{
    private CallGraph? _graph;

    [SetUp]
    public void Setup()
    {
        _graph = new CallGraph(new LiteralTermCountEqualizer());
    }
    

    [Test]
    public void ShouldSetHead()
    {
        var literal = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node = new CallGraphNode(literal);
        _graph?.AddNode(node);

        Assert.AreEqual(literal, _graph?.Root?.Literal);
    }

    [Test]
    public void ShouldNotSetHead()
    {
        var literal = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node = new CallGraphNode(literal);
        _graph?.AddNode(node);

        var literal2 = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node2 = new CallGraphNode(literal2);
        _graph?.AddNode(node2);

        Assert.AreNotEqual(literal2, _graph?.Root);
    }

    [Test]
    public void ShouldAddEdge()
    {
        var literal = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
        var node = new CallGraphNode(literal);
        _graph?.AddNode(node);

        var literal2 = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node2 = new CallGraphNode(literal2);
        _graph?.AddNode(node2);

        _graph?.AddEdge(new CallGraphEdge(node, node2, true, new Rule(literal, new BodyPart(literal2, null))));

        Assert.AreEqual(1, _graph?.Edges.Count);
    }


    [Test]
    public void ShouldThrowIfEdgeSourceIsNotInGraph()
    {
        var literal = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node = new CallGraphNode(literal);

        var literal2 = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node2 = new CallGraphNode(literal2);
        _graph?.AddNode(node2);

        Assert.Throws<ArgumentException>(() => _graph?.AddEdge(new CallGraphEdge(node, node2, true, new Rule(literal, new BodyPart(literal2, null)))));
    }

    [Test]
    public void ShouldThrowIfEdgeTargetIsNotInGraph()
    {
        var literal = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node = new CallGraphNode(literal);
        _graph?.AddNode(node);

        var literal2 = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node2 = new CallGraphNode(literal2);

        Assert.Throws<ArgumentException>(() => _graph?.AddEdge(new CallGraphEdge(node, node2, true, new Rule(literal, new BodyPart(literal2, null)))));
    }

    [Test]
    public void ShouldThrowIfNodeAlreadyExists()
    {
        var literal = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node = new CallGraphNode(literal);
        _graph?.AddNode(node);

        Assert.Throws<ArgumentException>(() => _graph?.AddNode(node));
    }

    [Test]
    public void ShouldThrowIfEdgeAlreadyExists()
    {
        var literal = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
        var node = new CallGraphNode(literal);
        _graph?.AddNode(node);

        var literal2 = new Literal(new Atom("reached", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
        var node2 = new CallGraphNode(literal2);
        _graph?.AddNode(node2);

        var edge = new CallGraphEdge(node, node2, true, new Rule(literal, new BodyPart(literal2, null)));
        _graph?.AddEdge(edge);

        Assert.Throws<ArgumentException>(() => _graph?.AddEdge(edge));
    }

    [Test]
    public void ShouldReturnTheRightNodes()
    {
        var literal = new Literal(new Atom("atom", new AtomParam[] { }), false, false);
        var literal2 = new Literal(new Atom("atom", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);

        var rule = new Rule(literal2, new BodyPart[] { new BodyPart(literal, null) }) ;
        var node2 = _graph?.AddNode(literal);
        var node = _graph?.AddNode(literal2);
        _graph?.AddEdge(new CallGraphEdge(node, node2, false, rule));

        var givenEdges = _graph.GetEdgesOfNode(node).ToArray();

        Assert.AreEqual(1, givenEdges.Length);
        Assert.IsTrue(givenEdges[0].Source.Literal.Equals(literal2));
        Assert.IsTrue(givenEdges[0].Target.Literal.Equals(literal));
    }
}
