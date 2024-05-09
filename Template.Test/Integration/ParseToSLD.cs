using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification;
using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Test.Integration
{
    [TestFixture]
    public class ParseToSLD
    {

        private Solver _solver = new Solver();
        private ApollonParser _parser = new ApollonParser();

        [Test]
        public void ShouldSuceed() 
        {
            var code = "p(a).\r\nq(X) :- p(X), r(X).\r\nr(a).";
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var goal = new BodyPart(new Literal(new Atom("q", new AtomParam(new Term("a"))), false, false), null);

            var res = _solver.Solve(new BodyPart[] { goal }).CHS;

            Assert.IsFalse(res.IsEmpty);
            Assert.AreEqual("{ (p(a)), (r(a)), (q(a)) }", res.ToString());
        }

        [Test]
        public void ShouldNotSuceed()
        {
            var code = "p(a).\r\nq(X) :- p(X), r(X).\r\nr(a).";
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var goal = new BodyPart(new Literal(new Atom("q", new AtomParam(new Term("b"))), false, false), null);

            var res = _solver.Solve(new BodyPart[] { goal }).CHS;

            Assert.IsTrue(res.IsEmpty);
        }


        [Test]
        public void ShouldNotSuceedWithNAFGoal()
        {
            var code = "p(a).\r\nq(X) :- p(X), r(X).\r\nr(a).";
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var goal = new BodyPart(new Literal(new Atom("q", new AtomParam(new Term("a"))), true, false), null);

            var res = _solver.Solve(new BodyPart[] { goal });

            Assert.IsTrue(res.CHS.IsEmpty);
            Assert.IsFalse(res.Substitution.Mappings.Any());
        }

        [Test]
        public void ShouldSuceedWithExampleProgram1()
        {
            var code = "parent(alice, bob).\r\nparent(bob, charlie).\r\nancestor(X, Y) :- parent(X, Y).\r\nancestor(X, Y) :- parent(X, Z), ancestor(Z, Y).\r\n";
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var goal = new BodyPart(new Literal(new Atom("ancestor", new AtomParam(new Term("alice")), new AtomParam(new Term("X"))), false, false), null);

            var res = _solver.Solve(new BodyPart[] { goal });

            Assert.IsFalse(res.CHS.IsEmpty);
            Assert.AreEqual("parent(alice, bob)", res.CHS.Literals[0].ToString());
            Assert.AreEqual("ancestor(alice, bob)", res.CHS.Literals[1].ToString());

            Assert.IsNotNull(res.Substitution);
            Assert.AreEqual("{ X -> bob }", res.Substitution.ToString());
        }

    }
}
