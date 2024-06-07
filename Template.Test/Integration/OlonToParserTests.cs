namespace Apollon.Test.Integration
{
    using Apollon.Lib.Graph;
    using Apollon.Lib.OLON;
    using Apollon.Lib.Rules;
    using AppollonParser;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class OlonToParserTests
    {
        private readonly ApollonParser parser = new();

        [Test]
        public void ShouldParseAtomWithConstraintRuleAndNormalRule()
        {
            Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/OLONRuleByConstraintRule.apo");

            CallGraph callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

            OlonSet olonSet = OlonDetector.DetectOlonIn(callGraph);

            Assert.IsNotNull(olonSet);
            Assert.AreEqual(0, olonSet.Nodes.Count);

            PreprocessedStatement[] processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleTypesAsStatements.ToArray());

            Assert.IsNotNull(processedRules);
            Assert.AreEqual(2, processedRules.Length);
            Assert.IsFalse(processedRules[0].IsOlonRule);
            Assert.IsTrue(processedRules[0].IsOrdiniaryRule);

            Assert.IsTrue(processedRules[1].IsOlonRule);
            Assert.IsTrue(processedRules[1].IsOrdiniaryRule);
        }

        [Test]
        public void ShouldDetectOlonNodeButNoOlonRules()
        {
            Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/ConstraintWithoutOLONRules.apo");

            CallGraph callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

            OlonSet olonSet = OlonDetector.DetectOlonIn(callGraph);

            Assert.IsNotNull(olonSet);
            Assert.AreEqual(0, olonSet.Nodes.Count);

            PreprocessedStatement[] processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleTypesAsStatements.ToArray());

            Assert.IsNotNull(processedRules);
            Assert.AreEqual(2, processedRules.Length);
            Assert.IsFalse(processedRules[0].IsOlonRule);
            Assert.IsTrue(processedRules[0].IsOrdiniaryRule);

            Assert.IsTrue(processedRules[1].IsOlonRule);
            Assert.IsTrue(processedRules[1].IsOrdiniaryRule);
        }

        [Test]
        public void ShouldDetectThreeOLONSandTwoOrdinaryRules()
        {
            Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/ComplexOLONTest.apo");

            CallGraph callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

            OlonSet olonSet = OlonDetector.DetectOlonIn(callGraph);

            Assert.IsNotNull(olonSet);
            Assert.AreEqual(3, olonSet.Nodes.Count);

            PreprocessedStatement[] processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleList);

            Assert.IsNotNull(processedRules);

            Assert.AreEqual(4, processedRules.Length);
        }

        [Test]
        public void ShouldDetectNoOLONRules()
        {
            string code = "faster(bunny, turtle).\r\nfaster(cat, bunny).\r\n\r\nis_faster(X, Y) :- faster(X, Y).\r\nis_faster(X, Y) :- faster(X, Z), is_faster(Z, Y).\r\n\r\nfastest(X) :- not is_faster(Y, X).";
            Lib.Program program = this.parser.ParseFromString(code);
            CallGraph callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

            OlonSet olonSet = OlonDetector.DetectOlonIn(callGraph);

            Assert.IsNotNull(olonSet);
            Assert.AreEqual(0, olonSet.Nodes.Count);
        }

        [Test]
        public void ShouldDetectFourRulesAsOlonRules()
        {
            var parser = new ApollonParser();
            var code = "reachable(V) :- chosen(U, V), reachable(U).\r\nreachable(0) :- chosen(V, 0).\r\nother(U, V) :- vertex(U), vertex(V), vertex(W), edge(U, W), V != W, chosen(U, W).\r\nchosen(U, V) :- edge(U, V), not other(U, V).\r\n :- vertex(U), not reachable(U).\r\n :- chosen(U, W), chosen(V, W), U != V.";
            var program = parser.ParseFromString(code);

            CallGraph callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

            var olonSet = OlonDetector.DetectOlonIn(callGraph);

            PreprocessedStatement[] processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleList);

            Assert.IsNotNull(processedRules);

            Assert.AreEqual(4, processedRules.Length);

        }
    }
}
