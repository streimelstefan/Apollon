using Apollon.Lib.Docu;
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
    public class CodeToDocu
    {

        private ApollonParser _parser = new ApollonParser();
        private IDocumentationGenerator _docuGenerator = new DocumentationGenerator();

        [SetUp]
        public void Setup()
        {
            _parser = new ApollonParser();
            _docuGenerator = new DocumentationGenerator();
        }

        [Test]
        public void ShouldParseProgramWithDocumentation()
        {
            var code = "a(X) :: @(X) is awsome.";
            var program = _parser.ParseFromString(code);

            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.Documentation.Length);
        }

        [Test]
        public void ShouldThrowErrorIfDocuUsesUnknownVariable()
        {
            var code = "a(X) :: @(Y) is awsome.";
            
            Assert.Throws<ParseException>(() => _parser.ParseFromString(code));
        }

        [Test]
        public void ShouldCreateDokuForOperationProgram()
        {
            var code = "a(X) :: @(X) is awsome.\na(X) :- X = a.";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awsome if\r\n  X is a.\r\n", docu);
        }

        [Test]
        public void ShouldCreateDokuForAtom()
        {
            var code = "a(X) :: @(X) is awsome.\na(a).";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("a is awsome.\r\n", docu);
        }

        [Test]
        public void ShouldCreateDokuForRulesThatReferenceOtherRules()
        {
            var code = "a(X) :: @(X) is awsome.\nb(X) :: @(X) is great.\na(X) :- b(X).";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awsome if\r\n  X is great.\r\n", docu);
        }

        [Test]
        public void ShouldCreateDokuForRulesThatReferenceTwoOtherRules()
        {
            var code = "a(X) :: @(X) is awsome.\nb(X) :: @(X) is great.\na(X) :- b(X), b(a).";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awsome if\r\n  X is great, and\r\n  a is great.\r\n", docu);
        }

        [Test]
        public void ShouldCreateDokuForRecursiveRules()
        {
            var code = "edge(X,Y) :: @(X) and @(Y) are directly connected.\r\nedge(a,b).\r\nedge(b,c).\r\nedge(c,d).\r\n\r\npath(X,Y) :: there exists a path from @(X) to @(Y).\r\npath(X,Y) :- edge(X,Y).\r\npath(X,Y) :- edge(X,Z), path(Z,Y).";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("a and b are directly connected.\r\nb and c are directly connected.\r\nc and d are directly connected.\r\nthere exists a path from X to Y if\r\n  X and Y are directly connected.\r\nthere exists a path from X to Y if\r\n  X and Z are directly connected, and\r\n  there exists a path from Z to Y.\r\n", docu);
        }

        [Test]
        public void ShouldSayHoldsIfThereIsNoDokumentationForABodyLiteral()
        {
            var code = "other(0).\r\n\r\nanother(X) :: @(X) is awesome.\r\nanother(X) :- other(X).";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  other(X) holds.\r\n", docu);
        }

        [Test]
        public void ShouldSayIfThereIsNoEvidenceOfInCaseOfNAF()
        {
            var code = "another(X) :: @(X) is awesome.\r\nanother(X) :- other(X).\r\n\r\nsomeNot(ZZ) :: @(ZZ) is stupid.\r\nsomeNot(ZZ) :- not another(ZZ).";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  other(X) holds.\r\nZZ is stupid if\r\n  there is no evidence that ZZ is awesome.\r\n", docu);
        }

        [Test]
        public void ShouldSayIfItIsNotTheCaseThatWhenNegativeLiteral()
        {
            var code = "someNeg(Y) :: @(Y) makes sense.\r\nsomeNeg(Y) :- -nat(Y).";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("Y makes sense if\r\n  it is not the case that nat(Y) holds.\r\n", docu);
        }

        [Test]
        public void ShouldCombineNafAndNegationDescriptions()
        {
            var code = "someNeg(Y) :: @(Y) makes sense.\r\nsomeNeg(Y) :- not -nat(Y).";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("Y makes sense if\r\n  there is no evidence that and it is not the case that nat(Y) holds.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForLessThenOperation()
        {
            var code = "a(X) :: @(X) is awesome.\r\na(X) :- X < 1";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  X is less than 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForGreaterThenOperation()
        {
            var code = "a(X) :: @(X) is awesome.\r\na(X) :- X > 1";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  X is greater than 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForGreaterThenOrEqualToOperation()
        {
            var code = "a(X) :: @(X) is awesome.\r\na(X) :- X >= 1";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  X is greater than or equal to 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForLessThenOrEqualToOperation()
        {
            var code = "a(X) :: @(X) is awesome.\r\na(X) :- X <= 1";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  X is less than or equal to 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForPlusOperation()
        {
            var code = "a(X) :: @(X) is awesome.\r\na(X) :- Y is X + 1";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  Y is X plus 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForTimesOperation()
        {
            var code = "a(X) :: @(X) is awesome.\r\na(X) :- Y is X * 1";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  Y is X times 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForDivideOperation()
        {
            var code = "a(X) :: @(X) is awesome.\r\na(X) :- Y is X / 1";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  Y is X divided by 1.\r\n", docu);
        }

        [Test]
        public void ShouldGenerateDocuForMinusOperation()
        {
            var code = "a(X) :: @(X) is awesome.\r\na(X) :- Y is X - 1";
            var program = _parser.ParseFromString(code);

            var docu = _docuGenerator.GenerateDokumentationFor(program);

            Assert.IsNotNull(docu);
            Assert.AreEqual("X is awesome if\r\n  Y is X minus 1.\r\n", docu);
        }
    }
}
