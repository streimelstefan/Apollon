namespace Apollon.Test
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Unification;
    using Apollon.Lib.Unification.Substitutioners;
    using AppollonParser;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class UnifierTests
    {
        private IUnifier unifier = new Unifier();
        private ApollonParser parser = new();

        [SetUp]
        public void Setup()
        {
            this.unifier = new Unifier();
            this.parser = new ApollonParser();
        }

        [Test]
        public void ShouldUnifyAtomsAndReturnMappingWhereXIsA()
        {
            string code = "head(X).\nhead(a).";
            Program program = this.parser.ParseFromString(code);
            Lib.Rules.Statement rule1 = program.Statements.First();
            Lib.Rules.Statement rule2 = program.Statements.Last();

            UnificationResult res = this.unifier.Unify(rule1.Head, rule2.Head);

            Assert.IsTrue(res.IsSuccess);

            Mapping[] mappings = res.Value.Mappings.ToArray();

            Assert.AreEqual(1, mappings.Length);
            Assert.AreEqual("X -> a", mappings[0].ToString());
        }

        [Test]
        public void ShouldNotUnifyAtoms()
        {
            string code = "head1(X).\nhead(a).";
            Program program = this.parser.ParseFromString(code);
            Lib.Rules.Statement rule1 = program.Statements.First();
            Lib.Rules.Statement rule2 = program.Statements.Last();

            UnificationResult res = this.unifier.Unify(rule1.Head, rule2.Head);

            Assert.IsTrue(res.IsError);
        }

        [Test]
        public void ShouldUnifyComplexAtoms()
        {
            string code = "f(a, X, g(Y)).\nf(Z, h(Z), g(b)).";
            Program program = this.parser.ParseFromString(code);
            Lib.Rules.Statement rule1 = program.Statements.First();
            Lib.Rules.Statement rule2 = program.Statements.Last();

            UnificationResult res = this.unifier.Unify(rule1.Head, rule2.Head);

            Assert.IsTrue(res.IsSuccess);

            Mapping[] mappings = res.Value.Mappings.ToArray();

            Assert.AreEqual(3, mappings.Length);
            Assert.AreEqual("Z -> a", mappings[0].ToString());
            Assert.AreEqual("X -> h(a)", mappings[1].ToString());
            Assert.AreEqual("Y -> b", mappings[2].ToString());
        }

        [Test]
        public void ShouldUnfiComplexAtomsInBody()
        {
            string code = ":- f(a, X, g(Y)).\n:- f(Z, h(Z), g(b)).";
            Program program = this.parser.ParseFromString(code);
            Lib.Rules.Statement rule1 = program.Statements.First();
            Lib.Rules.Statement rule2 = program.Statements.Last();

            UnificationResult res = this.unifier.Unify(rule1.Body.First(), rule2.Body.First());

            Assert.IsTrue(res.IsSuccess);

            Mapping[] mappings = res.Value.Mappings.ToArray();

            Assert.AreEqual(3, mappings.Length);
            Assert.AreEqual("Z -> a", mappings[0].ToString());
            Assert.AreEqual("X -> h(a)", mappings[1].ToString());
            Assert.AreEqual("Y -> b", mappings[2].ToString());
        }

        [Test]
        public void ShouldUnifyUsingExistingSubstitution()
        {
            string code = ":- f(X).\n:- f(X).";
            Program program = this.parser.ParseFromString(code);
            Lib.Rules.Statement rule1 = program.Statements.First();
            Lib.Rules.Statement rule2 = program.Statements.Last();

            Substitution sub = new();
            sub.Add(new Term("X"), new AtomParam(new Term("a")));
            UnificationResult res = this.unifier.Unify(rule1.Body.First(), rule2.Body.First(), sub);

            Assert.IsTrue(res.IsSuccess);

            Mapping[] mappings = res.Value.Mappings.ToArray();

            Assert.AreEqual(1, mappings.Length);
            Assert.AreEqual("X -> a", mappings[0].ToString());
        }
    }
}
