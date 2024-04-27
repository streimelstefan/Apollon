using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.Graph;
using Apollon.Lib.OLON;

namespace Apollon.Lib.Rules
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

        public PreprocessedStatement[] SetMetadataOn(Statement[] statements)
        {
            // this piece of code is highly inefficient...
            // TODO: Make faster without so many interations over the callgraph.
            var preProcessedRules = new PreprocessedStatement[statements.Length];

            for (int i = 0; i < statements.Length; i++)
            {
                var isOrdinary = IsOrdinaryRule(statements[i]);
                var isOlon = IsOlonRule(statements[i]);
                preProcessedRules[i] = new PreprocessedStatement(statements[i], isOlon, isOrdinary);
            }

            return preProcessedRules;
        }

        public bool IsOrdinaryRule(Statement statement)
        {
            foreach (var node in _callGraph.GetNodesOfStatement(statement))
            {
                // the node has other nodes that follow the path of the rule. Only nodes that represent the end of a path
                // are elegable to detect an ordinary rule.
                // if node is source node
                if (_callGraph.GetEdgesOfNode(node).Where(edge => edge.CreatorRule == statement).Any())
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

        public bool IsOlonRule(Statement statement)
        {
            foreach (var node in _callGraph.GetNodesOfStatement(statement))
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
