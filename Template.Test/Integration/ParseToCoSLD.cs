using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib;
using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.Resolution;
using Apollon.Lib.Resolution.CoSLD;
using Apollon.Lib.Logging;

namespace Apollon.Test.Integration
{
    [TestFixture]
    internal class ParseToCoSLD
    {

        private Solver _solver = new Solver();
        private ApollonParser _parser = new ApollonParser();

        [SetUp]
        public void Setup()
        {
            _solver = new Solver();
            _solver.Logger.Level = LogLevel.Trace;
            _parser = new ApollonParser();
        }

        [Test]
        public void ShouldSuceed()
        {
            var code = "p(a).\r\nq(X) :- p(X), r(X).\r\nr(a).";
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var goal = new BodyPart(new Literal(new Atom("q", new AtomParam(new Term("a"))), false, false), null);

            var results = _solver.Solve(new BodyPart[] { goal });
            var res = results.First().CHS;

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

            var results = _solver.Solve(new BodyPart[] { goal });
            var res = results.First().CHS;

            Assert.IsTrue(res.IsEmpty);
        }


        [Test]
        public void ShouldNotSuceedWithNAFGoal()
        {
            var code = "p(a).\r\nq(X) :- p(X), r(X).\r\nr(a).";
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var goal = new BodyPart(new Literal(new Atom("q", new AtomParam(new Term("a"))), true, false), null);

            var results = _solver.Solve(new BodyPart[] { goal });
            var res = results.First();

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

            var results = _solver.Solve(new BodyPart[] { goal });
            var res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);
            Assert.AreEqual("parent(alice, bob)", res.CHS.Literals[0].ToString());
            Assert.AreEqual("ancestor(alice, bob)", res.CHS.Literals[1].ToString());

            Assert.IsNotNull(res.Substitution);
            Assert.AreEqual("{ X -> bob }", res.Substitution.ToString());
        }

        [Test]
        public void ShouldRunPQLoop()
        {
            var code = "p(X) :- not q(X).\r\nq(X) :- not p(X).";
            var query = _parser.ParseQueryFromString("p(X).");
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var results = _solver.Solve(query);
            var res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);
            Assert.AreEqual("not q(V/0)", res.CHS.Literals[0].ToString());
            Assert.AreEqual("p(X)", res.CHS.Literals[1].ToString());
            Assert.AreEqual("{ X -> RV/0 }", res.Substitution.ToString());
        }

        [Test]
        public void ShouldRunLoopVar()
        {
            var code = "p(X) :- not q(X).\r\nq(X) :- not p(X).\r\nr(X) :- X != 3, X != 4, q(X).";
            var query = _parser.ParseQueryFromString("p(X), r(Y).");
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var results = _solver.Solve(query);
            var res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);

            var literals = res.CHS.Literals;
            Assert.AreEqual(5, literals.Count());
            Assert.AreEqual("not q(3)", literals[0].ToString()); // this should be not q(3) but not sure how or if to implement that.
            // it should be set to three when a value gets checked in the chs. But we losse al context once a literal enters the chs.
            // so not sure how to fix that currently.
            Assert.AreEqual("p(3)", literals[1].ToString()); // this should be p(3) but not sure how or if to implement that.
            Assert.AreEqual("not p(V/0 - {\\3() \\4()})", literals[2].ToString());
            Assert.AreEqual("q(X - {\\3() \\4()})", literals[3].ToString());
            Assert.AreEqual("r(X - {\\3() \\4()})", literals[4].ToString());
        }

        [Test]
        public void ShouldResolveForAlls()
        {
            var code = "faster(bunny, turtle).\r\nfaster(cat, bunny).\r\n\r\nis_faster(X, Y) :- faster(X, Y).\r\nis_faster(X, Y) :- faster(X, Z), is_faster(Z, Y).\r\n\r\nfastest(X) :- not is_faster(Y, X). ";
            var query = _parser.ParseQueryFromString("fastest(bunny).");
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var results = _solver.Solve(query);
            var res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);

            var literals = res.CHS.Literals;
            Assert.AreEqual(4, literals.Count());
            Assert.AreEqual("not faster(V/0 - {\\bunny() \\cat()}, bunny)", literals[0].ToString());
            Assert.AreEqual("not faster(V/0 - {\\bunny() \\cat()}, V/1)", literals[1].ToString());
            Assert.AreEqual("not is_faster(V/0 - {\\bunny() \\cat()}, bunny)", literals[2].ToString());
            Assert.AreEqual("fastest(bunny)", literals[3].ToString());
        }
    }
}
