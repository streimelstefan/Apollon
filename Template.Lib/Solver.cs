﻿//-----------------------------------------------------------------------
// <copyright file="Solver.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib
{
    using Apollon.Lib.DualRules;
    using Apollon.Lib.Graph;
    using Apollon.Lib.Linker;
    using Apollon.Lib.Logging;
    using Apollon.Lib.NMRCheck;
    using Apollon.Lib.OLON;
    using Apollon.Lib.Resolution;
    using Apollon.Lib.Resolution.CallStackAndCHS;
    using Apollon.Lib.Resolution.CoSLD;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Unification;

    /// <summary>
    /// This class is the high level function in order to solve an asp program.
    /// </summary>
    public class Solver
    {
        /// <summary>
        /// Gets an Enumerable of all ProcessedStatements.
        /// </summary>
        public IEnumerable<Statement>? ProcessedStatements { get; private set; }

        /// <summary>
        /// Gets or sets the Resolution.
        /// </summary>
        public IResolution Resolution { get; set; } = new CoSLDResolution();

        /// <summary>
        /// Gets or sets the VariableLinker.
        /// </summary>
        public IVariableLinker VariableLinker { get; set; } = new VariableLinker();

        /// <summary>
        /// Gets or sets the NMRCheckGenerator.
        /// </summary>
        public INMRCheckGenerator NmrCheckGenerator { get; set; } = new NMRCheckGenerator();

        /// <summary>
        /// Gets the loaded Program.
        /// </summary>
        public Program? LoadedProgram { get; private set; }

        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        public ILogger Logger { get; set; } = new ConsoleLogger();

        private Statement? NMRCheck { get; set; }

        /// <summary>
        /// Loads a program into the solver.
        /// </summary>
        /// <param name="program">The loaded program generated by the parser/lexer.</param>
        public void Load(Program program)
        {
            IDualRuleGenerator dualRuleGenerator = new DualRuleGenerator();

            CallGraph callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);
            this.LogCallGraphIfNeeded(callGraph);

            OlonSet olons = OlonDetector.DetectOlonIn(callGraph);

            RuleMetadataSetter rulePreprocessor = new(callGraph, olons);
            PreprocessedStatement[] processedRules = rulePreprocessor.SetMetadataOn(program.RuleTypesAsStatements.ToArray());
            this.LogProcessedRulesIfNeeded(processedRules);

            DualRule[] dualRules = dualRuleGenerator.GenerateDualRules(program.Statements.ToArray());
            Statement[] nmrRules = this.NmrCheckGenerator.GenerateNMRCheckRules(processedRules, program);

            this.NMRCheck = nmrRules.Last();
            this.ProcessedStatements = program.Statements.Union(dualRules).Union(nmrRules).Select(s => (Statement)s.Clone()).Select(s => this.VariableLinker.LinkVariables(s)).ToArray();
            this.LoadedProgram = program;

            List<Statement> processedStatements = new();
            VariableExtractor variableExtractor = new();

            // foreach (var s in ProcessedStatments)
            // {
            //     var variables = variableExtractor.ExtractVariablesFrom(s);
            //     foreach (var variable in variables)
            //     {
            //         var newName = $"RV/{variableIndex}";
            //
            //         variable.Value = newName;
            //         variableIndex++;
            //     }
            // }
            this.Logger.Info($"Loaded and preprocessed program: \n{string.Join("\n", this.ProcessedStatements)}");
        }

        /// <summary>
        /// This function solves the asp program with the given goals.
        /// </summary>
        /// <param name="goals">The goals the asp program should solve to.</param>
        /// <returns>An Enumerable of all Answer Sets piece by piece.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if no program was loaded beforehand.</exception>
        public IEnumerable<ResolutionResult> Solve(BodyPart[] goals)
        {
            if (this.ProcessedStatements == null)
            {
                throw new InvalidOperationException("No program loaded.");
            }

            BodyPart[] linkedGoals = this.VariableLinker.LinkVariables(new Statement(null, goals)).Body;

            BodyPart nMRCheckGoal = new(((Statement)this.NMRCheck!.Clone()).Head, null);
            BodyPart[] goalsCopy = linkedGoals.Select(g => (BodyPart)g.Clone())
                .Append(nMRCheckGoal)
                .ToArray();

            IEnumerable<ResolutionResult> results = this.Resolution.Resolute(this.ProcessedStatements.ToArray(), goalsCopy, this.Logger);

            foreach (ResolutionResult res in results)
            {
                yield return this.PostProcessResult(goals, res);
            }
        }

        private ResolutionResult PostProcessResult(BodyPart[] goals, ResolutionResult res)
        {
            // get the values of the variables of the query. as the result has the variables filled in.
            Unifier unifier = new();
            VariableExtractor variableExtractor = new();

            // remove all answers that are not in the original program
            List<Literal> final = new();

            // ignore naf negation when selecting literals.
            Literal[] allLiterals = this.LoadedProgram!.AllLiterals.Select(l =>
            {
                l.IsNAF = false;
                return l;
            }).ToArray();
            foreach (Literal literal in res.CHS.Literals)
            {
                // if literal exists in programm add it to final
                Literal litCopy = (Literal)literal.Clone();
                litCopy.IsNAF = false;
                if (allLiterals.Where(l => unifier.Unify(l, litCopy).IsSuccess).Any())
                {
                    final.Add(literal);
                }
            }

            return new ResolutionResult(new CHS(final), res.Substitution);
        }

        private void LogProcessedRulesIfNeeded(IEnumerable<PreprocessedStatement> rules)
        {
            if (this.Logger.Level > LogLevel.Debug)
            {
                return;
            }

            this.Logger.Debug("Tagged Rules:");

            foreach (PreprocessedStatement rule in rules)
            {
                this.Logger.Debug($"  {rule}");
            }
        }

        private void LogCallGraphIfNeeded(CallGraph callGraph)
        {
            if (this.Logger.Level > LogLevel.Debug)
            {
                return;
            }

            this.Logger.Debug("Created Call Graph:");
            this.Logger.Debug("  Nodes:");
            foreach (CallGraphNode node in callGraph.Nodes)
            {
                this.Logger.Debug($"    {node}");
            }

            this.Logger.Debug("  Edges:");
            foreach (CallGraphEdge edge in callGraph.Edges)
            {
                this.Logger.Debug($"    {edge}");
            }
        }
    }
}
