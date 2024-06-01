namespace Template.Test
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using NUnit.Framework;

    [TestFixture]
    public class AtomTests
    {
        [Test]
        public void AtomShouldCorrecltyConvertIntoAString()
        {
            Atom atom = new("likes", new AtomParam[] { new(null, new Term("anna")), new(null, new Term("icecream")) });

            Assert.AreEqual("likes(anna, icecream)", atom.ToString());
        }

        [Test]
        public void ShouldBeEqual()
        {
            Atom atom1 = new("likes", new AtomParam[] { new(null, new Term("anna")), new(null, new Term("icecream")) });
            Atom atom2 = new("likes", new AtomParam[] { new(null, new Term("anna")), new(null, new Term("icecream")) });
            Assert.IsTrue(atom1.Equals(atom2));
        }

        [Test]
        public void ShouldNotBeEqual()
        {
            Atom atom1 = new("likes", new AtomParam[] { new(null, new Term("anna")), new(null, new Term("icecream")) });
            Atom atom2 = new("like", new AtomParam[] { new(null, new Term("anna")), new(null, new Term("icecream")) });

            Assert.IsFalse(atom1.Equals(atom2));
        }
    }
}
