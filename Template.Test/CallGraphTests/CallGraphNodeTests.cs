using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;
using Template.Lib.CallGraph;

namespace Template.Test;

[TestFixture]
public class CallGraphNodeTests
{
    [Test]
    public void ShouldThrowIfLiteralIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new CallGraphNode(null));
    }
}

