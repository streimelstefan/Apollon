﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.DualRules;
using Apollon.Lib.Graph;
using Apollon.Lib.OLON;
using Apollon.Lib.Rules;

namespace Apollon.Lib
{
    public class Solver
    {

        public static void Solve(Program program)
        {
            var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

            var olons = OlonDetector.DetectOlonIn(callGraph);

            var rulePreprocessor = new RuleMetadataSetter(callGraph, olons);
            var processedRules = rulePreprocessor.SetMetadataOn(program.RuleTypesAsStatements.ToArray());
            IDualRuleGenerator dualRuleGenerator = new DualRuleGenerator();
            var dualRules = dualRuleGenerator.GenerateDualRules(program.Statements.ToArray());

        }

    }
}
