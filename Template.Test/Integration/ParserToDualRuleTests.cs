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

            Assert.AreEqual("_a0", rules[0].Head.Atom.Name);
            Assert.AreEqual("V/0", rules[0].Head.Atom.ParamList[0].Term.Value);
            Assert.AreEqual(1, rules[0].Body.Length);
            Assert.IsTrue(rules[0].Body[0].IsOperation);
            Assert.AreEqual(Operator.NotEquals, rules[0].Body[0].Operation.Operator);
            Assert.AreEqual("V/0", rules[0].Body[0].Operation.Variable.Term.Value);
            Assert.AreEqual("0", rules[0].Body[0].Operation.Condition.Literal.Atom.Name);

            Assert.AreEqual("a", rules[1].Head.Atom.Name);
            Assert.AreEqual("V/0", rules[1].Head.Atom.ParamList[0].Term.Value);
            Assert.AreEqual(1, rules[1].Body.Length);
            Assert.AreEqual("_a0", rules[1].Body[0].Literal.Atom.Name);
            Assert.AreEqual("V/0", rules[1].Body[0].Literal.Atom.ParamList[0].Term.Value);
        }

        [Test]
        public void SchouldCreateTwoDualRulesForThisRule()
        {
            var code = "p(X, b) :- q(X).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(3, rules.Length);
            Assert.AreEqual("not _p0(X, V/0) :- V/0 != b().", rules[0].ToString());
            Assert.AreEqual("not _p0(X, V/0) :- V/0 = b(), not q(X).", rules[1].ToString());
            Assert.AreEqual("not p(V/0, V/1) :- not _p0(V/0, V/1).", rules[2].ToString());
        }

        [Test]
        public void ShouldIgnoreAtomsInProgramWhenCreatingDualRules()
        {
            var code = "cat.\r\n-dog.\r\ndog :- not cat.";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(2, rules.Length);
            Assert.AreEqual("not _dog0() :- cat().", rules[0].ToString());
            Assert.AreEqual("not dog() :- not _dog0().", rules[1].ToString());
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
            Assert.AreEqual("not _p0() :- s().", rules[0].ToString());
            Assert.AreEqual("not p() :- not _p0().", rules[1].ToString());
            Assert.AreEqual("not _s0() :- r().", rules[2].ToString());
            Assert.AreEqual("not s() :- not _s0().", rules[3].ToString());
            Assert.AreEqual("not _r0() :- p().", rules[4].ToString());
            Assert.AreEqual("not _r1() :- s().", rules[5].ToString());
            Assert.AreEqual("not r() :- not _r0(), not _r1().", rules[6].ToString());
        }

        [Test]
        public void ShouldCreateMulipleRulesForStatmentsWithMultipleBodyParts()
        {
            var code = "happy(X) :- likes(X, prix), not hates(X, stefan).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(3, rules.Length);
            Assert.AreEqual("not _happy0(X) :- not likes(X, prix).", rules[0].ToString());
            Assert.AreEqual("not _happy0(X) :- likes(X, prix), hates(X, stefan).", rules[1].ToString());
            Assert.AreEqual("not happy(V/0) :- not _happy0(V/0).", rules[2].ToString());
        }

        [Test]
        public void ShouldCreateForAllRule()
        {
            var code = "a(X) :- b(X, Y), c(Y, X).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(4, rules.Length);
            Assert.AreEqual("not _a0(X, Y) :- not b(X, Y).", rules[0].ToString());
            Assert.AreEqual("not _a0(X, Y) :- b(X, Y), not c(Y, X).", rules[1].ToString());
            Assert.AreEqual("not _a0(X) :- forall(Y, not _a0(X, Y)).", rules[2].ToString());
            Assert.AreEqual("not a(V/0) :- not _a0(V/0).", rules[3].ToString());
        }

        [Test]
        public void ShouldCreateForAllForSinglePrivateVariables()
        {
            var code = "a(X) :- b(X, Y).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(3, rules.Length);
            Assert.AreEqual("not _a0(X, Y) :- not b(X, Y).", rules[0].ToString());
            Assert.AreEqual("not _a0(X) :- forall(Y, not _a0(X, Y)).", rules[1].ToString());
            Assert.AreEqual("not a(V/0) :- not _a0(V/0).", rules[2].ToString());
        }

        [Test]
        public void ShouldParseComplexerProgramWithOnlyAtoms()
        {
            var code = "bird(tweety).\r\ncat(sylvester).\r\ndog(pluto).\r\nlikes(mary, pizza).\r\nlikes(john, pasta).\r\n";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(11, rules.Length);
            Assert.AreEqual("not _bird0(V/0) :- V/0 != tweety().", rules[0].ToString());
            Assert.AreEqual("not bird(V/0) :- not _bird0(V/0).", rules[1].ToString());
            Assert.AreEqual("not _cat0(V/0) :- V/0 != sylvester().", rules[2].ToString());
            Assert.AreEqual("not cat(V/0) :- not _cat0(V/0).", rules[3].ToString());
            Assert.AreEqual("not _dog0(V/0) :- V/0 != pluto().", rules[4].ToString());
            Assert.AreEqual("not dog(V/0) :- not _dog0(V/0).", rules[5].ToString());
            Assert.AreEqual("not _likes0(V/0, V/1) :- V/0 != mary().", rules[6].ToString());
            Assert.AreEqual("not _likes0(V/0, V/1) :- V/0 = mary(), V/1 != pizza().", rules[7].ToString());
            Assert.AreEqual("not _likes1(V/0, V/1) :- V/0 != john().", rules[8].ToString());
            Assert.AreEqual("not _likes1(V/0, V/1) :- V/0 = john(), V/1 != pasta().", rules[9].ToString());
            Assert.AreEqual("not likes(V/0, V/1) :- not _likes0(V/0, V/1), not _likes1(V/0, V/1).", rules[10].ToString());
        }

        [Test]
        public void ShouldCreateDualRulesComplexerProgramWithRecursion()
        {
            var code = "parent(alice, bob).\r\nparent(bob, charlie).\r\nancestor(X, Y) :- parent(X, Y).\r\nancestor(X, Y) :- parent(X, Z), ancestor(Z, Y).\r\n";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(10, rules.Length);
            Assert.AreEqual("not _parent0(V/0, V/1) :- V/0 != alice().", rules[0].ToString());
            Assert.AreEqual("not _parent0(V/0, V/1) :- V/0 = alice(), V/1 != bob().", rules[1].ToString());
            Assert.AreEqual("not _parent1(V/0, V/1) :- V/0 != bob().", rules[2].ToString());
            Assert.AreEqual("not _parent1(V/0, V/1) :- V/0 = bob(), V/1 != charlie().", rules[3].ToString());
            Assert.AreEqual("not parent(V/0, V/1) :- not _parent0(V/0, V/1), not _parent1(V/0, V/1).", rules[4].ToString());
            Assert.AreEqual("not _ancestor0(X, Y) :- not parent(X, Y).", rules[5].ToString());
            Assert.AreEqual("not _ancestor1(X, Y, Z) :- not parent(X, Z).", rules[6].ToString());
            Assert.AreEqual("not _ancestor1(X, Y, Z) :- parent(X, Z), not ancestor(Z, Y).", rules[7].ToString());
            Assert.AreEqual("not _ancestor1(X, Y) :- forall(Z, not _ancestor1(X, Y, Z)).", rules[8].ToString());
            Assert.AreEqual("not ancestor(V/0, V/1) :- not _ancestor0(V/0, V/1), not _ancestor1(V/0, V/1).", rules[9].ToString());
        }

        [Test]
        public void ShouldConvertLessThenToGreaterThenOrEquals()
        {
            var code = "a(X) :- X < 2.";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(2, rules.Length);
            Assert.AreEqual("not _a0(X) :- X >= 2.", rules[0].ToString());
            Assert.AreEqual("not a(V/0) :- not _a0(V/0).", rules[1].ToString());
        }

        [Test]
        public void ShouldConvertGreaterThenToLessThenOrEquals()
        {
            var code = "a(X) :- X > 2.";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(2, rules.Length);
            Assert.AreEqual("not _a0(X) :- X <= 2.", rules[0].ToString());
            Assert.AreEqual("not a(V/0) :- not _a0(V/0).", rules[1].ToString());
        }

        [Test]
        public void ShouldConvertGreaterThenOrEqualToLessThen()
        {
            var code = "a(X) :- X >= 2.";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(2, rules.Length);
            Assert.AreEqual("not _a0(X) :- X < 2.", rules[0].ToString());
            Assert.AreEqual("not a(V/0) :- not _a0(V/0).", rules[1].ToString());
        }

        [Test]
        public void ShouldConvertLessThanOrEqualToLessThen()
        {
            var code = "a(X) :- X <= 2.";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(2, rules.Length);
            Assert.AreEqual("not _a0(X) :- X > 2.", rules[0].ToString());
            Assert.AreEqual("not a(V/0) :- not _a0(V/0).", rules[1].ToString());
        }

        [Test]
        public void ShouldNAFSwithIsOperations()
        {
            var code = "a(X) :- Y is X + 2.";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());

            Assert.AreEqual(3, rules.Length);
            Assert.AreEqual("not _a0(X, Y) :- not Y is X + 2.", rules[0].ToString());
            Assert.AreEqual("not _a0(X) :- forall(Y, not _a0(X, Y)).", rules[1].ToString());
            Assert.AreEqual("not a(V/0) :- not _a0(V/0).", rules[2].ToString());
        }

    }
}
