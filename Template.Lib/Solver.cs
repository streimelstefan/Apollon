using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.DualRules;
using Apollon.Lib.Graph;
using Apollon.Lib.OLON;
using Apollon.Lib.Resolution;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification;

namespace Apollon.Lib
{
    public class Solver
    {

        public IEnumerable<Statement>? ProcessedStatments { get; private set; }
        public IResolution Resolution { get; set; } = new SLDResolution();

        public void Load(Program program)
        {
            IDualRuleGenerator dualRuleGenerator = new DualRuleGenerator();

            var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

            var olons = OlonDetector.DetectOlonIn(callGraph);

            var rulePreprocessor = new RuleMetadataSetter(callGraph, olons);
            var processedRules = rulePreprocessor.SetMetadataOn(program.RuleTypesAsStatements.ToArray());
            var dualRules = dualRuleGenerator.GenerateDualRules(program.Statements.ToArray());

            ProcessedStatments = program.Statements.Union(dualRules).ToArray();
        }

        public ResolutionResult Solve(BodyPart[] goals)
        {
            if (ProcessedStatments == null)
            {
                throw new InvalidOperationException("No program loaded.");
            }
            var goalsCopy = goals.Select(g => (BodyPart)g.Clone()).ToArray();

            var res = Resolution.Resolute(ProcessedStatments.ToArray(), goalsCopy);

            var unifier = new Unifier();
            Substitution sub = new Substitution();
            foreach (var goal in  goals)
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

            return new ResolutionResult(res.CHS, sub);
        }

    }
}
