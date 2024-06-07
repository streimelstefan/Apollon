namespace Apollon.Test.Integration
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Logging;
    using Apollon.Lib.Resolution;
    using Apollon.Lib.Rules;
    using AppollonParser;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    internal class ParseToCoSLD
    {
        private Solver solver = new();
        private ApollonParser parser = new();

        [SetUp]
        public void Setup()
        {
            this.solver = new Solver();
            this.solver.Logger.Level = LogLevel.Silly;
            this.parser = new ApollonParser();
        }

        [Test]
        public void ShouldSuceed()
        {
            string code = "p(a).\r\nq(X) :- p(X), r(X).\r\nr(a).";
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            BodyPart goal = new(new Literal(new Atom("q", new AtomParam(new Term("a"))), false, false), null);

            IEnumerable<ResolutionResult> results = this.solver.Solve(new BodyPart[] { goal });
            Lib.Resolution.CallStackAndCHS.CHS res = results.First().CHS;

            Assert.IsFalse(res.IsEmpty);
            Assert.AreEqual("{ (p(a)), (r(a)), (q(a)) }", res.ToString());
        }

        [Test]
        public void ShouldNotSuceed()
        {
            string code = "p(a).\r\nq(X) :- p(X), r(X).\r\nr(a).";
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            BodyPart goal = new(new Literal(new Atom("q", new AtomParam(new Term("b"))), false, false), null);

            IEnumerable<ResolutionResult> results = this.solver.Solve(new BodyPart[] { goal });
            Lib.Resolution.CallStackAndCHS.CHS res = results.First().CHS;

            Assert.IsTrue(res.IsEmpty);
        }

        [Test]
        public void ShouldNotSuceedWithNAFGoal()
        {
            string code = "p(a).\r\nq(X) :- p(X), r(X).\r\nr(a).";
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            BodyPart goal = new(new Literal(new Atom("q", new AtomParam(new Term("a"))), true, false), null);

            IEnumerable<ResolutionResult> results = this.solver.Solve(new BodyPart[] { goal });
            ResolutionResult res = results.First();

            Assert.IsTrue(res.CHS.IsEmpty);
            Assert.IsFalse(res.Substitution.Mappings.Any());
        }

        [Test]
        public void ShouldSuceedWithExampleProgram1AndReturnTwoAnswerSets()
        {
            string code = "parent(alice, bob).\r\nparent(bob, charlie).\r\nancestor(X, Y) :- parent(X, Y).\r\nancestor(X, Y) :- parent(X, Z), ancestor(Z, Y).\r\n";
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            BodyPart goal = new(new Literal(new Atom("ancestor", new AtomParam(new Term("alice")), new AtomParam(new Term("X"))), false, false), null);

            IEnumerable<ResolutionResult> results = this.solver.Solve(new BodyPart[] { goal });
            ResolutionResult[] res = results.ToArray();
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
            string code = "p(X) :- not q(X).\r\nq(X) :- not p(X).";
            BodyPart[] query = this.parser.ParseQueryFromString("p(X).");
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);
            Assert.AreEqual("not q(RV/4)", res.CHS.Literals[0].ToString());
            Assert.AreEqual("p(RV/1)", res.CHS.Literals[1].ToString());
            Assert.AreEqual("{ X -> RV/1 }", res.Substitution.ToString());
        }

        [Test]
        public void ShouldRunLoopVar()
        {
            string code = "p(X) :- not q(X).\r\nq(X) :- not p(X).\r\nr(X) :- X != 3, X != 4, q(X).";
            BodyPart[] query = this.parser.ParseQueryFromString("p(X), r(Y).");
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);

            List<Literal> literals = res.CHS.Literals;
            Assert.AreEqual(5, literals.Count());

            Assert.AreEqual("not q(3)", literals[0].ToString()); // this should be not q(3) but not sure how or if to implement that.
            // it should be set to three when a value gets checked in the chs. But we losse al context once a literal enters the chs.
            // so not sure how to fix that currently.
            Assert.AreEqual("p(3)", literals[1].ToString()); // this should be p(3) but not sure how or if to implement that.
            Assert.AreEqual("not p(RV/11 - {\\3 \\4})", literals[2].ToString());
            Assert.AreEqual("q(RV/8 - {\\3 \\4})", literals[3].ToString());
            Assert.AreEqual("r(RV/6 - {\\3 \\4})", literals[4].ToString());
        }

        [Test]
        public void ShouldResolveForAlls()
        {
            string code = "faster(bunny, turtle).\r\nfaster(cat, bunny).\r\n\r\nis_faster(X, Y) :- faster(X, Y).\r\nis_faster(X, Y) :- faster(X, Z), is_faster(Z, Y).\r\n\r\nfastest(X) :- not is_faster(Y, X). ";
            BodyPart[] query = this.parser.ParseQueryFromString("fastest(bunny).");
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);

            List<Literal> literals = res.CHS.Literals;
            Assert.AreEqual(4, literals.Count());
            Assert.AreEqual("not faster(RV/13 - {\\bunny \\cat}, bunny)", literals[0].ToString());
            Assert.AreEqual("not faster(RV/32 - {\\bunny \\cat}, RV/33)", literals[1].ToString());
            Assert.AreEqual("not is_faster(RV/9 - {\\bunny \\cat}, bunny)", literals[2].ToString());
            Assert.AreEqual("fastest(bunny)", literals[3].ToString());
        }

        [Test]
        public void ShouldResolveForAllExampleFromThePaper()
        {
            string code = "p :- not q(X).\r\nq(Y) :- Y = a.\r\nq(Y) :- Y != a.";
            BodyPart[] query = this.parser.ParseQueryFromString("not p.");
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);

            List<Literal> literals = res.CHS.Literals;
            Assert.AreEqual(3, literals.Count());
            Assert.AreEqual("q(RV/5 - {\\a})", literals[0].ToString());
            Assert.AreEqual("q(a)", literals[1].ToString());
            Assert.AreEqual("not p()", literals[2].ToString());
        }

        [Test]
        public void ShouldRunGPA()
        {
            var code = "eligible(X) :- highGPA(X).\r\neligible(X) :- special(X), fairGPA(X).\r\n-eligible(X) :- -special(X), -highGPA(X).\r\ninterview(X) :- not eligible(X), not -eligible(X).\r\nfairGPA(john).\r\n-highGPA(john).";
            BodyPart[] query = this.parser.ParseQueryFromString("interview(john).");
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult res = results.First();

            Assert.IsFalse(res.CHS.IsEmpty);

            List<Literal> literals = res.CHS.Literals;
            Assert.AreEqual(10, literals.Count());
            Assert.AreEqual("not highGPA(john)", literals[0].ToString());
            Assert.AreEqual("not special(john)", literals[1].ToString());
            Assert.AreEqual("not eligible(john)", literals[2].ToString());
            Assert.AreEqual("not -special(john)", literals[3].ToString());
            Assert.AreEqual("not -eligible(john)", literals[4].ToString());
            Assert.AreEqual("interview(john)", literals[5].ToString());
            Assert.AreEqual("not -highGPA(RV/19 - {\\john})", literals[6].ToString());
            Assert.AreEqual("-highGPA(john)", literals[7].ToString());
            Assert.AreEqual("not -special(RV/29)", literals[8].ToString());
            Assert.AreEqual("not -eligible(RV/34)", literals[9].ToString());
        }

        [Test]
        public void ShouldReturnThreeAnswerSets()
        {
            string code = "p(a).\r\np(b).\r\np(c).";
            BodyPart[] query = this.parser.ParseQueryFromString("p(X).");

            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult[] res = results.ToArray();

            Assert.AreEqual(3, res.Length);
            Assert.AreEqual("{ (p(a)) }", res[0].CHS.ToString());
            Assert.AreEqual("{ (p(b)) }", res[1].CHS.ToString());
            Assert.AreEqual("{ (p(c)) }", res[2].CHS.ToString());
        }

        [Test]
        public void ShouldAddOne()
        {
            string code = "addOne(X, Y) :- Y is X + 1.";
            BodyPart[] query = this.parser.ParseQueryFromString("addOne(1, X).");
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult res = results.First();

            Assert.IsNotNull(res);
        }

        [Test]
        public void ShouldRunHamCycle()
        {
            Program program = this.parser.ParseFromFile("../../../TestPrograms/Hamcycle.apo");
            BodyPart[] query = this.parser.ParseQueryFromString("chosen(1,2).");

            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult res = results.First();

            Assert.IsNotNull(res);
        }

        [Test]
        public void ShouldDoSomething()
        {
            Program program = this.parser.ParseFromString("a(X) :- X = a(X).");
            BodyPart[] query = this.parser.ParseQueryFromString("a(X).");

            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult res = results.First();

            Assert.IsNotNull(res);
        }

        [Test]
        public void ShouldSucceedExampleProgram()
        {
            string code = "p :- not q.\r\nq :- not r.\r\nr :- not p.\r\nq :- not p.";
            BodyPart[] query = this.parser.ParseQueryFromString("q.");
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult[] res = results.ToArray();

            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Length);

            Assert.AreEqual("{ (not p()), (q()), (r()) }", res[0].CHS.ToString());
            Assert.AreEqual(0, res[0].Substitution.Mappings.Count());
            Assert.IsTrue(res[0].Success);
        }

        [Test]
        public void ShouldSucceedExampleProgramWithGoalP()
        {
            string code = "p :- not q.\r\nq :- not r.\r\nr :- not p.\r\nq :- not p.";
            BodyPart[] query = this.parser.ParseQueryFromString("p.");
            Program program = this.parser.ParseFromString(code);
            this.solver.Load(program);

            IEnumerable<ResolutionResult> results = this.solver.Solve(query);
            ResolutionResult[] res = results.ToArray();

            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Length);

            Assert.AreEqual("{ () }", res[0].CHS.ToString());
            Assert.AreEqual(0, res[0].Substitution.Mappings.Count());
            Assert.IsFalse(res[0].Success);
        }
    }
}
