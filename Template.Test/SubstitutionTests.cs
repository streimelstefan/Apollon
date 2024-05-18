using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
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
    public class SubstitutionTests
    {
        private ISubstitution _sub = new Substitution();
        private ApollonParser _parser = new ApollonParser();

        [SetUp]
        public void Setup()
        {
            _sub = new Substitution();
            _parser = new ApollonParser();
        }


        [Test] 
        public void ShouldSubstitueXCorrectly()
        {
            _sub.Add(new Term("X"), new AtomParam(new Term("stefan")));
            Statement statement = new Rule(new Literal(new Atom("likes", new AtomParam(new Term("X"))), false, false));

            var substituted = _sub.Apply(statement);
            Assert.IsNotNull(substituted);
            Assert.AreEqual("stefan", substituted.Head.Atom.ParamList[0].Term.Value);
        }

        [Test]
        public void ShouldSubstitueYWithAtom()
        {
            _sub.Add(new Term("Y"), new AtomParam(new Literal(new Atom("best", new AtomParam(new Term("test"))), false, false)));
            Statement statement = new Rule(new Literal(new Atom("likes", new AtomParam(new Term("Y"))), false, false));
            
            var substituted = _sub.Apply(statement);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("best", substituted.Head.Atom.ParamList[0].Literal.Atom.Name);
            Assert.AreEqual("test", substituted.Head.Atom.ParamList[0].Literal.Atom.ParamList[0].Term.Value);
        }

        [Test]
        public void ShouldSubstituteXInBodyWithTerm()
        {
            var code = "likes(X) :- not likes(X).";
            var program = _parser.ParseFromString(code);
            var rule = program.RuleList.First();
            _sub.Add(new Term("X"), new AtomParam(new Term("stefan")));

            var substituted = _sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(stefan) :- not likes(stefan).", substituted.ToString());
        }

        [Test]
        public void ShouldSubstituteXInBodyWithAtom()
        {
            var code = "likes(X) :- not likes(X).";
            var program = _parser.ParseFromString(code);
            var rule = program.RuleList.First();
            _sub.Add(new Term("X"), new AtomParam(new Literal(new Atom("hates", new AtomParam(new Term("stefan"))), false, false)));

            var substituted = _sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(hates(stefan)) :- not likes(hates(stefan)).", substituted.ToString());
        }

        [Test]
        public void ShouldSubstituteXInBodyWithNegativeLiteral()
        {
            var code = "likes(X) :- not likes(X).";
            var program = _parser.ParseFromString(code);
            var rule = program.RuleList.First();
            _sub.Add(new Term("X"), new AtomParam(new Literal(new Atom("hates", new AtomParam(new Term("stefan"))), false, true)));

            var substituted = _sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(-hates(stefan)) :- not likes(-hates(stefan)).", substituted.ToString());
        }

        [Test]
        public void ShouldSubstituteXInOperation()
        {
            var code = "likes(X) :- X != 0.";
            var program = _parser.ParseFromString(code);
            var rule = program.RuleList.First();
            _sub.Add(new Term("X"), new AtomParam(new Literal(new Atom("hates", new AtomParam(new Term("stefan"))), false, true)));

            var substituted = _sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(-hates(stefan)) :- -hates(stefan) != 0().", substituted.ToString());
        }

        [Test]
        public void ShouldSubstituteXAsTermInOperation()
        {
            var code = "likes(X) :- X != 0.";
            var program = _parser.ParseFromString(code);
            var rule = program.RuleList.First();
            _sub.Add(new Term("X"), new AtomParam(new Term("0")));

            var substituted = _sub.Apply(rule);

            Assert.IsNotNull(substituted);
            Assert.AreEqual("likes(0) :- 0() != 0().", substituted.ToString());
        }

        [Test]
        public void ShouldBackPropagateSimpleMappings()
        {
            var newSub = new Substitution();
            newSub.Add(new Term("X"), new AtomParam(new Literal(new Atom("a"), false, false)));

            _sub.Add(new Term("Y"), new AtomParam(new Term("X")));

            _sub.BackPropagate(newSub);

            var mappings = _sub.Mappings;
            Assert.AreEqual(1, mappings.Count());
            var mapping = mappings.First();

            Assert.IsNotNull(mapping);
            Assert.AreEqual(true, mapping.MapsTo.IsLiteral);
            Assert.AreEqual("Y -> a()", mapping.ToString());
        }

        [Test]
        public void ShouldShouldUnionPVLsWhenBackPropagating()
        {
            var newSub = new Substitution();
            newSub.Add(new Term("X"), new AtomParam(new Term("Y", new PVL(new AtomParam[] {new AtomParam(new Term("a"))}))));

            _sub.Add(new Term("Y"), new AtomParam(new Term("X")));

            _sub.BackPropagate(newSub);

            var mappings = _sub.Mappings;
            Assert.AreEqual(1, mappings.Count());
            var mapping = mappings.First();

            Assert.IsNotNull(mapping);
            Assert.AreEqual("Y - {\\a} -> X - {\\a}", mapping.ToString());
        }

        [Test]
        public void ShouldDoNothingIfThereIsNothingToBackPropagate()
        {
            var newSub = new Substitution();
            newSub.Add(new Term("Y"), new AtomParam(new Term("X", new PVL(new AtomParam[] { new AtomParam(new Term("a")) }))));

            _sub.Add(new Term("Y"), new AtomParam(new Term("X")));

            _sub.BackPropagate(newSub);

            var mappings = _sub.Mappings;
            Assert.AreEqual(1, mappings.Count());
            var mapping = mappings.First();

            Assert.IsNotNull(mapping);
            Assert.AreEqual("Y -> X", mapping.ToString());
        }


    }
}
