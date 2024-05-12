using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.DisagreementFinders;
using Apollon.Lib.Unification.Substitutioners;
using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Test
{
    [TestFixture]
    public class UnifierTests
    {
        private IUnifier _unifier = new Unifier();
        private ApollonParser _parser = new ApollonParser();


        [SetUp]
        public void Setup()
        {
            _unifier = new Unifier();
            _parser = new ApollonParser();
        }

        [Test]
        public void ShouldUnifyAtomsAndReturnMappingWhereXIsA()
        {
            var code = "head(X).\nhead(a).";
            var program = _parser.ParseFromString(code);
            var rule1 = program.Statements.First();
            var rule2 = program.Statements.Last();

            var res = _unifier.Unify(rule1.Head, rule2.Head);

            Assert.IsTrue(res.IsSuccess);

            var mappings = res.Value.Mappings.ToArray();

            Assert.AreEqual(1, mappings.Length);
            Assert.AreEqual("X -> a", mappings[0].ToString());
        }

        [Test]
        public void ShouldNotUnifyAtoms()
        {
            var code = "head1(X).\nhead(a).";
            var program = _parser.ParseFromString(code);
            var rule1 = program.Statements.First();
            var rule2 = program.Statements.Last();

            var res = _unifier.Unify(rule1.Head, rule2.Head);

            Assert.IsTrue(res.IsError);
        }

        [Test]
        public void ShouldUnifyComplexAtoms()
        {
            var code = "f(a, X, g(Y)).\nf(Z, h(Z), g(b)).";
            var program = _parser.ParseFromString(code);
            var rule1 = program.Statements.First();
            var rule2 = program.Statements.Last();

            var res = _unifier.Unify(rule1.Head, rule2.Head);

            Assert.IsTrue(res.IsSuccess);

            var mappings = res.Value.Mappings.ToArray();

            Assert.AreEqual(3, mappings.Length);
            Assert.AreEqual("Z -> a", mappings[0].ToString());
            Assert.AreEqual("X -> h(a)", mappings[1].ToString());
            Assert.AreEqual("Y -> b", mappings[2].ToString());
        }

        [Test]
        public void ShouldUnfiComplexAtomsInBody()
        {
            var code = ":- f(a, X, g(Y)).\n:- f(Z, h(Z), g(b)).";
            var program = _parser.ParseFromString(code);
            var rule1 = program.Statements.First();
            var rule2 = program.Statements.Last();

            var res = _unifier.Unify(rule1.Body.First(), rule2.Body.First());

            Assert.IsTrue(res.IsSuccess);

            var mappings = res.Value.Mappings.ToArray();

            Assert.AreEqual(3, mappings.Length);
            Assert.AreEqual("Z -> a", mappings[0].ToString());
            Assert.AreEqual("X -> h(a)", mappings[1].ToString());
            Assert.AreEqual("Y -> b", mappings[2].ToString());
        }

        [Test]
        public void ShouldUnifyUsingExistingSubstitution()
        {
            var code = ":- f(X).\n:- f(X).";
            var program = _parser.ParseFromString(code);
            var rule1 = program.Statements.First();
            var rule2 = program.Statements.Last();

            var sub = new Substitution();
            sub.Add(new Term("X"), new AtomParam(new Term("a")));
            var res = _unifier.Unify(rule1.Body.First(), rule2.Body.First(), sub);

            Assert.IsTrue(res.IsSuccess);

            var mappings = res.Value.Mappings.ToArray();

            Assert.AreEqual(1, mappings.Length);
            Assert.AreEqual("X -> a", mappings[0].ToString());
        }
    }
}
