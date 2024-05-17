using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Docu;
using Apollon.Lib.Rules;
using NUnit.Framework;
using System.Linq;

namespace Apollon.Test
{
    [TestFixture]
    public class ProgramTests
    {

        [Test]
        public void ShouldReturnEnumerableOfAllRulesAndConstraintsAsStatements()
        {
            var rules = new Rule[] { new Rule(new Literal(new Atom("a"), false, false), new BodyPart[] { new BodyPart(new Literal(new Atom("b"), false, false), null) }) };
            var constraints = new Constraint[] { new Constraint(new Literal[] { new Literal(new Atom("a"), false, false) }) };
            var literals = new Literal[0];
            var program = new Program(literals, rules, constraints, new Documentation[0]);

            var ruleStatements = program.RuleTypesAsStatements.ToArray();

            Assert.AreEqual(2, ruleStatements.Length);
            Assert.IsTrue(rules[0].Equals(ruleStatements[0]));
            Assert.IsTrue(constraints[0].Equals(ruleStatements[1]));
        }

        [Test]
        public void ShouldReturnTheProgramAsAnEnumerableOfStatements()
        {
            var rules = new Rule[] { new Rule(new Literal(new Atom("a"), false, false), new BodyPart[] { new BodyPart(new Literal(new Atom("b"), false, false), null) }) };
            var constraints = new Constraint[] { new Constraint(new Literal[] { new Literal(new Atom("a"), false, false) }) };
            var literals = new Literal[] {new Literal(new Atom("a"), false, false)};
            var program = new Program(literals, rules, constraints, new Documentation[0]);

            var ruleStatements = program.Statements.ToArray();

            Assert.AreEqual(3, ruleStatements.Length);
            Assert.IsTrue(literals[0].Equals(ruleStatements[0].Head)); 
            Assert.IsTrue(rules[0].Equals(ruleStatements[1]));
            Assert.IsTrue(constraints[0].Equals(ruleStatements[2]));
        }

    }
}
