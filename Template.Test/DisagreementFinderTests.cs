using Apollon.Lib.Unification;
using Apollon.Lib.Unification.DisagreementFinders;
using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Test
{
    [TestFixture]
    public class DisagreementFinderTests
    {
        private IDisagreementFinder _finder = new DisagreementFinder();
        private ApollonParser _parser = new ApollonParser();

        [SetUp]
        public void Setup()
        {
            _finder = new DisagreementFinder();
            _parser = new ApollonParser();
        }

        [Test]
        public void ShouldFindThatRuleNameIsDifferentAndReturnError()
        {
            var code = "head1(X).\nhead2(X).";
            var program = _parser.ParseFromString(code);
            var rule1 = program.Statements.First();
            var rule2 = program.Statements.Last();

            var res = _finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsError);
        }

        [Test]
        public void ShouldFindDisagreementInHeadParamOneAndReturnTheVariableAndTerm()
        {
            var code = "head(X).\nhead(test).";
            var program = _parser.ParseFromString(code);
            var rule1 = program.Statements.First();
            var rule2 = program.Statements.Last();

            var res = _finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsSuccess);
            Assert.IsNotNull(res.Value);
            Assert.AreEqual("X", res.Value.First.Term.Value);
            Assert.AreEqual("test", res.Value.Second.Term.Value);
        }

        [Test]
        public void ShouldFindDisagreementInHeadParamOneAndReturnTheVariableAndLiteral()
        {
            var code = "head(X).\nhead(a(b(X))).";
            var program = _parser.ParseFromString(code);
            var rule1 = program.Statements.First();
            var rule2 = program.Statements.Last();

            var res = _finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsSuccess);
            Assert.IsNotNull(res.Value);
            Assert.AreEqual("X", res.Value.First.Term.Value);
            Assert.AreEqual("a(b(X))", res.Value.Second.Literal.ToString());
        }

        [Test]
        public void ShouldFindTheNafDifferenceAndThrowAnError()
        {
            var code = "head(a) :- not b.\nhead(a) :- b.";
            var program = _parser.ParseFromString(code);
            var rule1 = program.RuleList.First();
            var rule2 = program.RuleList.Last();

            var res = _finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsError);
        }

        [Test]
        public void ShouldFindTheNegativeDifferenceAndThrowAnError()
        {
            var code = "head(a) :- -b.\nhead(a) :- b.";
            var program = _parser.ParseFromString(code);
            var rule1 = program.RuleList.First();
            var rule2 = program.RuleList.Last();

            var res = _finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsError);
        }

        [Test]
        public void ShouldFindNestedVariable()
        {
            var code = "head(a) :- a(X).\nhead(a) :- a(b(x)).";
            var program = _parser.ParseFromString(code);
            var rule1 = program.RuleList.First();
            var rule2 = program.RuleList.Last();

            var res = _finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsSuccess);
            Assert.IsNotNull(res.Value);
            Assert.AreEqual("X", res.Value.First.Term.Value);
            Assert.AreEqual("b(x)", res.Value.Second.Literal.ToString());
        }
    }
}
