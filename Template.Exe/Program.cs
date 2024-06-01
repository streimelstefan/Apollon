//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

using System.CommandLine;
using System.CommandLine.Parsing;
using Apollon.Lib;
using Apollon.Lib.Docu;
using Apollon.Lib.Logging;
using Apollon.Lib.Rules;
using AppollonParser;

Argument<string> fileArgument = new("file", "The path to the file that will be parsed.");

Option<string> queryOption = new("--query", "The query that will be executed on the parsed file.");

queryOption.AddAlias("-q");

Option<LogLevel> loggingOption = new("--logging-level", "The Logging Level that the program should execute with.");

loggingOption.AddAlias("-l");

loggingOption.SetDefaultValue(LogLevel.NoLogging);

Option<bool> interactiveOption = new("--interactive", "The specified file will be read and preprocessed. After that interactive querries can be made.");

interactiveOption.AddAlias("-i");

Option<bool> parseOnlyOption = new("--parse-only", "The specified file will only checked for syntactical and grammatical correctness.");

Option<bool> generateDocuOption = new("--generate-documentation", "Generates the documentation for the file loaded.");

generateDocuOption.AddAlias("-d");

RootCommand rootCommand = new("Parser for s(ASP).");

rootCommand.AddArgument(fileArgument);

rootCommand.AddOption(queryOption);

rootCommand.AddOption(interactiveOption);

rootCommand.AddOption(parseOnlyOption);

rootCommand.AddOption(loggingOption);

rootCommand.AddOption(generateDocuOption);

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
    (file, query, interactive, parseOnly, logLevel, genDocu) =>
{
    ApollonParser parser = new();

    Apollon.Lib.Program program = parser.ParseFromFile(file);

    if (parseOnly)
    {
        return;
    }

    if (genDocu)
    {
        DocumentationGenerator dokuGenerator = new();

        Console.WriteLine(dokuGenerator.GenerateDokumentationFor(program));
        return;
    }

    if (interactive)
    {
        Solver solver = new();
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

            BodyPart[] goals = parser.ParseQueryFromString(input);

            IEnumerable<Apollon.Lib.Resolution.ResolutionResult> results = solver.Solve(goals);

            ResultStringBuilder resultBuilder = new();

            foreach (Apollon.Lib.Resolution.ResolutionResult result in results)
            {
                Console.WriteLine(resultBuilder.CreateResultString(result));
            }
        }
        while (input != "exit");
    }
    else
    {
        BodyPart[] goals = parser.ParseQueryFromString(query);

        Solver solver = new();
        solver.Logger.Level = logLevel;
        solver.Load(program);

        IEnumerable<Apollon.Lib.Resolution.ResolutionResult> results = solver.Solve(goals);

        ResultStringBuilder resultBuilder = new();

        foreach (Apollon.Lib.Resolution.ResolutionResult result in results)
        {
            Console.WriteLine(resultBuilder.CreateResultString(result));
        }
    }
},
    fileArgument,
    queryOption,
    interactiveOption,
    parseOnlyOption,
    loggingOption,
    generateDocuOption);

return rootCommand.Invoke(args);