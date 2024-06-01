namespace Apollon.Test
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Rules.Operations;
    using Apollon.Lib.Unification;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class ExactUnifierTests
    {
        private IUnifier unifier = new ExactUnifier();

        [SetUp]
        public void Setup()
        {
            this.unifier = new ExactUnifier();
        }

        [Test]
        public void ShouldUnifyIfTheyAreExactlyTheSame()
        {
            Literal lit1 = new(new Atom("a", new AtomParam(new Term("X"))), false, false);
            Literal lit2 = new(new Atom("a", new AtomParam(new Term("X"))), false, false);

            UnificationResult res = this.unifier.Unify(lit1, lit2);

            Assert.IsTrue(res.IsSuccess);
            Assert.AreEqual(0, res.Value.Mappings.Count());
        }

        [Test]
        public void ShouldUnifyIfOnlyVariableNamesAreDifferent()
        {
            Literal lit1 = new(new Atom("a", new AtomParam(new Term("X"))), false, false);
            Literal lit2 = new(new Atom("a", new AtomParam(new Term("Y"))), false, false);

            UnificationResult res = this.unifier.Unify(lit1, lit2);

            Assert.IsTrue(res.IsSuccess);
            Assert.AreEqual(0, res.Value.Mappings.Count());
        }

        [Test]
        public void ShouldNotUnifyIfAnythingButVariableNamesAreDifferent()
        {
            Literal lit1 = new(new Atom("a", new AtomParam(new Term("X"))), false, false);
            Literal lit2 = new(new Atom("b", new AtomParam(new Term("X"))), false, false);

            UnificationResult res = this.unifier.Unify(lit1, lit2);

            Assert.IsFalse(res.IsSuccess);
        }

        [Test]
        public void ShouldUnifyOperationWithVariableNameDifferences()
        {
            Operation op1 = new(new AtomParam(new Term("X")), Operator.Equals, new AtomParam(new Literal(new Atom("a"), false, false)));
            Operation op2 = new(new AtomParam(new Term("Y")), Operator.Equals, new AtomParam(new Literal(new Atom("a"), false, false)));

            UnificationResult res = this.unifier.Unify(new BodyPart(null, op1), new BodyPart(null, op2));

            Assert.IsTrue(res.IsSuccess);
        }

        [Test]
        public void ShouldUnifyForAllWithMissmatchedVariableNames()
        {
            BodyPart bp1 = new(new Term("X"), new Literal(new Atom("a"), false, false));
            BodyPart bp2 = new(new Term("Y"), new Literal(new Atom("a"), false, false));

            UnificationResult res = this.unifier.Unify(bp1, bp2);

            Assert.IsTrue(res.IsSuccess);
        }
    }
}
