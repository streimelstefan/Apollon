using Apollon.Lib;
using AppollonParser;
using Apollon.Lib.Graph;
using Apollon.Lib.OLON;
using Apollon.Lib.Rules;

var code = "q :- not p.\n" +
    "p :- q, d.";

var parser = new ApollonParser();
var program = parser.ParseFromString(code);

// normal code would be:
// Solver.Solve(program);
// but we would not be able to see what happens so we do manually here what the solver does internally


var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

Console.WriteLine("Call Graph: ");
foreach (var edge in callGraph.Edges)
{
    Console.WriteLine($"({edge?.Source.Literal}) -{(edge.IsNAF ? "*" : "")}> ({edge.Target.Literal}) Created by: {edge.CreatorRule}");
}

var olons = OlonDetector.DetectOlonIn(callGraph);

Console.WriteLine();
Console.WriteLine("OLON Nodes: ");
foreach (var olon in olons.Nodes)
{
    Console.WriteLine($"({olon.Literal})");
}

var rulePreprocessor = new RuleMetadataSetter(callGraph, olons);
var processedRules = rulePreprocessor.SetMetadataOn(program.RuleList);

Console.WriteLine();
Console.WriteLine("Processed Rules:");
foreach (var rule in processedRules)
{
    Console.WriteLine($"OLON Rule: {(rule.IsOlonRule ? "true" : "false")} Ordinary Rule: {(rule.IsOrdiniaryRule ? "true" : "false")} || {rule}");
}

