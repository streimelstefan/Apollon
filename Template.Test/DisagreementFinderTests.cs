namespace Apollon.Test
{
    using Apollon.Lib.Unification.DisagreementFinders;
    using AppollonParser;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class DisagreementFinderTests
    {
        private IDisagreementFinder finder = new DisagreementFinder();
        private ApollonParser parser = new();

        [SetUp]
        public void Setup()
        {
            this.finder = new DisagreementFinder();
            this.parser = new ApollonParser();
        }

        [Test]
        public void ShouldFindThatRuleNameIsDifferentAndReturnError()
        {
            string code = "head1(X).\nhead2(X).";
            Lib.Program program = this.parser.ParseFromString(code);
            Lib.Rules.Statement rule1 = program.Statements.First();
            Lib.Rules.Statement rule2 = program.Statements.Last();

            DisagreementResult res = this.finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsError);
        }

        [Test]
        public void ShouldFindDisagreementInHeadParamOneAndReturnTheVariableAndTerm()
        {
            string code = "head(X).\nhead(test).";
            Lib.Program program = this.parser.ParseFromString(code);
            Lib.Rules.Statement rule1 = program.Statements.First();
            Lib.Rules.Statement rule2 = program.Statements.Last();

            DisagreementResult res = this.finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsSuccess);
            Assert.IsNotNull(res.Value);
            Assert.AreEqual("X", res.Value.First.Term.Value);
            Assert.AreEqual("test", res.Value.Second.Term.Value);
        }

        [Test]
        public void ShouldFindDisagreementInHeadParamOneAndReturnTheVariableAndLiteral()
        {
            string code = "head(X).\nhead(a(b(X))).";
            Lib.Program program = this.parser.ParseFromString(code);
            Lib.Rules.Statement rule1 = program.Statements.First();
            Lib.Rules.Statement rule2 = program.Statements.Last();

            DisagreementResult res = this.finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsSuccess);
            Assert.IsNotNull(res.Value);
            Assert.AreEqual("X", res.Value.First.Term.Value);
            Assert.AreEqual("a(b(X))", res.Value.Second.Literal.ToString());
        }

        [Test]
        public void ShouldFindTheNafDifferenceAndThrowAnError()
        {
            string code = "head(a) :- not b.\nhead(a) :- b.";
            Lib.Program program = this.parser.ParseFromString(code);
            Lib.Rules.Rule rule1 = program.RuleList.First();
            Lib.Rules.Rule rule2 = program.RuleList.Last();

            DisagreementResult res = this.finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsError);
        }

        [Test]
        public void ShouldFindTheNegativeDifferenceAndThrowAnError()
        {
            string code = "head(a) :- -b.\nhead(a) :- b.";
            Lib.Program program = this.parser.ParseFromString(code);
            Lib.Rules.Rule rule1 = program.RuleList.First();
            Lib.Rules.Rule rule2 = program.RuleList.Last();

            DisagreementResult res = this.finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsError);
        }

        [Test]
        public void ShouldFindNestedVariable()
        {
            string code = "head(a) :- a(X).\nhead(a) :- a(b(x)).";
            Lib.Program program = this.parser.ParseFromString(code);
            Lib.Rules.Rule rule1 = program.RuleList.First();
            Lib.Rules.Rule rule2 = program.RuleList.Last();

            DisagreementResult res = this.finder.FindDisagreement(rule1, rule2);

            Assert.IsTrue(res.IsSuccess);
            Assert.IsNotNull(res.Value);
            Assert.AreEqual("X", res.Value.First.Term.Value);
            Assert.AreEqual("b(x)", res.Value.Second.Literal.ToString());
        }
    }
}
