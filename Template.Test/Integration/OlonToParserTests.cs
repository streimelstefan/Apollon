using Antlr4.Runtime;
using Apollon.Lib.CallGraph;
using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib.Graph;
using Template.Lib.OLON;
using Template.Lib.Rules;

namespace Apollon.Test.Integration
{
    [TestFixture]
    public class OlonToParserTests
    {
        private ApollonParser _parser = new ApollonParser();

        [Test]
        public void ShouldParseAtomWithConstraintRuleAndNormalRule()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/OLONRuleByConstraintRule.apo");

            var callGraph = new CallGraphBuilder(new LiteralTermCountEqualizer()).BuildCallGraph(program);

            var olonSet = OlonDetector.DetectOlonIn(callGraph);

            Assert.IsNotNull(olonSet);
            Assert.AreEqual(1, olonSet.Nodes.Count);

            var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleList);

            Assert.IsNotNull(processedRules);
            Assert.AreEqual(2, processedRules.Length);
            Assert.IsTrue(processedRules[0].IsOlonRule);
            Assert.IsFalse(processedRules[0].IsOrdiniaryRule);


            Assert.IsTrue(processedRules[1].IsOlonRule);
            Assert.IsFalse(processedRules[1].IsOrdiniaryRule);
        }

        [Test]
        public void ShouldDetectOlonNodeButNoOlonRules()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/ConstraintWithoutOLONRules.apo");

            var callGraph = new CallGraphBuilder(new LiteralTermCountEqualizer()).BuildCallGraph(program);

            var olonSet = OlonDetector.DetectOlonIn(callGraph);

            Assert.IsNotNull(olonSet);
            Assert.AreEqual(1, olonSet.Nodes.Count);

            var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleList);

            Assert.IsNotNull(processedRules);
            Assert.AreEqual(2, processedRules.Length);
            Assert.IsFalse(processedRules[0].IsOlonRule);
            Assert.IsTrue(processedRules[0].IsOrdiniaryRule);

            Assert.IsTrue(processedRules[1].IsOlonRule);
            Assert.IsFalse(processedRules[1].IsOrdiniaryRule);
        }


        [Test]
        public void ShouldDetectThreeOLONSandTwoOrdinaryRules()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/ComplexOLONTest.apo");

            var callGraph = new CallGraphBuilder(new LiteralTermCountEqualizer()).BuildCallGraph(program);

            var olonSet = OlonDetector.DetectOlonIn(callGraph);

            Assert.IsNotNull(olonSet);
            Assert.AreEqual(3, olonSet.Nodes.Count);

            var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleList);

            Assert.IsNotNull(processedRules);

            Assert.AreEqual(4, processedRules.Length);
        }
    }
}
