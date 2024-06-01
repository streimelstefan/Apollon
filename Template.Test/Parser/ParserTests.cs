namespace Template.Test.Parser
{
    using AppollonParser;
    using NUnit.Framework;

    [TestFixture]
    public class ParserTests
    {
        private ApollonParser parser = new();

        [SetUp]
        public void SetUp()
        {
            this.parser = new ApollonParser();
        }

        [Test]
        public void SouldParseAndReturnAProgramThatIsNotNull()
        {
            string input = string.Empty;

            Apollon.Lib.Program program = this.parser.ParseFromString(input);

            Assert.IsNotNull(program);
        }

        [Test]
        public void ShouldParseTwoFacts()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/BasicFacts.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(2, program.LiteralList.Length);
            Assert.IsFalse(program.LiteralList[0].IsNAF);
            Assert.IsFalse(program.LiteralList[0].IsNegative);
            Assert.AreEqual("cat", program.LiteralList[0].Atom.Name);
            Assert.AreEqual(0, program.LiteralList[0].Atom.ParamList.Length);

            Assert.IsFalse(program.LiteralList[1].IsNAF);
            Assert.IsFalse(program.LiteralList[1].IsNegative);
            Assert.AreEqual("dog", program.LiteralList[1].Atom.Name);
            Assert.AreEqual(0, program.LiteralList[1].Atom.ParamList.Length);
        }

        [Test]
        public void ShouldParseABasicRule()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/BasicRule.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.LiteralList.Length);
            Assert.AreEqual(1, program.RuleList.Length);

            Assert.AreEqual("bird", program.RuleList[0].Head.Atom.Name);
            Assert.AreEqual("cat", program.RuleList[0].Body[0].Literal.Atom.Name);
            Assert.AreEqual("dog", program.RuleList[0].Body[1].Literal.Atom.Name);
        }

        [Test]
        public void ShouldParseWithComments()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/CommentsFactsAndRules.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.LiteralList.Length);
            Assert.AreEqual(1, program.RuleList.Length);

            Assert.AreEqual("bird", program.RuleList[0].Head.Atom.Name);
            Assert.AreEqual("worm", program.RuleList[0].Body[0].Literal.Atom.Name);

            Assert.AreEqual("fish", program.LiteralList[0].Atom.Name);
        }

        [Test]
        public void ShouldParseNAFAndNegationCorrectly()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/NafAndNegation.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(2, program.LiteralList.Length);
            Assert.AreEqual(1, program.RuleList.Length);

            Assert.AreEqual("dog", program.RuleList[0].Head.Atom.Name);
            Assert.AreEqual("cat", program.RuleList[0].Body[0].Literal.Atom.Name);
            Assert.IsFalse(program.RuleList[0].Body[0].Literal.IsNegative);
            Assert.IsTrue(program.RuleList[0].Body[0].Literal.IsNAF);

            Assert.AreEqual("cat", program.LiteralList[0].Atom.Name);
            Assert.IsFalse(program.LiteralList[0].IsNegative);
            Assert.IsFalse(program.LiteralList[0].IsNAF);
            Assert.AreEqual("dog", program.LiteralList[1].Atom.Name);
            Assert.IsTrue(program.LiteralList[1].IsNegative);
            Assert.IsFalse(program.LiteralList[1].IsNAF);
        }

        [Test]
        public void ShouldParseRulesWithVariables()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/RulesWithVariablesAndTerms.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.LiteralList.Length);
            Assert.AreEqual(1, program.RuleList.Length);

            Assert.AreEqual("happy", program.RuleList[0].Head.Atom.Name);
            Assert.AreEqual(1, program.RuleList[0].Head.Atom.ParamList.Length);
            Assert.IsTrue(program.RuleList[0].Head.Atom.ParamList[0].IsTerm);
            Assert.AreEqual("X", program.RuleList[0].Head.Atom.ParamList[0].Term.Value);
            Assert.IsTrue(program.RuleList[0].Head.Atom.ParamList[0].Term.IsVariable);

            Assert.AreEqual("likes", program.RuleList[0].Body[0].Literal.Atom.Name);
            Assert.AreEqual(2, program.RuleList[0].Body[0].Literal.Atom.ParamList.Length);
            Assert.AreEqual("X", program.RuleList[0].Body[0].Literal.Atom.ParamList[0].Term.Value);
            Assert.IsTrue(program.RuleList[0].Body[0].Literal.Atom.ParamList[0].Term.IsVariable);
            Assert.AreEqual("prix", program.RuleList[0].Body[0].Literal.Atom.ParamList[1].Term.Value);
            Assert.IsFalse(program.RuleList[0].Body[0].Literal.Atom.ParamList[1].Term.IsVariable);

            Assert.AreEqual("hates", program.RuleList[0].Body[1].Literal.Atom.Name);
            Assert.AreEqual(2, program.RuleList[0].Body[1].Literal.Atom.ParamList.Length);
            Assert.AreEqual("X", program.RuleList[0].Body[1].Literal.Atom.ParamList[0].Term.Value);
            Assert.IsTrue(program.RuleList[0].Body[1].Literal.Atom.ParamList[0].Term.IsVariable);
            Assert.AreEqual("stefan", program.RuleList[0].Body[1].Literal.Atom.ParamList[1].Term.Value);
            Assert.IsFalse(program.RuleList[0].Body[1].Literal.Atom.ParamList[1].Term.IsVariable);
        }

        [Test]
        public void ShouldParseAtomWithTerms()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/AtomWithTerms.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.LiteralList.Length);

            Assert.AreEqual("likes", program.LiteralList[0].Atom.Name);
            Assert.AreEqual(2, program.LiteralList[0].Atom.ParamList.Length);
            Assert.AreEqual("X", program.LiteralList[0].Atom.ParamList[0].Term.Value);
            Assert.IsTrue(program.LiteralList[0].Atom.ParamList[0].Term.IsVariable);
            Assert.AreEqual("prix", program.LiteralList[0].Atom.ParamList[1].Term.Value);
        }

        [Test]
        public void ShouldParseAtomWithConstraintRule()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/AtomWithConstraintRule.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.LiteralList.Length);
            Assert.AreEqual(0, program.RuleList.Length);
            Assert.AreEqual(1, program.ConstraintList.Length);

            Assert.AreEqual("a", program.LiteralList[0].Atom.Name);

            Assert.AreEqual(1, program.ConstraintList[0].Body.Length);
            Assert.AreEqual("b", program.ConstraintList[0].Body[0].Literal.Atom.Name);
        }

        [Test]
        public void ShouldParseAtomWithConstraintRuleAndNormalRule()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/OLONRuleByConstraintRule.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.LiteralList.Length);
            Assert.AreEqual(1, program.RuleList.Length);
            Assert.AreEqual(1, program.ConstraintList.Length);

            Assert.IsNotNull(program.RuleList[0].Head);
            Assert.AreEqual("c", program.RuleList[0].Head?.Atom.Name);

            Assert.AreEqual(1, program.ConstraintList[0].Body.Length);
            Assert.AreEqual("b", program.ConstraintList[0].Body[0].Literal.Atom.Name);
        }

        [Test]
        public void ShouldParseNestedAtomsCorrectly()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/AtomsWithStructures.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.LiteralList.Length);
            Assert.AreEqual(0, program.RuleList.Length);
            Assert.AreEqual(0, program.ConstraintList.Length);

            Apollon.Lib.Literal literal = program.LiteralList[0];
            Assert.AreEqual("outer", literal.Atom.Name);

            Assert.AreEqual(2, literal.Atom.ParamList.Length);

            Apollon.Lib.Atoms.AtomParam innerLiteralParam = literal.Atom.ParamList[0];
            Assert.IsTrue(innerLiteralParam.IsLiteral);

            Apollon.Lib.Literal? innerAtom = innerLiteralParam.Literal;
            Assert.AreEqual("inner", innerAtom.Atom.Name);
            Assert.AreEqual(2, innerAtom.Atom.ParamList.Length);
            Assert.IsTrue(innerAtom.Atom.ParamList[0].IsTerm);
            Assert.AreEqual("inner_param", innerAtom.Atom.ParamList[0].Term.Value);
            Assert.IsTrue(innerAtom.Atom.ParamList[1].IsTerm);
            Assert.AreEqual("inner_param2", innerAtom.Atom.ParamList[1].Term.Value);

            Assert.IsTrue(literal.Atom.ParamList[1].IsTerm);
            Assert.AreEqual("outer_param", literal.Atom.ParamList[1].Term.Value);
        }

        [Test]
        public void ShouldParseRulesWithOperationsCorrectly()
        {
            Apollon.Lib.Program program = this.parser.ParseFromFile("../../../TestPrograms/RulesWithOperation.apo");

            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.LiteralList.Length);
            Assert.AreEqual(2, program.RuleList.Length);
            Assert.AreEqual(0, program.ConstraintList.Length);
        }

        [Test]
        public void ShouldParseQuery()
        {
            Apollon.Lib.Rules.BodyPart[] query = this.parser.ParseQueryFromString("a(X).");

            Assert.IsNotNull(query);
            Assert.AreEqual(1, query.Length);
            Assert.AreEqual("a(X)", query[0].ToString());
        }

        [Test]
        public void ShouldParseOperation()
        {
            Apollon.Lib.Rules.BodyPart[] query = this.parser.ParseQueryFromString("X = a.");

            Assert.IsNotNull(query);
            Assert.AreEqual("X = a", query[0].ToString());
        }

        [Test]
        public void ShouldParseForAllAsLiteral()
        {
            Apollon.Lib.Rules.BodyPart[] query = this.parser.ParseQueryFromString("forall(X, b(X)).");

            Assert.IsNotNull(query);
            Assert.AreEqual(1, query.Length);
            Assert.IsFalse(query[0].IsForAll);
            Assert.IsTrue(query[0].IsLiteral);
        }

        [Test]
        public void ShouldParseMultiGoalQuery()
        {
            Apollon.Lib.Rules.BodyPart[] query = this.parser.ParseQueryFromString("a(X), b(a).");

            Assert.IsNotNull(query);
            Assert.AreEqual(2, query.Length);
            Assert.AreEqual("a(X)", query[0].ToString());
            Assert.AreEqual("b(a)", query[1].ToString());
        }

        [Test]
        public void ShouldParseGoalWithNestedLiteral()
        {
            Apollon.Lib.Rules.BodyPart[] query = this.parser.ParseQueryFromString("a(c(X)).");

            Assert.IsNotNull(query);
            Assert.AreEqual(1, query.Length);
            Assert.AreEqual("a(c(X))", query[0].ToString());
        }

        [Test]
        public void ShouldParseQueryWithOperationsAndLiterals()
        {
            Apollon.Lib.Rules.BodyPart[] query = this.parser.ParseQueryFromString("a(X), X = a.");

            Assert.IsNotNull(query);
            Assert.AreEqual(2, query.Length);
            Assert.AreEqual("a(X)", query[0].ToString());
            Assert.AreEqual("X = a", query[1].ToString());
        }
    }
}
