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
using Apollon.Lib.Unification.Substitutioners;

namespace Apollon.Lib
{
    public class Solver
    {

        public IEnumerable<Statement>? ProcessedStatments { get; private set; }
        public IResolution Resolution { get; set; } = new CoSLDResolution();

        public IVariableLinker VariableLinker { get; set; } = new VariableLinker();

        public INMRCheckGenerator nmrCheckGenerator { get; set; } = new NMRCheckGenerator();

        private Statement? NMRCheck { get; set; }

        public Program? LoadedProgram { get; private set; }

        public ILogger Logger { get; set; } = new ConsoleLogger();

        public void Load(Program program)
        {
            IDualRuleGenerator dualRuleGenerator = new DualRuleGenerator();

            var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);
            var olons = OlonDetector.DetectOlonIn(callGraph);

            var rulePreprocessor = new RuleMetadataSetter(callGraph, olons);
            var processedRules = rulePreprocessor.SetMetadataOn(program.RuleTypesAsStatements.ToArray());
            var dualRules = dualRuleGenerator.GenerateDualRules(program.Statements.ToArray());
            var nmrRules = nmrCheckGenerator.GenerateNMRCheckRules(processedRules);

            NMRCheck = nmrRules.Last();
            ProcessedStatments = program.Statements.Union(dualRules).Union(nmrRules).Select(s => VariableLinker.LinkVariables(s)).ToArray();
            LoadedProgram = program;

            Logger.Info($"Loaded and preprocessed program: \n{string.Join("\n", ProcessedStatments)}");
        }

        public IEnumerable<ResolutionResult> Solve(BodyPart[] goals)
        {
            if (ProcessedStatments == null)
            {
                throw new InvalidOperationException("No program loaded.");
            }
            var linkedGoals = VariableLinker.LinkVariables(new Statement(null, goals)).Body;

            var NMRCheckGoal = new BodyPart(((Statement)NMRCheck.Clone()).Head, null);
            var goalsCopy = linkedGoals.Select(g => (BodyPart)g.Clone())
                .Append(NMRCheckGoal)
                .ToArray();

            var results = Resolution.Resolute(ProcessedStatments.ToArray(), goalsCopy, Logger);

            foreach (var res in results)
            {
                yield return PostProcessResult(goals, res);
            }
        }

        private ResolutionResult PostProcessResult(BodyPart[] goals, ResolutionResult res)
        {
            // get the values of the variables of the query. as the result has the variables filled in.
            var unifier = new Unifier();
            var variableExtractor = new VariableExtractor();
            // Substitution sub = new Substitution();
            // foreach (var goal in goals)
            // {
            //     foreach (var literal in res.CHS.Literals)
            //     {
            //         var goalRes = unifier.Unify(goal.Literal, literal);
            // 
            //         if (goalRes.IsError)
            //         {
            //             continue;
            //         }
            // 
            //         foreach (var mapping in goalRes.Value.Mappings)
            //         {
            //             sub.Add(mapping.Variable, mapping.MapsTo);
            //         }
            //     }
            // }

            // remove all answers that are not in the original program
            var final = new List<Literal>();
            // ignore naf negation when selecting literals.
            var allLiterals = LoadedProgram.AllLiterals.Select(l => { l.IsNAF = false; return l; }).ToArray();
            foreach (var literal in res.CHS.Literals)
            {
                // if literal exists in programm add it to final
                var litCopy = (Literal)literal.Clone();
                litCopy.IsNAF = false;
                if (allLiterals.Where(l => unifier.Unify(l, litCopy).IsSuccess).Any())
                {
                    final.Add(literal);
                }
            }

            return new ResolutionResult(new CHS(final), res.Substitution);
        }
    }
}
