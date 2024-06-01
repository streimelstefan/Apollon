namespace Apollon.Test
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Docu;
    using Apollon.Lib.Rules;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void ShouldReturnEnumerableOfAllRulesAndConstraintsAsStatements()
        {
            Rule[] rules = new Rule[] { new(new Literal(new Atom("a"), false, false), new BodyPart[] { new(new Literal(new Atom("b"), false, false), null) }) };
            Constraint[] constraints = new Constraint[] { new(new Literal[] { new(new Atom("a"), false, false) }) };
            Literal[] literals = new Literal[0];
            Program program = new(literals, rules, constraints, new Documentation[0]);

            Statement[] ruleStatements = program.RuleTypesAsStatements.ToArray();

            Assert.AreEqual(2, ruleStatements.Length);
            Assert.IsTrue(rules[0].Equals(ruleStatements[0]));
            Assert.IsTrue(constraints[0].Equals(ruleStatements[1]));
        }

        [Test]
        public void ShouldReturnTheProgramAsAnEnumerableOfStatements()
        {
            Rule[] rules = new Rule[] { new(new Literal(new Atom("a"), false, false), new BodyPart[] { new(new Literal(new Atom("b"), false, false), null) }) };
            Constraint[] constraints = new Constraint[] { new(new Literal[] { new(new Atom("a"), false, false) }) };
            Literal[] literals = new Literal[] { new(new Atom("a"), false, false) };
            Program program = new(literals, rules, constraints, new Documentation[0]);

            Statement[] ruleStatements = program.Statements.ToArray();

            Assert.AreEqual(3, ruleStatements.Length);
            Assert.IsTrue(literals[0].Equals(ruleStatements[0].Head));
            Assert.IsTrue(rules[0].Equals(ruleStatements[1]));
            Assert.IsTrue(constraints[0].Equals(ruleStatements[2]));
        }
    }
}
