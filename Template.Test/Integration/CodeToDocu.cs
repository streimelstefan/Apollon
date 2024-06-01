namespace Apollon.Test.Integration
{
    using Apollon.Lib.Docu;
    using AppollonParser;
    using NUnit.Framework;

    [TestFixture]
    public class CodeToDocu
    {
        private ApollonParser parser = new();
        private IDocumentationGenerator docuGenerator = new DocumentationGenerator();

        [SetUp]
        public void Setup()
        {
            this.parser = new ApollonParser();
            this.docuGenerator = new DocumentationGenerator();
        }

        [Test]
        public void ShouldParseProgramWithDocumentation()
        {
            string code = "a(X) :: @(X) is awsome.";
            Lib.Program program = this.parser.ParseFromString(code);

            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.Documentation.Length);
        }

        [Test]
        public void ShouldThrowErrorIfDocuUsesUnknownVariable()
        {
            string code = "a(X) :: @(Y) is awsome.";

            _ = Assert.Throws<ParseException>(() => this.parser.ParseFromString(code));
        }

        [Test]
        public void ShouldCreateDokuForOperationProgram()
        {
            string code = "a(X) :: @(X) is awsome.\na(X) :- X = a.";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awsome if\r\n  X is a.\r\n", docu);
        }

        [Test]
        public void ShouldCreateDokuForAtom()
        {
            string code = "a(X) :: @(X) is awsome.\na(a).";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("a is awsome.\r\n", docu);
        }

        [Test]
        public void ShouldCreateDokuForRulesThatReferenceOtherRules()
        {
            string code = "a(X) :: @(X) is awsome.\nb(X) :: @(X) is great.\na(X) :- b(X).";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awsome if\r\n  X is great.\r\n", docu);
        }

        [Test]
        public void ShouldCreateDokuForRulesThatReferenceTwoOtherRules()
        {
            string code = "a(X) :: @(X) is awsome.\nb(X) :: @(X) is great.\na(X) :- b(X), b(a).";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awsome if\r\n  X is great, and\r\n  a is great.\r\n", docu);
        }

        [Test]
        public void ShouldCreateDokuForRecursiveRules()
        {
            string code = "edge(X,Y) :: @(X) and @(Y) are directly connected.\r\nedge(a,b).\r\nedge(b,c).\r\nedge(c,d).\r\n\r\npath(X,Y) :: there exists a path from @(X) to @(Y).\r\npath(X,Y) :- edge(X,Y).\r\npath(X,Y) :- edge(X,Z), path(Z,Y).";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("a and b are directly connected.\r\nb and c are directly connected.\r\nc and d are directly connected.\r\nthere exists a path from X to Y if\r\n  X and Y are directly connected.\r\nthere exists a path from X to Y if\r\n  X and Z are directly connected, and\r\n  there exists a path from Z to Y.\r\n", docu);
        }

        [Test]
        public void ShouldSayHoldsIfThereIsNoDokumentationForABodyLiteral()
        {
            string code = "other(0).\r\n\r\nanother(X) :: @(X) is awesome.\r\nanother(X) :- other(X).";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  other(X) holds.\r\n", docu);
        }

        [Test]
        public void ShouldSayIfThereIsNoEvidenceOfInCaseOfNAF()
        {
            string code = "another(X) :: @(X) is awesome.\r\nanother(X) :- other(X).\r\n\r\nsomeNot(ZZ) :: @(ZZ) is stupid.\r\nsomeNot(ZZ) :- not another(ZZ).";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  other(X) holds.\r\nZZ is stupid if\r\n  there is no evidence that ZZ is awesome.\r\n", docu);
        }

        [Test]
        public void ShouldSayIfItIsNotTheCaseThatWhenNegativeLiteral()
        {
            string code = "someNeg(Y) :: @(Y) makes sense.\r\nsomeNeg(Y) :- -nat(Y).";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("Y makes sense if\r\n  it is not the case that nat(Y) holds.\r\n", docu);
        }

        [Test]
        public void ShouldCombineNafAndNegationDescriptions()
        {
            string code = "someNeg(Y) :: @(Y) makes sense.\r\nsomeNeg(Y) :- not -nat(Y).";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("Y makes sense if\r\n  there is no evidence that and it is not the case that nat(Y) holds.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForLessThenOperation()
        {
            string code = "a(X) :: @(X) is awesome.\r\na(X) :- X < 1";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  X is less than 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForGreaterThenOperation()
        {
            string code = "a(X) :: @(X) is awesome.\r\na(X) :- X > 1";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  X is greater than 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForGreaterThenOrEqualToOperation()
        {
            string code = "a(X) :: @(X) is awesome.\r\na(X) :- X >= 1";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  X is greater than or equal to 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForLessThenOrEqualToOperation()
        {
            string code = "a(X) :: @(X) is awesome.\r\na(X) :- X <= 1";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  X is less than or equal to 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForPlusOperation()
        {
            string code = "a(X) :: @(X) is awesome.\r\na(X) :- Y is X + 1";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  Y is X plus 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForTimesOperation()
        {
            string code = "a(X) :: @(X) is awesome.\r\na(X) :- Y is X * 1";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  Y is X times 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForDivideOperation()
        {
            string code = "a(X) :: @(X) is awesome.\r\na(X) :- Y is X / 1";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  Y is X divided by 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForMinusOperation()
        {
            string code = "a(X) :: @(X) is awesome.\r\na(X) :- Y is X - 1";
            Lib.Program program = this.parser.ParseFromString(code);

            string docu = this.docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  Y is X minus 1.\r\n", docu);
        }
    }
}
