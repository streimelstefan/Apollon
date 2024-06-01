namespace Template.Test;
using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Resolution.CoSLD;
using NUnit.Framework;
using System;
using System.Linq;

[TestFixture]
public class CHSTests
{
    private CHS? chs;

    [SetUp]
    public void Setup()
    {
        this.chs = new CHS();
    }

    [Test]
    public void ShouldThrowWhenAddingMultiple()
    {
        Literal literal = new(new Atom("human", new AtomParam[] { new(null, new Term("V")) }), false, false);
        Literal literal2 = new(new Atom("human", new AtomParam[] { new(null, new Term("V")) }), false, false);

        this.chs?.Add(literal, new SubstitutionGroups());
        _ = Assert.Throws<ArgumentException>(() => this.chs?.Add(literal2, new SubstitutionGroups()));
    }

    [Test]
    public void ShouldBuild()
    {
        Literal literal = new(new Atom("human", new AtomParam[] { new(null, new Term("V")) }), false, false);
        Literal literal2 = new(new Atom("informatiker", new AtomParam[] { new(null, new Term("V")) }), false, false);

        this.chs?.Add(literal, new SubstitutionGroups());
        this.chs?.Add(literal2, new SubstitutionGroups());
    }

    [Test]
    public void ShouldContainInRightOrder()
    {
        string[] names = new string[] { "human", "informatiker", "kuh", "esel", "elephant" };

        foreach (string name in names)
        {
            this.chs?.Add(new Literal(new Atom(name, new AtomParam[] { new(null, new Term("V")) }), false, false), new SubstitutionGroups());
        }

        foreach (string? name in names.Reverse()) // Reverse since CHS behaves like LIFO
        {
            Assert.AreEqual(name, this.chs?.Pop().Atom.Name);
        }

        Assert.IsTrue(this.chs?.Empty());
    }
}
