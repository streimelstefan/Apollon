using Apollon.Lib;
using AppollonParser;
using Apollon.Lib.Graph;
using Apollon.Lib.OLON;
using Apollon.Lib.Rules;
using Apollon.Lib.NMRCheck;
using Apollon.Lib.DualRules;
using Apollon.Lib.Docu;

var query = "p(X), r(Y).";

var parser = new ApollonParser();

var program = parser.ParseFromFile("./Test.apo");
var goals = parser.ParseQueryFromString(query);

// var dokuGenerator = new DocumentationGenerator();
// 
// Console.WriteLine(dokuGenerator.GenerateDokumentationFor(program));

var solver = new Solver();
solver.Load(program);

var result = solver.Solve(goals);

var resultBuilder = new ResultStringBuilder();

Console.WriteLine(resultBuilder.CreateResultString(result));

