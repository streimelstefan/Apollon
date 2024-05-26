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
            _solver.Logger.Level = LogLevel.Silly;
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
        public void ShouldSuceedWithExampleProgram1AndReturnTwoAnswerSets()
        {
            var code = "parent(alice, bob).\r\nparent(bob, charlie).\r\nancestor(X, Y) :- parent(X, Y).\r\nancestor(X, Y) :- parent(X, Z), ancestor(Z, Y).\r\n";
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var goal = new BodyPart(new Literal(new Atom("ancestor", new AtomParam(new Term("alice")), new AtomParam(new Term("X"))), false, false), null);

            var results = _solver.Solve(new BodyPart[] { goal });
            var res = results.ToArray();
            Assert.AreEqual(res.Length, 2);

            Assert.IsFalse(res[0].CHS.IsEmpty);
            Assert.AreEqual("parent(alice, bob)", res[0].CHS.Literals[0].ToString());
            Assert.AreEqual("ancestor(alice, bob)", res[0].CHS.Literals[1].ToString());

            Assert.IsNotNull(res[0].Substitution);
            Assert.AreEqual("{ X -> bob }", res[0].Substitution.ToString());

            Assert.IsFalse(res[1].CHS.IsEmpty);
            Assert.AreEqual("parent(alice, bob)", res[1].CHS.Literals[0].ToString());
            Assert.AreEqual("parent(bob, charlie)", res[1].CHS.Literals[1].ToString());
            Assert.AreEqual("ancestor(bob, charlie)", res[1].CHS.Literals[2].ToString());
            Assert.AreEqual("ancestor(alice, charlie)", res[1].CHS.Literals[3].ToString());

            Assert.IsNotNull(res[1].Substitution);
            Assert.AreEqual("{ X -> charlie }", res[1].Substitution.ToString());
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
            Assert.AreEqual("not q(RV/4)", res.CHS.Literals[0].ToString());
            Assert.AreEqual("p(RV/1)", res.CHS.Literals[1].ToString());
            Assert.AreEqual("{ X -> RV/1 }", res.Substitution.ToString());
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
            //Assert.AreEqual("not q(3)", literals[0].ToString()); // this should be not q(3) but not sure how or if to implement that.
            // it should be set to three when a value gets checked in the chs. But we losse al context once a literal enters the chs.
            // so not sure how to fix that currently.
            //Assert.AreEqual("p(3)", literals[1].ToString()); // this should be p(3) but not sure how or if to implement that.
            Assert.AreEqual("not p(RV/11 - {\\3 \\4})", literals[2].ToString());
            Assert.AreEqual("q(RV/8 - {\\3 \\4})", literals[3].ToString());
            Assert.AreEqual("r(RV/6 - {\\3 \\4})", literals[4].ToString());
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
            Assert.AreEqual("not faster(RV/13 - {\\bunny \\cat}, bunny)", literals[0].ToString());
            Assert.AreEqual("not faster(RV/32 - {\\bunny \\cat}, RV/33)", literals[1].ToString());
            Assert.AreEqual("not is_faster(RV/9 - {\\bunny \\cat}, bunny)", literals[2].ToString());
            Assert.AreEqual("fastest(bunny)", literals[3].ToString());
        }

        [Test]
        public void ShouldResolveForAllExampleFromThePaper()
        {
            var code = "p :- not q(X).\r\nq(Y) :- Y = a.\r\nq(Y) :- Y != a.";
            var query = _parser.ParseQueryFromString("not p.");
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var results = _solver.Solve(query);
            var res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);

            var literals = res.CHS.Literals;
            Assert.AreEqual(3, literals.Count());
            Assert.AreEqual("q(RV/5 - {\\a})", literals[0].ToString());
            Assert.AreEqual("q(a)", literals[1].ToString());
            Assert.AreEqual("not p()", literals[2].ToString());
        }

        [Test]
        public void ShouldReturnThreeAnswerSets()
        {
            var code = "p(a).\r\np(b).\r\np(c).";
            var query = _parser.ParseQueryFromString("p(X).");

            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var results = _solver.Solve(query);
            var res = results.ToArray();

            Assert.AreEqual(3, res.Length);
            Assert.AreEqual("{ (p(a)) }", res[0].CHS.ToString());
            Assert.AreEqual("{ (p(b)) }", res[1].CHS.ToString());
            Assert.AreEqual("{ (p(c)) }", res[2].CHS.ToString());
        }

        [Test]
        public void ShouldAddOne()
        {
            var code = "addOne(X, Y) :- Y is X + 1.";
            var query = _parser.ParseQueryFromString("addOne(1, X).");
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var results = _solver.Solve(query);
            var res = results.First();

            Assert.IsNotNull(res);
        }

        [Test]
        public void ShouldRunHamCycle()
        {
            var program = _parser.ParseFromFile("../../../TestPrograms/Hamcycle.apo");
            var query = _parser.ParseQueryFromString("chosen(1,2).");

            _solver.Load(program);

            var results = _solver.Solve(query);
            var res = results.First();

            Assert.IsNotNull(res);
        }

        [Test]
        public void ShouldSucceedExampleProgram()
        {
            var code = "p :- not q.\r\nq :- not r.\r\nr :- not p.\r\nq :- not p."; 
            var query = _parser.ParseQueryFromString("q.");
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var results = _solver.Solve(query);
            var res = results.ToArray();

            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Length);

            Assert.AreEqual("{ (not p()), (q()), (r()) }", res[0].CHS.ToString());
            Assert.AreEqual(0, res[0].Substitution.Mappings.Count());
            Assert.IsTrue(res[0].Success);
        }

        [Test]
        public void ShouldSucceedExampleProgramWithGoalP()
        {
            var code = "p :- not q.\r\nq :- not r.\r\nr :- not p.\r\nq :- not p.";
            var query = _parser.ParseQueryFromString("p.");
            var program = _parser.ParseFromString(code);
            _solver.Load(program);

            var results = _solver.Solve(query);
            var res = results.ToArray();

            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Length);

            Assert.AreEqual("{ () }", res[0].CHS.ToString());
            Assert.AreEqual(0, res[0].Substitution.Mappings.Count());
            Assert.IsFalse(res[0].Success);
        }
    }
}
