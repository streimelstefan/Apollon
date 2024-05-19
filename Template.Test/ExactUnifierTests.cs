using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Test
{
    [TestFixture]
    public class ExactUnifierTests
    {

        private IUnifier _unifier = new ExactUnifier();

        [SetUp]
        public void Setup()
        {
            _unifier = new ExactUnifier();
        }

        [Test]
        public void ShouldUnifyIfTheyAreExactlyTheSame()
        {
            var lit1 = new Literal(new Atom("a", new AtomParam(new Term("X"))), false, false);
            var lit2 = new Literal(new Atom("a", new AtomParam(new Term("X"))), false, false);
            
            var res = _unifier.Unify(lit1, lit2);

            Assert.IsTrue(res.IsSuccess);
            Assert.AreEqual(0, res.Value.Mappings.Count());
        }

        [Test]
        public void ShouldUnifyIfOnlyVariableNamesAreDifferent()
        {
            var lit1 = new Literal(new Atom("a", new AtomParam(new Term("X"))), false, false);
            var lit2 = new Literal(new Atom("a", new AtomParam(new Term("Y"))), false, false);

            var res = _unifier.Unify(lit1, lit2);

            Assert.IsTrue(res.IsSuccess);
            Assert.AreEqual(0, res.Value.Mappings.Count());
        }

        [Test]
        public void ShouldNotUnifyIfAnythingButVariableNamesAreDifferent()
        {
            var lit1 = new Literal(new Atom("a", new AtomParam(new Term("X"))), false, false);
            var lit2 = new Literal(new Atom("b", new AtomParam(new Term("X"))), false, false);

            var res = _unifier.Unify(lit1, lit2);

            Assert.IsFalse(res.IsSuccess);
        }

        [Test]
        public void ShouldUnifyOperationWithVariableNameDifferences()
        {
            var op1 = new Operation(new AtomParam(new Term("X")), Operator.Equals, new Literal(new Atom("a"), false, false));
            var op2 = new Operation(new AtomParam(new Term("Y")), Operator.Equals, new Literal(new Atom("a"), false, false));

            var res = _unifier.Unify(new BodyPart(null, op1), new BodyPart(null, op2));

            Assert.IsTrue(res.IsSuccess);
        }

        [Test]
        public void ShouldUnifyForAllWithMissmatchedVariableNames()
        {
            var bp1 = new BodyPart(new Term("X"), new Literal(new Atom("a"), false, false));
            var bp2 = new BodyPart(new Term("Y"), new Literal(new Atom("a"), false, false));

            var res = _unifier.Unify(bp1, bp2);

            Assert.IsTrue(res.IsSuccess);
        }
    }
}
