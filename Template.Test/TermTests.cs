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
    public class TermTests
    {

        [Test]
        public void ShouldDetectThatItIsAVariable()
        {
            var term = new Term("A");

            Assert.IsTrue(term.IsVariable);
        }

        [Test]
        public void ShouldDetectThatItIsNotAVariable()
        {
            var term = new Term("aBN");

            Assert.IsFalse(term.IsVariable);
        }

        [Test]
        public void ShouldBeEqual()
        {
            var term1 = new Term("aBN");
            var term2 = new Term("aBN");

            Assert.IsTrue(term1.Equals(term2));
        }

        [Test]
        public void ShouldNotBeEqual()
        {
            var term1 = new Term("aBN");
            var term2 = new Term("aBn");

            Assert.IsFalse(term1.Equals(term2));
        }

    }
}
