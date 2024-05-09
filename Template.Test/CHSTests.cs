using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Resolution.CallStackAndCHS;

namespace Template.Test;

[TestFixture]
public class CHSTests
{
    private CHS? _chs;

    [SetUp]
    public void Setup()
    {
        _chs = new CHS();
    }

    [Test]
    public void ShouldThrowWhenAddingMultiple()
    {
        var literal = new Literal(new Atom("human", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
        var literal2 = new Literal(new Atom("human", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);

        _chs?.Add(literal);
        Assert.Throws<ArgumentException>(() => _chs?.Add(literal2));
    }

    [Test]
    public void ShouldBuild()
    {
        var literal = new Literal(new Atom("human", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);
        var literal2 = new Literal(new Atom("informatiker", new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false);

        _chs?.Add(literal);
        _chs?.Add(literal2);
    }

    [Test]
    public void ShouldContainInRightOrder()
    {
        var names = new string[] { "human", "informatiker", "kuh", "esel", "elephant"};


        foreach (var name in names)
        {
            _chs?.Add(new Literal(new Atom(name, new AtomParam[] { new AtomParam(null, new Term("V")) }), false, false));
        }

        foreach(var name in names.Reverse()) // Reverse since CHS behaves like LIFO
        {
            Assert.AreEqual(name, _chs?.Pop().Atom.Name);
        }

        Assert.IsTrue(_chs?.Empty());
    }
}
