using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib.Graph;

namespace Template.Lib.OLON
{
    public class OlonDetector
    {
        public static OlonSet DetectOlonIn(CallGraph callGraph)
        {
            if (callGraph.Root == null) throw new ArgumentException($"{nameof(callGraph)} is not allowed to be empty!");

            var olonSet = new OlonSet();

            // contains all the nodes that have been visited in a specific path.
            var visited = new List<CallGraphNode>();

            DetectOlonRec(callGraph.Root, visited, olonSet, callGraph);

            return olonSet;
        }

        private static void DetectOlonRec(CallGraphNode node, List<CallGraphNode> visited, OlonSet olonSet, CallGraph callGraph, int nafCount = 0)
        {
            // create a copy of the current visiting stack, so there is no false positive. If we did not do this a path without an olon
            // could be tagged as containing an olon, when the path gets scanned before an olon loop. In this case all the nodes of the loop would be added
            // to the visited stack of that olon stack, because the visited stack would be a global entity over the whole execution context of this function.
            // by coping we make sure that only the nodes that are actually in one path are in the stack.
            var visitedCopy = new List<CallGraphNode>(visited);

            // if there is a loop with an odd number of naf
            if (visited.Contains(node) && nafCount % 2 == 1)
            {
                // Add all nodes to olon set, no need to add the current node since it is already in the visited stack.
                olonSet.Nodes.UnionWith(visitedCopy);
                return;
            }

            visitedCopy.Add(node);

            foreach (var edge in callGraph.getEdgesOfNode(node))
            {
                DetectOlonRec(edge.Target, visitedCopy, olonSet, callGraph, edge.IsNAF ? nafCount + 1 : nafCount);
            }
        }

    }
}
