using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib.Graph;
using Template.Lib.OLON;

namespace Template.Lib.Rules
{
    public class RuleMetadataSetter
    {

        private readonly CallGraph _callGraph;
        private readonly OlonSet _olons;

        public RuleMetadataSetter(CallGraph callGraph, OlonSet olons)
        {
            _callGraph = callGraph;
            _olons = olons;
        }

        public PreprocessedRule[] SetMetadataOn(Rule[] rules)
        {
            // this piece of code is highly inefficient...
            // TODO: Make faster without so many interations over the callgraph.
            var preProcessedRules = new PreprocessedRule[rules.Length];

            for (int i = 0; i < rules.Length; i++)
            {
                var isOrdinary = IsOrdinaryRule(rules[i]);
                var isOlon = IsOlonRule(rules[i]);
                preProcessedRules[i] = new PreprocessedRule(rules[i], isOlon, isOrdinary);
            }

            return preProcessedRules;
        }

        public bool IsOrdinaryRule(Rule rule)
        {
            foreach (var node in _callGraph.GetNodesOfRule(rule))
            {
                // the node has other nodes that follow the path of the rule. Only nodes that represent the end of a path
                // are elegable to detect an ordinary rule.
                if (_callGraph.GetEdgesOfNode(node).Where(edge => edge.CreatorRule == rule).Any())
                {
                    continue;
                }

                if (!_olons.Nodes.Contains(node))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsOlonRule(Rule rule)
        {
            foreach (var node in _callGraph.GetNodesOfRule(rule))
            {
                if (_olons.Nodes.Contains(node))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
