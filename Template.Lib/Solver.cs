using Apollon.Lib.CallGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib.Graph;
using Template.Lib.OLON;
using Template.Lib.Rules;

namespace Template.Lib
{
    public class Solver
    {

        public static void Solve(Program program)
        {
            var callGraph = new CallGraphBuilder(new LiteralTermCountEqualizer()).BuildCallGraph(program);

            var olons = OlonDetector.DetectOlonIn(callGraph);

            var rulePreprocessor = new RuleMetadataSetter(callGraph, olons);
            var processedRules = rulePreprocessor.SetMetadataOn(program.RuleList);

        }

    }
}
