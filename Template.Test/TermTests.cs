namespace Template.Test
{
    using Apollon.Lib;
    using NUnit.Framework;

    [TestFixture]
    public class TermTests
    {
        [Test]
        public void ShouldDetectThatItIsAVariable()
        {
            Term term = new("A");

            Assert.IsTrue(term.IsVariable);
        }

        [Test]
        public void ShouldDetectThatItIsNotAVariable()
        {
            Term term = new("aBN");

            Assert.IsFalse(term.IsVariable);
        }

        [Test]
        public void ShouldBeEqual()
        {
            Term term1 = new("aBN");
            Term term2 = new("aBN");

            Assert.IsTrue(term1.Equals(term2));
        }

        [Test]
        public void ShouldNotBeEqual()
        {
            Term term1 = new("aBN");
            Term term2 = new("aBn");

            Assert.IsFalse(term1.Equals(term2));
        }
    }
}
