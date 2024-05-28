﻿using Apollon.Lib.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.OLON
{
    /// <summary>
    /// Detects all the olon loops in a given <see cref="CallGraph"/>.
    /// </summary>
    public class OlonDetector
    {
        /// <summary>
        /// Detects all the olon loops in a given <see cref="CallGraph"/>.
        /// </summary>
        /// <param name="callGraph">The callgraph to detect olons in.</param>
        /// <returns>All the nodes that belong to an OLON.</returns>
        /// <exception cref="ArgumentException">is thrown if the callgraph is null.</exception>
        public static OlonSet DetectOlonIn(CallGraph callGraph)
        {
            if (callGraph.Root == null) throw new ArgumentException($"{nameof(callGraph)} is not allowed to be empty!");

            var olonSet = new OlonSet();

            // contains all the nodes that have been visited in a specific path.
            // TODO: Change List to Stack so the list does not need to be recreated at each stepp
            // but can just be popped at the end of each step.
            var visited = new List<(CallGraphNode, int)>();

            foreach (var startNode in callGraph.Nodes)
            {
                // TODO: if startNode has been visited already skip check
                DetectOlonRec(startNode, visited, olonSet, callGraph);
            }

            // add all target nodes of constraint rules
            // foreach (var node in callGraph.Edges.Where(e => e.Source == null).Select(e => e.Target))
            // {
            //     olonSet.Nodes.Add(node);
            // }

            return olonSet;
        }

        private static void DetectOlonRec(CallGraphNode node, List<(CallGraphNode node, int nafCount)> visited, OlonSet olonSet, CallGraph callGraph)
        {
            // create a copy of the current visiting stack, so there is no false positive. If we did not do this a path without an olon
            // could be tagged as containing an olon, when the path gets scanned before an olon loop. In this case all the nodes of the loop would be added
            // to the visited stack of that olon stack, because the visited stack would be a global entity over the whole execution context of this function.
            // by coping we make sure that only the nodes that are actually in one path are in the stack.
            var visitedCopy = new List<(CallGraphNode node, int nafCount)>(visited);

            // if there is a loop
            var recursionNode = visited.Where(i => i.node == node);
            if (recursionNode.Any())
            {
                // if the loop is an ood loop over negation
                if (recursionNode.First().nafCount % 2 == 1)
                {
                    // Add all nodes to olon set, no need to add the current node since it is already in the visited stack.
                    // TODO: Add only the nodes form the current node up.
                    olonSet.Nodes.UnionWith(visitedCopy.SkipWhile(n => n.node != node).Select(i => i.node));
                }
                return;
            }


            visitedCopy.Add((node, 0));

            foreach (var edge in callGraph.GetEdgesOfNode(node))
            {
                if (edge.IsNAF)
                {
                    DetectOlonRec(edge.Target, visitedCopy.Select(i => (i.node, ++i.nafCount)).ToList(), olonSet, callGraph);
                } else
                {
                    DetectOlonRec(edge.Target, visitedCopy, olonSet, callGraph);
                }
            }
        }

    }
}
