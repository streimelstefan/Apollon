namespace Template.Test
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class RuleTests
    {
        [Test]
        public void ShouldCorrectlyConvertToString()
        {
            Atom atom = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal head = new(atom, false, false);
            BodyPart body1 = new(new Literal(atom, false, true), null);
            BodyPart body2 = new(new Literal(atom, true, true), null);

            Rule rule = new(head, body1, body2);

            Assert.AreEqual("reached(V) :- -reached(V), not -reached(V).", rule.ToString());
        }

        [Test]
        public void ShouldThrowIfTheHeadIsNAFNegated()
        {
            Atom atom = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal head = new(atom, true, false);
            BodyPart body1 = new(new Literal(atom, false, true), null);
            BodyPart body2 = new(new Literal(atom, true, true), null);

            _ = Assert.Throws<ArgumentException>(() => new Rule(head, body1, body2));
        }

        [Test]
        public void ShouldBeEqual()
        {
            Atom atom1 = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal head1 = new(atom1, false, false);
            BodyPart body1 = new(new Literal(atom1, false, true), null);

            Rule rule1 = new(head1, body1);

            Atom atom2 = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal head2 = new(atom2, false, false);
            BodyPart body2 = new(new Literal(atom2, false, true), null);
            Rule rule2 = new(head2, body2);

            Assert.IsTrue(rule1.Equals(rule2));
        }

        [Test]
        public void ShouldNotBeEqual()
        {
            Atom atom1 = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal head1 = new(atom1, false, false);
            BodyPart body1 = new(new Literal(atom1, false, true), null);

            Rule rule1 = new(head1, body1);

            Atom atom2 = new("reached", new AtomParam[] { new(null, new Term("V")) });
            Literal head2 = new(atom2, false, false);
            BodyPart body2 = new(new Literal(atom2, true, true), null);
            Rule rule2 = new(head2, body2);

            Assert.IsFalse(rule1.Equals(rule2));
        }
    }
}
