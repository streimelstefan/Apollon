namespace Template.Test.OLON
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Docu;
    using Apollon.Lib.Graph;
    using Apollon.Lib.OLON;
    using Apollon.Lib.Rules;
    using NUnit.Framework;

    [TestFixture]
    public class OlonDetectorTests
    {
        [Test]
        public void ShouldDetectAllNodesAsPartOfOlon()
        {
            Literal literal1 = new(new Atom("atom1", new AtomParam[] { new(null, new Term("V")) }), false, false);
            Literal literal2Naf = new(new Atom("atom2", new AtomParam[] { new(null, new Term("V")) }), true, false);
            Literal literal2 = new(new Atom("atom2", new AtomParam[] { new(null, new Term("V")) }), false, false);
            Rule rule1 = new(literal1, new BodyPart[] { new(literal2Naf, null) });
            Rule rule2 = new(literal2, new BodyPart[] { new(literal1, null) });
            Rule[] rules = new Rule[] { rule1, rule2 };
            Program prgram = new(new Literal[] { }, rules, new Constraint[0], new Documentation[0]);

            CallGraph graph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(prgram);

            OlonSet olonSet = OlonDetector.DetectOlonIn(graph);

            Assert.AreEqual(olonSet.Nodes.Count, 2);
        }

        [Test]
        public void SouldNotDetectAnyNodesAsPartOfOlon()
        {
            Literal literal1 = new(new Atom("atom1", new AtomParam[] { new(null, new Term("V")) }), false, false);
            Literal literal2 = new(new Atom("atom2", new AtomParam[] { new(null, new Term("V")) }), false, false);
            Rule rule1 = new(literal1, new BodyPart[] { new(literal2, null) });
            Rule rule2 = new(literal2, new BodyPart[] { new(literal1, null) });
            Rule[] rules = new Rule[] { rule1, rule2 };
            Program prgram = new(new Literal[] { }, rules, new Constraint[0], new Documentation[0]);

            CallGraph graph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(prgram);

            OlonSet olonSet = OlonDetector.DetectOlonIn(graph);

            Assert.AreEqual(olonSet.Nodes.Count, 0);
        }

        [Test]
        public void ShouldDetectAllNodesAsPartOfOlonAndNotDetectNodesOutsideOlon()
        {
            Literal literal1 = new(new Atom("atom1", new AtomParam[] { new(null, new Term("V")) }), false, false);
            Literal literal2Naf = new(new Atom("atom2", new AtomParam[] { new(null, new Term("V")) }), true, false);
            Literal literal2 = new(new Atom("atom2", new AtomParam[] { new(null, new Term("V")) }), false, false);
            Literal literal3 = new(new Atom("atom3", new AtomParam[] { new(null, new Term("V")) }), false, false);
            Rule rule1 = new(literal1, new BodyPart[] { new(literal2Naf, null) });
            Rule rule2 = new(literal2, new BodyPart[] { new(literal1, null) });
            Rule[] rules = new Rule[] { rule1, rule2 };
            Program prgram = new(new Literal[] { }, rules, new Constraint[0], new Documentation[0]);

            CallGraph graph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(prgram);

            OlonSet olonSet = OlonDetector.DetectOlonIn(graph);

            Assert.AreEqual(olonSet.Nodes.Count, 2);
            CallGraphNode? node = graph.GetNode(literal3);
            Assert.IsFalse(olonSet.IsPartOfOlon(node));
        }
    }
}
