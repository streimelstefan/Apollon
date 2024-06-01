namespace Template.Test
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using NUnit.Framework;

    [TestFixture]
    public class LiteralTests
    {
        [Test]
        public void LiteralShouldCorrectlyConvertIntoString()
        {
            Atom atom = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal literal = new(atom, false, false);

            Assert.AreEqual("reached(V)", literal.ToString());
        }

        [Test]
        public void LiteralShouldCorrectlyConvertIntoStringWhenUsingNAF()
        {
            Atom atom = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal literal = new(atom, true, false);

            Assert.AreEqual("not reached(V)", literal.ToString());
        }

        [Test]
        public void LiteralShouldCorrectlyConvertIntoStringWhenUsingNegation()
        {
            Atom atom = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal literal = new(atom, false, true);

            Assert.AreEqual("-reached(V)", literal.ToString());
        }

        [Test]
        public void LiteralShouldCorrectlyConvertIntoStringWhenUsingNAFandNegation()
        {
            Atom atom = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal literal = new(atom, true, true);

            Assert.AreEqual("not -reached(V)", literal.ToString());
        }

        [Test]
        public void ShouldBeEqual()
        {
            Atom atom1 = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Atom atom2 = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal literal1 = new(atom1, true, true);
            Literal literal2 = new(atom2, true, true);

            Assert.IsTrue(literal1.Equals(literal2));
        }

        [Test]
        public void ShouldNotBeEqual()
        {
            Atom atom1 = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Atom atom2 = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal literal1 = new(atom1, true, true);
            Literal literal2 = new(atom2, true, false);

            Assert.IsFalse(literal1.Equals(literal2));
        }
    }
}
