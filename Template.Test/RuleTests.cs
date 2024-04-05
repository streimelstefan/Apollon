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
    public class RuleTests
    {
        [Test]
        public void ShouldCorrectlyConvertToString()
        {
            var atom = new Atom("reached", new Term[] { new Term("V") });
            var head = new Literal(atom, false, false);
            var body1 = new Literal(atom, false, true);
            var body2 = new Literal(atom, true, true);

            var rule = new Rule(head, body1, body2 );

            Assert.AreEqual("reached(V) :- -reached(V), not -reached(V)", rule.ToString());
        }

        [Test]
        public void ShouldThrowIfTheHeadIsNAFNegated()
        {
            var atom = new Atom("reached", new Term[] { new Term("V") });
            var head = new Literal(atom, true, false);
            var body1 = new Literal(atom, false, true);
            var body2 = new Literal(atom, true, true);

            Assert.Throws<ArgumentException>(() => new Rule(head, body1, body2));
        }

        [Test]
        public void ShouldBeEqual()
        {
            var atom1 = new Atom("reached", new Term[] { new Term("V") });
            var head1 = new Literal(atom1, false, false);
            var body1 = new Literal(atom1, false, true);

            var rule1 = new Rule(head1, body1);


            var atom2 = new Atom("reached", new Term[] { new Term("V") });
            var head2 = new Literal(atom2, false, false);
            var body2 = new Literal(atom2, false, true);
            var rule2 = new Rule(head2, body2);

            Assert.IsTrue(rule1.Equals(rule2));
        }

        [Test]
        public void ShouldNotBeEqual()
        {
            var atom1 = new Atom("reached", new Term[] { new Term("V") });
            var head1 = new Literal(atom1, false, false);
            var body1 = new Literal(atom1, false, true);

            var rule1 = new Rule(head1, body1);


            var atom2 = new Atom("reached", new Term[] { new Term("V") });
            var head2 = new Literal(atom2, false, false);
            var body2 = new Literal(atom2, true, true);
            var rule2 = new Rule(head2, body2);

            Assert.IsFalse(rule1.Equals(rule2));
        }

    }
}
