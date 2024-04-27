using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.Graph;
using Apollon.Lib;
using Apollon.Lib.OLON;
using Apollon.Lib.Rules;
using Apollon.Lib.Atoms;

namespace Template.Test.OLON
{
    [TestFixture]
    public class OlonDetectorTests
    {
        [Test]
        public void ShouldDetectAllNodesAsPartOfOlon()
        {
            var literal1 = new Literal(new Atom("atom1", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
            var literal2Naf = new Literal(new Atom("atom2", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
            var literal2 = new Literal(new Atom("atom2", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
            var rule1 = new Rule(literal1, new Literal[] { literal2Naf });
            var rule2 = new Rule(literal2, new Literal[] { literal1 });
            var rules = new Rule[] { rule1, rule2 };
            var prgram = new Program(new Literal[] {}, rules, new Constraint[0]);

            var graph = new CallGraphBuilder(new LiteralTermCountEqualizer()).BuildCallGraph(prgram);

            var olonSet = OlonDetector.DetectOlonIn(graph);

            Assert.AreEqual(olonSet.Nodes.Count, 2);
        }

        [Test]
        public void SouldNotDetectAnyNodesAsPartOfOlon()
        {
            var literal1 = new Literal(new Atom("atom1", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
            var literal2 = new Literal(new Atom("atom2", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
            var rule1 = new Rule(literal1, new Literal[] { literal2 });
            var rule2 = new Rule(literal2, new Literal[] { literal1 });
            var rules = new Rule[] { rule1, rule2 };
            var prgram = new Program(new Literal[] { }, rules, new Constraint[0]);

            var graph = new CallGraphBuilder(new LiteralTermCountEqualizer()).BuildCallGraph(prgram);

            var olonSet = OlonDetector.DetectOlonIn(graph);

            Assert.AreEqual(olonSet.Nodes.Count, 0);
        }

        [Test]
        public void ShouldDetectAllNodesAsPartOfOlonAndNotDetectNodesOutsideOlon()
        {
            var literal1 = new Literal(new Atom("atom1", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
            var literal2Naf = new Literal(new Atom("atom2", new AtomParam[] { new AtomParam(null, new Term("V")) }), true, false);
            var literal2 = new Literal(new Atom("atom2", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
            var literal3 = new Literal(new Atom("atom3", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
            var rule1 = new Rule(literal1, new Literal[] { literal2Naf });
            var rule2 = new Rule(literal2, new Literal[] { literal1, literal3 });
            var rules = new Rule[] { rule1, rule2 };
            var prgram = new Program(new Literal[] { }, rules, new Constraint[0]);

            var graph = new CallGraphBuilder(new LiteralTermCountEqualizer()).BuildCallGraph(prgram);

            var olonSet = OlonDetector.DetectOlonIn(graph);

            Assert.AreEqual(olonSet.Nodes.Count, 2);
            var node = graph.Nodes.Where(node => node.Literal.Equals(literal3)).First();
            Assert.IsFalse(olonSet.IsPartOfOlon(node));
        }
    }
}
