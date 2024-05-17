﻿using Apollon.Lib.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;
using Apollon.Lib.Graph;
using Apollon.Lib.Atoms;
using Apollon.Lib.Docu;

namespace Template.Test;

public class CallGraphBuilderTests
{
    private CallGraphBuilder? _callGraphBuilder;

    [SetUp]
    public void Setup()
    {
        _callGraphBuilder = new CallGraphBuilder(new LiteralParamCountEqualizer());
    }

    [Test]
    public void ShouldBuildCallGraph()
    {
        var literal = new Literal(new Atom("human", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
        var literal2 = new Literal(new Atom("informatiker", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);

        var program = new Program(new Literal[] { literal, literal2 }, new Rule[] { new Rule(literal, new BodyPart(literal2, null)) }, new Constraint[0], new Documentation[0]);
        var graph = _callGraphBuilder?.BuildCallGraph(program);

        //Assert.AreEqual(new List<CallGraphNode> { new CallGraphNode(literal), new CallGraphNode(literal2)}, graph.Nodes);
        //Assert.AreEqual(new List<CallGraphEdge> { new CallGraphEdge(new CallGraphNode(literal), new CallGraphNode(literal2), 1, new Rule(literal, literal2)) }, graph.Edges);
        Assert.AreEqual(graph?.Nodes.Count, 2);
        Assert.AreEqual(graph?.Edges.Count, 1);
    }

    [Test]
    public void ShouldBuildCallGraphCorrectly()
    {
        var literals = new Literal[] { new Literal(new Atom("atom", new AtomParam[] { }), false, false) };
        var rules = new Rule[] { new Rule(new Literal(new Atom("atom", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false), new BodyPart[] { new BodyPart(new Literal(new Atom("atom", new AtomParam[] { }), false, false), null) }) };
        var prgram = new Program(literals, rules, new Constraint[0], new Documentation[0]);

        var graph = _callGraphBuilder?.BuildCallGraph(prgram);

        Assert.AreEqual(graph?.Nodes.Count, 2);
        Assert.AreEqual(graph?.Edges.Count, 1);

        var testLiteral = new Literal(new Atom("atom", new AtomParam[] { }), false, false);
        var testLiteral2 = new Literal(new Atom("atom", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
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
        var literals = new Literal[] { new Literal(new Atom("atom", new AtomParam[] { }), false, false) };
        var rules = new Rule[] { new Rule(new Literal(new Atom("atom", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false), new BodyPart[] { new BodyPart(new Literal(new Atom("atom", new AtomParam[] { }), true, false), null) }) };
        var prgram = new Program(literals, rules, new Constraint[0], new Documentation[0]);

        var graph = _callGraphBuilder?.BuildCallGraph(prgram);

        Assert.AreEqual(graph?.Nodes.Count, 2);
        Assert.AreEqual(graph?.Edges.Count, 1);

        var testLiteral = new Literal(new Atom("atom", new AtomParam[] { }), false, false);
        var testLiteral2 = new Literal(new Atom("atom", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
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
