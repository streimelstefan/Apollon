//-----------------------------------------------------------------------
// <copyright file="RuleMetadataSetter.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Rules
{
    using Apollon.Lib.Graph;
    using Apollon.Lib.OLON;

    /// <summary>
    /// Sets the metadata on rules.
    /// </summary>
    public class RuleMetadataSetter
    {
        private readonly CallGraph callGraph;
        private readonly OlonSet olons;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleMetadataSetter"/> class.
        /// </summary>
        /// <param name="callGraph">The callgraph used to set metadata from.</param>
        /// <param name="olons">The already detected OLON cycles.</param>
        public RuleMetadataSetter(CallGraph callGraph, OlonSet olons)
        {
            this.callGraph = callGraph;
            this.olons = olons;
        }

        /// <summary>
        /// Sets the metadata on the given statements.
        /// </summary>
        /// <param name="statements">The statements the metadata should be set on.</param>
        /// <returns>A List of all Statements with metadata.</returns>
        public PreprocessedStatement[] SetMetadataOn(Statement[] statements)
        {
            // this piece of code is highly inefficient...
            // TODO: Make faster without so many interations over the callgraph.
            PreprocessedStatement[] preProcessedRules = new PreprocessedStatement[statements.Length];

            for (int i = 0; i < statements.Length; i++)
            {
                bool isOrdinary = this.IsOrdinaryRule(statements[i]);
                bool isOlon = this.IsOlonRule(statements[i]);
                preProcessedRules[i] = new PreprocessedStatement(statements[i], isOlon, isOrdinary);
            }

            return preProcessedRules;
        }

        /// <summary>
        /// Determines whether the given statement is an ordinary rule.
        /// </summary>
        /// <param name="statement">The statement that should be checked for an ordinary rule.</param>
        /// <returns>A value determining whether the rule is an ordinary rule or not.</returns>
        public bool IsOrdinaryRule(Statement statement)
        {
            foreach (CallGraphNode node in this.callGraph.GetNodesOfStatement(statement))
            {
                // the node has other nodes that follow the path of the rule. Only nodes that represent the end of a path
                // are elegable to detect an ordinary rule.
                // if node is source node
                if (this.callGraph.GetEdgesOfNode(node).Where(edge => edge.CreatorRule == statement).Any())
                {
                    continue;
                }

                if (!this.olons.Nodes.Contains(node))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the given statement is an OLON rule.
        /// </summary>
        /// <param name="statement">The statement that should be checked for an ordinary rule.</param>
        /// <returns>A value determining whether the rule is an OLON rule or not.</returns>
        public bool IsOlonRule(Statement statement)
        {
            if (statement.Head == null)
            {
                return true;
            }

            return olons.IsPartOfOlon(callGraph.GetNodeOfLiteral(statement.Head));
        }
    }
}
