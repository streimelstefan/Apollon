namespace Template.Test;
using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Graph;
using Apollon.Lib.Rules;
using NUnit.Framework;
using System;
using System.Linq;

[TestFixture]
public class CallGraphTests
{
    private CallGraph? graph;

    [SetUp]
    public void Setup()
    {
        this.graph = new CallGraph(new LiteralParamCountEqualizer());
    }

    [Test]
    public void ShouldSetHead()
    {
        Literal literal = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node = new(literal);
        _ = this.graph?.AddNode(node);

        Assert.AreEqual(literal, this.graph?.Root?.Literal);
    }

    [Test]
    public void ShouldNotSetHead()
    {
        Literal literal = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node = new(literal);
        _ = this.graph?.AddNode(node);

        Literal literal2 = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node2 = new(literal2);
        _ = this.graph?.AddNode(node2);

        Assert.AreNotEqual(literal2, this.graph?.Root);
    }

    [Test]
    public void ShouldAddEdge()
    {
        Literal literal = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), false, false);
        CallGraphNode node = new(literal);
        _ = this.graph?.AddNode(node);

        Literal literal2 = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node2 = new(literal2);
        _ = this.graph?.AddNode(node2);

        _ = this.graph?.AddEdge(new CallGraphEdge(node, node2, true, new Rule(literal, new BodyPart(literal2, null))));

        Assert.AreEqual(1, this.graph?.Edges.Count);
    }

    [Test]
    public void ShouldThrowIfEdgeSourceIsNotInGraph()
    {
        Literal literal = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node = new(literal);

        Literal literal2 = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node2 = new(literal2);
        _ = this.graph?.AddNode(node2);

        _ = Assert.Throws<ArgumentException>(() => this.graph?.AddEdge(new CallGraphEdge(node, node2, true, new Rule(literal, new BodyPart(literal2, null)))));
    }

    [Test]
    public void ShouldThrowIfEdgeTargetIsNotInGraph()
    {
        Literal literal = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node = new(literal);
        _ = this.graph?.AddNode(node);

        Literal literal2 = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node2 = new(literal2);

        _ = Assert.Throws<ArgumentException>(() => this.graph?.AddEdge(new CallGraphEdge(node, node2, true, new Rule(literal, new BodyPart(literal2, null)))));
    }

    [Test]
    public void ShouldThrowIfNodeAlreadyExists()
    {
        Literal literal = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node = new(literal);
        _ = this.graph?.AddNode(node);

        _ = Assert.Throws<ArgumentException>(() => this.graph?.AddNode(node));
    }

    [Test]
    public void ShouldThrowIfEdgeAlreadyExists()
    {
        Literal literal = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), false, false);
        CallGraphNode node = new(literal);
        _ = this.graph?.AddNode(node);

        Literal literal2 = new(new Atom("reached", new AtomParam[] { new(null, new Term("V")) }), true, false);
        CallGraphNode node2 = new(literal2);
        _ = this.graph?.AddNode(node2);

        CallGraphEdge edge = new(node, node2, true, new Rule(literal, new BodyPart(literal2, null)));
        _ = this.graph?.AddEdge(edge);

        _ = Assert.Throws<ArgumentException>(() => this.graph?.AddEdge(edge));
    }

    [Test]
    public void ShouldReturnTheRightNodes()
    {
        Literal literal = new(new Atom("atom", new AtomParam[] { }), false, false);
        Literal literal2 = new(new Atom("atom", new AtomParam[] { new(null, new Term("V")) }), false, false);

        Rule rule = new(literal2, new BodyPart[] { new(literal, null) });
        CallGraphNode? node2 = this.graph?.AddNode(literal);
        CallGraphNode? node = this.graph?.AddNode(literal2);
        _ = this.graph?.AddEdge(new CallGraphEdge(node, node2, false, rule));

        CallGraphEdge[] givenEdges = this.graph.GetEdgesOfNode(node).ToArray();

        Assert.AreEqual(1, givenEdges.Length);
        Assert.IsTrue(givenEdges[0].Source.Literal.Equals(literal2));
        Assert.IsTrue(givenEdges[0].Target.Literal.Equals(literal));
    }
}
