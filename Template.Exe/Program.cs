using Apollon.Lib;
using AppollonParser;
using Apollon.Lib.Graph;
using Apollon.Lib.OLON;
using Apollon.Lib.Rules;
using Apollon.Lib.NMRCheck;
using Apollon.Lib.DualRules;

var code = "q :- not p.\n" +   "p :- q, d.";
var query = "";

var parser = new ApollonParser();

var program = parser.ParseFromString(code);
var goals = parser.ParseQueryFromString(query);

var solver = new Solver();
solver.Load(program);

var result = solver.Solve(goals);

Console.WriteLine(result.CHS);
Console.WriteLine(result.Substitution);

