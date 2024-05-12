using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.DualRules;
using Apollon.Lib.Graph;
using Apollon.Lib.Linker;
using Apollon.Lib.OLON;
using Apollon.Lib.Resolution;
using Apollon.Lib.Resolution.SLD;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;

namespace Apollon.Lib
{
    public class Solver
    {

        public IEnumerable<Statement>? ProcessedStatments { get; private set; }
        public IResolution Resolution { get; set; } = new SLDResolution();

        public IVariableLinker VariableLinker { get; set; } = new VariableLinker();

        public Program LoadedProgram { get; private set; }

        public void Load(Program program)
        {
            IDualRuleGenerator dualRuleGenerator = new DualRuleGenerator();

            var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

            var olons = OlonDetector.DetectOlonIn(callGraph);

            var rulePreprocessor = new RuleMetadataSetter(callGraph, olons);
            var processedRules = rulePreprocessor.SetMetadataOn(program.RuleTypesAsStatements.ToArray());
            var dualRules = dualRuleGenerator.GenerateDualRules(program.Statements.ToArray());

            ProcessedStatments = program.Statements.Union(dualRules).Select(s => VariableLinker.LinkVariables(s)).ToArray();
            LoadedProgram = program;
        }

        public ResolutionResult Solve(BodyPart[] goals)
        {
            if (ProcessedStatments == null)
            {
                throw new InvalidOperationException("No program loaded.");
            }
            var linkedGoals = VariableLinker.LinkVariables(new Statement(null, goals)).Body;
            var goalsCopy = linkedGoals.Select(g => (BodyPart)g.Clone()).ToArray();

            var res = Resolution.Resolute(ProcessedStatments.ToArray(), goalsCopy);

            return PostProcessResult(goals, res);
        }

        private ResolutionResult PostProcessResult(BodyPart[] goals, ResolutionResult res)
        {
            // get the values of the variables of the query. as the result has the variables filled in.
            var unifier = new Unifier();
            Substitution sub = new Substitution();
            foreach (var goal in goals)
            {
                foreach (var literal in res.CHS.Literals)
                {
                    var goalRes = unifier.Unify(goal.Literal, literal);

                    if (goalRes.IsError)
                    {
                        continue;
                    }

                    foreach (var mapping in goalRes.Value.Mappings)
                    {
                        sub.Add(mapping.Variable, mapping.MapsTo);
                    }
                }
            }

            // remove all answers that are not in the original program
            var final = new List<Literal>();
            var allLiterals = LoadedProgram.AllLiterals.ToArray();
            foreach (var literal in res.CHS.Literals)
            {
                // if literal exists in programm add it to final
                if (allLiterals.Where(l => unifier.Unify(l, literal).IsSuccess).Any())
                {
                    final.Add(literal);
                }
            }

            return new ResolutionResult(new CHS(final), sub);
        }
    }
}
