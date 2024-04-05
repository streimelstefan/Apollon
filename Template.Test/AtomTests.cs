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
    public class AtomTests
    {
        [Test]
        public void AtomShouldCorrecltyConvertIntoAString()
        {
            var atom = new Atom("likes", new Term[] { new Term("anna"), new Term("icecream") });

            Assert.AreEqual("likes(anna, icecream)", atom.ToString());
        }

        [Test]
        public void ShouldBeEqual()
        {
            var atom1 = new Atom("likes", new Term[] { new Term("anna"), new Term("icecream") });
            var atom2 = new Atom("likes", new Term[] { new Term("anna"), new Term("icecream") });

            Assert.IsTrue(atom1.Equals(atom2));
        }

        [Test]
        public void ShouldNotBeEqual()
        {
            var atom1 = new Atom("likes", new Term[] { new Term("anna"), new Term("icecream") });
            var atom2 = new Atom("like", new Term[] { new Term("anna"), new Term("icecream") });

            Assert.IsFalse(atom1.Equals(atom2));
        }
    }
}
