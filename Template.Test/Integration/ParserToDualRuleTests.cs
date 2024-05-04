using Apollon.Lib.DualRules;
using Apollon.Lib.Rules.Operations;
using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Test.Integration
{
    [TestFixture]
    public class ParserToDualRuleTests
    {

        private ApollonParser parser = new ApollonParser();
        private IDualRuleGenerator generator = new DualRuleGenerator();

        [SetUp]
        public void Setup()
        {
            parser = new ApollonParser();
            generator = new DualRuleGenerator();
        }

        [Test]
        public void ShouldUnuninifyAtoms()
        {
            var code = "a(0).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(2, rules.Length);
            Assert.IsTrue(rules[0].Head.IsNAF);
            Assert.IsTrue(rules[1].Head.IsNAF);

            Assert.AreEqual("a0", rules[0].Head.Atom.Name);
            Assert.AreEqual("V/0", rules[0].Head.Atom.ParamList[0].Term.Value);
            Assert.AreEqual(1, rules[0].Body.Length);
            Assert.IsTrue(rules[0].Body[0].IsOperation);
            Assert.AreEqual(Operator.NotEquals, rules[0].Body[0].Operation.Operator);
            Assert.AreEqual("V/0", rules[0].Body[0].Operation.Variable.Value);
            Assert.AreEqual("0", rules[0].Body[0].Operation.Condition.Name);

            Assert.AreEqual("a", rules[1].Head.Atom.Name);
            Assert.AreEqual("V/0", rules[1].Head.Atom.ParamList[0].Term.Value);
            Assert.AreEqual(1, rules[1].Body.Length);
            Assert.AreEqual("a0", rules[1].Body[0].Literal.Atom.Name);
            Assert.AreEqual("V/0", rules[1].Body[0].Literal.Atom.ParamList[0].Term.Value);
        }

        [Test]
        public void SchouldCreateTwoDualRulesForThisRule()
        {
            var code = "p(X, b) :- q(X).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(3, rules.Length);
            Assert.AreEqual("not p0(X, V/0) :- V/0 != b().", rules[0].ToString());
            Assert.AreEqual("not p0(X, V/0) :- V/0 = b(), not q(X).", rules[1].ToString());
            Assert.AreEqual("not p(V/0, V/1) :- not p0(V/0, V/1).", rules[2].ToString());
        }

        [Test]
        public void ShouldIgnoreAtomsInProgramWhenCreatingDualRules()
        {
            var code = "cat.\r\n-dog.\r\ndog :- not cat.";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(2, rules.Length);
            Assert.AreEqual("not dog0() :- cat().", rules[0].ToString());
            Assert.AreEqual("not dog() :- not dog0().", rules[1].ToString());
        }

        [Test]
        public void ShouldIgnoreConstraintStatments()
        {
            var code = ":- b.";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(0, rules.Length);
        }

        [Test]
        public void ShouldHandleComplexerDualRules()
        {
            var code = "p :- not s\r\ns :- not r.\r\nr :- not p.\r\nr :- not s.";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(7, rules.Length);
            Assert.AreEqual("not p0() :- s().", rules[0].ToString());
            Assert.AreEqual("not p() :- not p0().", rules[1].ToString());
            Assert.AreEqual("not s0() :- r().", rules[2].ToString());
            Assert.AreEqual("not s() :- not s0().", rules[3].ToString());
            Assert.AreEqual("not r0() :- p().", rules[4].ToString());
            Assert.AreEqual("not r1() :- s().", rules[5].ToString());
            Assert.AreEqual("not r() :- not r0(), not r1().", rules[6].ToString());
        }

        [Test]
        public void ShouldCreateMulipleRulesForStatmentsWithMultipleBodyParts()
        {
            var code = "happy(X) :- likes(X, prix), not hates(X, stefan).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(3, rules.Length);
            Assert.AreEqual("not happy0(X) :- not likes(X, prix).", rules[0].ToString());
            Assert.AreEqual("not happy0(X) :- likes(X, prix), hates(X, stefan).", rules[1].ToString());
            Assert.AreEqual("not happy(V/0) :- not happy0(V/0).", rules[2].ToString());
        }

        [Test]
        public void ShouldCreateForAllRule()
        {
            var code = "a(X) :- b(X, Y), c(Y, X).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(4, rules.Length);
            Assert.AreEqual("not a0(X, Y) :- not b(X, Y).", rules[0].ToString());
            Assert.AreEqual("not a0(X, Y) :- b(X, Y), not c(Y, X).", rules[1].ToString());
            Assert.AreEqual("not a0(X) :- forall(Y, not a0(X, Y)).", rules[2].ToString());
            Assert.AreEqual("not a(V/0) :- not a0(V/0).", rules[3].ToString());
        }

    }
}
