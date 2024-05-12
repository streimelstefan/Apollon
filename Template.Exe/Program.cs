using Apollon.Lib;
using AppollonParser;
using Apollon.Lib.Graph;
using Apollon.Lib.OLON;
using Apollon.Lib.Rules;
using Apollon.Lib.NMRCheck;
using Apollon.Lib.DualRules;

var code = "a(X) :- b(X, Z), not c(Z, X).\n" + "c(X, b) :- a(X).";
var query = "true.";

var parser = new ApollonParser();

var program = parser.ParseFromString(code);
var goals = parser.ParseQueryFromString(query);

var solver = new Solver();
solver.Load(program);

var result = solver.Solve(goals);

Console.WriteLine(result.CHS);
Console.WriteLine(result.Substitution);

