using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Test.Parser
{
    [TestFixture]
    public class ParserTests
    {

        private ApollonParser _parser = new ApollonParser();

        [SetUp]
        public void SetUp()
        {
            _parser = new ApollonParser();
        }

        [Test]
        public void SouldParseAndReturnAProgramThatIsNotNull()
        {
            var input = "";

            var program = _parser.ParseFromString(input);

            Assert.IsNotNull(program);
        }

        [Test]
        public void ShouldParseTwoFacts()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/BasicFacts.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(2, program.LiteralList.Length);
            Assert.IsFalse(program.LiteralList[0].IsNAF);
            Assert.IsFalse(program.LiteralList[0].IsNegative);
            Assert.AreEqual("cat", program.LiteralList[0].Atom.Name);
            Assert.AreEqual(0, program.LiteralList[0].Atom.TermList.Length);

            Assert.IsFalse(program.LiteralList[1].IsNAF);
            Assert.IsFalse(program.LiteralList[1].IsNegative);
            Assert.AreEqual("dog", program.LiteralList[1].Atom.Name);
            Assert.AreEqual(0, program.LiteralList[1].Atom.TermList.Length);
        }

        [Test]
        public void ShouldParseABasicRule()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/BasicRule.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.LiteralList.Length);
            Assert.AreEqual(1, program.RuleList.Length);

            Assert.AreEqual("bird", program.RuleList[0].Head.Atom.Name);
            Assert.AreEqual("cat", program.RuleList[0].Body[0].Atom.Name);
            Assert.AreEqual("dog", program.RuleList[0].Body[1].Atom.Name);
        }

        [Test]
        public void ShouldParseWithComments()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/CommentsFactsAndRules.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.LiteralList.Length);
            Assert.AreEqual(1, program.RuleList.Length);

            Assert.AreEqual("bird", program.RuleList[0].Head.Atom.Name);
            Assert.AreEqual("worm", program.RuleList[0].Body[0].Atom.Name);

            Assert.AreEqual("fish", program.LiteralList[0].Atom.Name);
        }

        [Test]
        public void ShouldParseNAFAndNegationCorrectly()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/NafAndNegation.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(2, program.LiteralList.Length);
            Assert.AreEqual(1, program.RuleList.Length);

            Assert.AreEqual("dog", program.RuleList[0].Head.Atom.Name);
            Assert.AreEqual("cat", program.RuleList[0].Body[0].Atom.Name);
            Assert.IsFalse(program.RuleList[0].Body[0].IsNegative);
            Assert.IsTrue(program.RuleList[0].Body[0].IsNAF);

            Assert.AreEqual("cat", program.LiteralList[0].Atom.Name);
            Assert.IsFalse(program.LiteralList[0].IsNegative);
            Assert.IsFalse(program.LiteralList[0].IsNAF);
            Assert.AreEqual("dog", program.LiteralList[1].Atom.Name);
            Assert.IsTrue(program.LiteralList[1].IsNegative);
            Assert.IsFalse(program.LiteralList[1].IsNAF);
        }

        [Test]
        public void ShouldParseRulesWithVariables()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/RulesWithVariablesAndTerms.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.LiteralList.Length);
            Assert.AreEqual(1, program.RuleList.Length);

            Assert.AreEqual("happy", program.RuleList[0].Head.Atom.Name);
            Assert.AreEqual(1, program.RuleList[0].Head.Atom.TermList.Length);
            Assert.AreEqual("X", program.RuleList[0].Head.Atom.TermList[0].Value);
            Assert.IsTrue(program.RuleList[0].Head.Atom.TermList[0].IsVariable);

            Assert.AreEqual("likes", program.RuleList[0].Body[0].Atom.Name);
            Assert.AreEqual(2, program.RuleList[0].Head.Atom.TermList.Length);
            Assert.AreEqual("X", program.RuleList[0].Head.Atom.TermList[0].Value);
            Assert.IsTrue(program.RuleList[0].Head.Atom.TermList[0].IsVariable);
            Assert.AreEqual("prix", program.RuleList[0].Head.Atom.TermList[1].Value);
            Assert.IsFalse(program.RuleList[0].Head.Atom.TermList[1].IsVariable);


            Assert.AreEqual("hates", program.RuleList[1].Body[0].Atom.Name);
            Assert.AreEqual(2, program.RuleList[1].Head.Atom.TermList.Length);
            Assert.AreEqual("X", program.RuleList[1].Head.Atom.TermList[0].Value);
            Assert.IsTrue(program.RuleList[1].Head.Atom.TermList[0].IsVariable);
            Assert.AreEqual("prix", program.RuleList[1].Head.Atom.TermList[1].Value);
            Assert.IsFalse(program.RuleList[1].Head.Atom.TermList[1].IsVariable);
        }

        [Test]
        public void ShouldParseAtomWithTerms()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/AtomWithTerms.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.LiteralList.Length);

            Assert.AreEqual("likes", program.LiteralList[0].Atom.Name);
            Assert.AreEqual(2, program.LiteralList[0].Atom.TermList.Length);
            Assert.AreEqual("X", program.LiteralList[0].Atom.TermList[0].Value);
            Assert.IsTrue(program.LiteralList[0].Atom.TermList[0].IsVariable);
            Assert.AreEqual("prix", program.LiteralList[0].Atom.TermList[1].Value);
        }
    }
}
