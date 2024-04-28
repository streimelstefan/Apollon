using Apollon.Lib.DualRules;
using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Test.Integration
{
    [TestFixture]
    public class ParserToDualRuleTests
    {

        private ApollonParser parser = new ApollonParser();
        private IDualRuleGenerator generator = new DualRuleGenerator();

        [SetUp]
        public void Setup()
        {
            parser = new ApollonParser();
            generator = new DualRuleGenerator();
        }

        [Test]
        public void ShouldUnuninifyAtoms()
        {
            var code = "a(0).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());



        }

        [Test]
        public void SchouldCreateTwoDualRulesForThisRule()
        {
            var code = "p(X, b) :- q(X).";
            var program = parser.ParseFromString(code);

            var rules = generator.GenerateDualRules(program.Statements.ToArray());



        }

    }
}
