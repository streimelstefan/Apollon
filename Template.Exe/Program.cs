using System.CommandLine;
using Apollon.Lib;
using AppollonParser;
using Apollon.Lib.Graph;
using Apollon.Lib.OLON;
using Apollon.Lib.Rules;
using Apollon.Lib.NMRCheck;
using Apollon.Lib.DualRules;
using Apollon.Lib.Docu;
using System.CommandLine.Parsing;
using Antlr4.Runtime;
using Apollon.Lib.Logging;

var fileArgument = new Argument<string>("file", "The path to the file that will be parsed.");

var queryOption = new Option<string>("--query", "The query that will be executed on the parsed file.");

var loggingOption = new Option<LogLevel>("--logging-level", "The Logging Level that the program should execute with.");

loggingOption.AddAlias("-l");

queryOption.AddAlias("-q");

var interactiveOption = new Option<bool>("--interactive", "The specified file will be read and preprocessed. After that interactive querries can be made.");

interactiveOption.AddAlias("-i");

var parseOnlyOption = new Option<bool>("--parse-only", "The specified file will only checked for syntactical and grammatical correctness.");

var rootCommand = new RootCommand("Parser for s(ASP).");

rootCommand.AddArgument(fileArgument);

rootCommand.AddOption(queryOption);

rootCommand.AddOption(interactiveOption);

rootCommand.AddOption(parseOnlyOption);

rootCommand.AddValidator(result =>
{
    if (result.Children.Count((s) => s.Symbol == interactiveOption ||
                                     s.Symbol == queryOption) != 1)
    {
        result.ErrorMessage = $"You must use either interactive Mode or specify a query!";
    }
});

fileArgument.AddValidator(result =>
{
    if (!System.IO.File.Exists(result.Tokens[0].Value))
    {
        result.ErrorMessage = $"The file {result.Tokens[0].Value} does not exist.";
    }
});

rootCommand.SetHandler(
    (file, query, interactive, parseOnly) =>
{
    var parser = new ApollonParser();

    var program = parser.ParseFromFile(file);

    if (parseOnly)
    {
        return;
    }

    if (interactive)
    {
        var solver = new Solver();
        solver.Load(program);

        string? input = string.Empty;
        Console.WriteLine("Now entering Interactive Mode. Enter ':q' or press CTRL+C to exit.");

        do
        {
            Console.Write("Query: ");
            input = Console.ReadLine();

            if (input == ":q")
            {
                break;
            }

            var goals = parser.ParseQueryFromString(input);

            var results = solver.Solve(goals);

            var resultBuilder = new ResultStringBuilder();

            foreach (var result in results)
            {
                Console.WriteLine(resultBuilder.CreateResultString(result));
            }
        }
        while (input != "exit");
    }
    else
    {
        var goals = parser.ParseQueryFromString(query);

        var solver = new Solver();
        solver.Load(program);

        var results = solver.Solve(goals);

        var resultBuilder = new ResultStringBuilder();

        foreach (var result in results)
        {
            Console.WriteLine(resultBuilder.CreateResultString(result));
        }
    }
},
    fileArgument,
    queryOption,
    interactiveOption,
    parseOnlyOption);

//return await rootCommand.InvokeAsync(args);
return rootCommand.Invoke(args);

//var query = "p(X), r(Y).";

//var parser = new ApollonParser();

//var program = parser.ParseFromFile("./Test.apo");
//var goals = parser.ParseQueryFromString(query);

//// var dokuGenerator = new DocumentationGenerator();
//// 
//// Console.WriteLine(dokuGenerator.GenerateDokumentationFor(program));

//var solver = new Solver();
//solver.Load(program);

//var results = solver.Solve(goals);

//var resultBuilder = new ResultStringBuilder();


//foreach (var result in results)
//{
//    Console.WriteLine(resultBuilder.CreateResultString(result));
//}

