using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;

namespace Template.Test
{
    [TestFixture]
    public class LiteralTests
    {
        [Test]
        public void LiteralShouldCorrectlyConvertIntoString()
        {
            var atom = new Atom("reached", new Term[] { new Term("V") });
            var literal = new Literal(atom, false, false);

            Assert.AreEqual("reached(V)", literal.ToString());
        }

        [Test]
        public void LiteralShouldCorrectlyConvertIntoStringWhenUsingNAF()
        {
            var atom = new Atom("reached", new Term[] { new Term("V") });
            var literal = new Literal(atom, true, false);

            Assert.AreEqual("not reached(V)", literal.ToString());
        }

        [Test]
        public void LiteralShouldCorrectlyConvertIntoStringWhenUsingNegation()
        {
            var atom = new Atom("reached", new Term[] { new Term("V") });
            var literal = new Literal(atom, false, true);

            Assert.AreEqual("-reached(V)", literal.ToString());
        }

        [Test]
        public void LiteralShouldCorrectlyConvertIntoStringWhenUsingNAFandNegation()
        {
            var atom = new Atom("reached", new Term[] { new Term("V") });
            var literal = new Literal(atom, true, true);

            Assert.AreEqual("not -reached(V)", literal.ToString());
        }

        [Test]
        public void ShouldBeEqual()
        {
            var atom1 = new Atom("reached", new Term[] { new Term("V") });
            var atom2 = new Atom("reached", new Term[] { new Term("V") });
            var literal1 = new Literal(atom1, true, true);
            var literal2 = new Literal(atom2, true, true);

            Assert.IsTrue(literal1.Equals(literal2));
        }

        [Test]
        public void ShouldNotBeEqual()
        {
            var atom1 = new Atom("reached", new Term[] { new Term("V") });
            var atom2 = new Atom("reached", new Term[] { new Term("V") });
            var literal1 = new Literal(atom1, true, true);
            var literal2 = new Literal(atom2, true, false);

            Assert.IsFalse(literal1.Equals(literal2));
        }
    }
}
