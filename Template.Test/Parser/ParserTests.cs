using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Test.Parser
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void SouldParse()
        {
            var input = "a()\nx() :- a().";

            var program = new AppollonParser.ApollonParser().ParseFromString(input);

            Assert.IsNotNull(program);
        }
    }
}
