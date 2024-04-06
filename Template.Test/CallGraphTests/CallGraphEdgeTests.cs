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
public class CallGraphEdgeTests
{
    [Test]
    public void ShouldThrowIfSourceIsNull()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), false, false);

        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node2 = new CallGraphNode(literal2);

        Assert.Throws<ArgumentNullException>(() => new CallGraphEdge(null, node2, 1, new Rule(literal, literal2)));
    }

    [Test]
    public void ShouldThrowIfTargetIsNull()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), false, false);
        var node = new CallGraphNode(literal);

        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);

        Assert.Throws<ArgumentNullException>(() => new CallGraphEdge(node, null, 1, new Rule(literal, literal2)));
    }

    [Test]
    public void ShouldThrowIfRuleIsNull()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), false, false);
        var node = new CallGraphNode(literal);

        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node2 = new CallGraphNode(literal2);

        Assert.Throws<ArgumentNullException>(() => new CallGraphEdge(node, node2, 1, null));
    }

    [Test]
    public void ShouldThrowIfWeightIsInvalid()
    {
        var literal = new Literal(new Atom("reached", new Term[] { new Term("V") }), false, false);
        var node = new CallGraphNode(literal);

        var literal2 = new Literal(new Atom("reached", new Term[] { new Term("V") }), true, false);
        var node2 = new CallGraphNode(literal2);

        Assert.Throws<ArgumentOutOfRangeException>(() => new CallGraphEdge(node, node2, -1, new Rule(literal, literal2)));
    }
}
