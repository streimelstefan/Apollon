namespace Apollon.Test
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Unification;
    using Apollon.Lib.Unification.Substitutioners;
    using AppollonParser;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class SubstitutionTests
    {
        private Substitution sub = new();
        private ApollonParser parser = new();

        [SetUp]
        public void Setup()
        {
            this.sub = new Substitution();
            this.parser = new ApollonParser();
        }

        [Test]
        public void ShouldSubstitueXCorrectly()
        {
            this.sub.Add(new Term("X"), new AtomParam(new Term("stefan")));
            Statement statement = new Rule(new Literal(new Atom("likes", new AtomParam(new Term("X"))), false, false));

            Statement substituted = this.sub.Apply(statement);
            Assert.IsNotNull(substituted);
            Assert.AreEqual("stefan", substituted.Head.Atom.ParamList[0].Term.Value);
        }

        [Test]
        public void ShouldSubstitueYWithAtom()
        {
            this.sub.Add(new Term("Y"), new AtomParam(new Literal(new Atom("best", new AtomParam(new Term("test"))), false, false)));
            Statement statement = new Rule(new Literal(new Atom("likes", new AtomParam(new Term("Y"))), false, false));

            Statement substituted = this.sub.Apply(statement);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("best", substituted.Head.Atom.ParamList[0].Literal.Atom.Name);
            Assert.AreEqual("test", substituted.Head.Atom.ParamList[0].Literal.Atom.ParamList[0].Term.Value);
        }

        [Test]
        public void ShouldSubstituteXInBodyWithTerm()
        {
            string code = "likes(X) :- not likes(X).";
            Program program = this.parser.ParseFromString(code);
            Rule rule = program.RuleList.First();
            this.sub.Add(new Term("X"), new AtomParam(new Term("stefan")));

            Statement substituted = this.sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(stefan) :- not likes(stefan).", substituted.ToString());
        }

        [Test]
        public void ShouldSubstituteXInBodyWithAtom()
        {
            string code = "likes(X) :- not likes(X).";
            Program program = this.parser.ParseFromString(code);
            Rule rule = program.RuleList.First();
            this.sub.Add(new Term("X"), new AtomParam(new Literal(new Atom("hates", new AtomParam(new Term("stefan"))), false, false)));

            Statement substituted = this.sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(hates(stefan)) :- not likes(hates(stefan)).", substituted.ToString());
        }

        [Test]
        public void ShouldSubstituteXInBodyWithNegativeLiteral()
        {
            string code = "likes(X) :- not likes(X).";
            Program program = this.parser.ParseFromString(code);
            Rule rule = program.RuleList.First();
            this.sub.Add(new Term("X"), new AtomParam(new Literal(new Atom("hates", new AtomParam(new Term("stefan"))), false, true)));

            Statement substituted = this.sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(-hates(stefan)) :- not likes(-hates(stefan)).", substituted.ToString());
        }

        [Test]
        public void ShouldSubstituteXInOperation()
        {
            string code = "likes(X) :- X != 0.";
            Program program = this.parser.ParseFromString(code);
            Rule rule = program.RuleList.First();
            this.sub.Add(new Term("X"), new AtomParam(new Literal(new Atom("hates", new AtomParam(new Term("stefan"))), false, true)));

            Statement substituted = this.sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(-hates(stefan)) :- -hates(stefan) != 0.", substituted.ToString());
        }

        [Test]
        public void ShouldSubstituteXAsTermInOperation()
        {
            string code = "likes(X) :- X != 0.";
            Program program = this.parser.ParseFromString(code);
            Rule rule = program.RuleList.First();
            this.sub.Add(new Term("X"), new AtomParam(new Term("0")));

            Statement substituted = this.sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(0) :- 0 != 0.", substituted.ToString());
        }

        [Test]
        public void ShouldBackPropagateSimpleMappings()
        {
            Substitution newSub = new();
            newSub.Add(new Term("X"), new AtomParam(new Literal(new Atom("a"), false, false)));

            this.sub.Add(new Term("Y"), new AtomParam(new Term("X")));

            this.sub.BackPropagate(newSub);

            IEnumerable<Mapping> mappings = this.sub.Mappings;
            Assert.AreEqual(1, mappings.Count());
            Mapping mapping = mappings.First();

            Assert.IsNotNull(mapping);
            Assert.AreEqual(true, mapping.MapsTo.IsLiteral);
            Assert.AreEqual("Y -> a()", mapping.ToString());
        }

        [Test]
        public void ShouldShouldUnionPVLsWhenBackPropagating()
        {
            Substitution newSub = new();
            newSub.Add(new Term("X"), new AtomParam(new Term("Y", new PVL(new AtomParam[] { new(new Term("a")) }))));

            this.sub.Add(new Term("Y"), new AtomParam(new Term("X")));

            this.sub.BackPropagate(newSub);

            IEnumerable<Mapping> mappings = this.sub.Mappings;
            Assert.AreEqual(1, mappings.Count());
            Mapping mapping = mappings.First();

            Assert.IsNotNull(mapping);
            Assert.AreEqual("Y - {\\a} -> X - {\\a}", mapping.ToString());
        }

        [Test]
        public void ShouldAddVariablesThatAreNotInduced()
        {
            Substitution newSub = new();
            newSub.Add(new Term("B"), new AtomParam(new Term("a", new PVL(new AtomParam[] { new(new Term("a")) }))));

            this.sub.Add(new Term("Y"), new AtomParam(new Term("X")));

            this.sub.BackPropagate(newSub);

            IEnumerable<Mapping> mappings = this.sub.Mappings;
            Assert.AreEqual(2, mappings.Count());
            Mapping mapping = mappings.First();

            Assert.IsNotNull(mapping);
            Assert.AreEqual("Y -> X", mapping.ToString());
            Assert.AreEqual("B -> a - {\\a}", mappings.Last().ToString());
        }

        [Test]
        public void ShouldContractIfThereAreLists()
        {
            this.sub.Add(new Term("X"), new AtomParam(new Term("Y")));
            this.sub.Add(new Term("Y"), new AtomParam(new Term("a")));

            this.sub.Contract();

            IEnumerable<Mapping> mappings = this.sub.Mappings;

            Assert.AreEqual(1, mappings.Count());
            Assert.AreEqual("X -> a", mappings.First().ToString());
        }

        [Test]
        public void ShouldContractIfThereAreListsWithMoreThenTwoElements()
        {
            this.sub.Add(new Term("X"), new AtomParam(new Term("Y")));
            this.sub.Add(new Term("Y"), new AtomParam(new Term("Z")));
            this.sub.Add(new Term("Z"), new AtomParam(new Term("a")));

            this.sub.Contract();

            IEnumerable<Mapping> mappings = this.sub.Mappings;

            Assert.AreEqual(1, mappings.Count());
            Assert.AreEqual("X -> a", mappings.First().ToString());
        }

        [Test]
        public void ShouldDoNothingIfThereAreNoLists()
        {
            this.sub.Add(new Term("X"), new AtomParam(new Term("Y")));
            this.sub.Add(new Term("Z"), new AtomParam(new Term("a")));

            this.sub.Contract();

            IEnumerable<Mapping> mappings = this.sub.Mappings;

            Assert.AreEqual(2, mappings.Count());
            Assert.AreEqual("X -> Y", mappings.First().ToString());
            Assert.AreEqual("Z -> a", mappings.Last().ToString());
        }

        [Test]
        public void ShouldDoNothingIfTheSubIsEmpty()
        {
            this.sub.Contract();

            IEnumerable<Mapping> mappings = this.sub.Mappings;

            Assert.AreEqual(0, mappings.Count());
        }

        [Test]
        public void ShouldAddValueTermEvenIfItIsTheSameAsTheVariable()
        {
            this.sub.Add(new Term("X"), new AtomParam(new Term("X")));

            IEnumerable<Mapping> mappings = this.sub.Mappings;

            Assert.AreEqual(1, mappings.Count());
        }

        [Test]
        public void ShouldAddValueTermIfThePVLsAreDifferent()
        {
            this.sub.Add(new Term("X"), new AtomParam(new Term("X", new PVL(new AtomParam[] { new(new Term("a")) }))));

            IEnumerable<Mapping> mappings = this.sub.Mappings;

            Assert.AreEqual(1, mappings.Count());
        }
    }
}
