using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib.Rules;

namespace Template.Lib.Graph
{
    /// <summary>
    /// Represents a CallGraph. A CallGraph is a directed graph that represents the calls between Literals.
    /// </summary>
    public class CallGraph
    {

        public CallGraphNode? Root { get; set; } // private or public?; made them public for now for testing purposes
        public List<CallGraphNode> Nodes { get; set; } // Nodes contains Root Element
        public List<CallGraphEdge> Edges { get; set; } // Edges are directional

        /// <summary>
        /// Initializes a new instance of the CallGraph class.
        /// </summary>
        public CallGraph()
        {
            Nodes = new List<CallGraphNode>();
            Edges = new List<CallGraphEdge>();
        }

        /// <summary>
        /// Adds a Node to the CallGraph. If the Root is not set, the Node will be set as the Root.
        /// </summary>
        /// <param name="node">The Node that will be added onto the CallGraph.</param>
        public CallGraphNode AddNode(CallGraphNode node)
        {
            if (Nodes.Contains(node))
            {
                throw new ArgumentException("Node already exists in the Graph!");
            }

            if (Root == null)
            {
                Root = node;
            }

            // Add Check if Literal already exists inside Graph?
            Nodes.Add(node);
            return node;
        }

        public CallGraphNode AddNode(Literal node)
        {
            return this.AddNode(new CallGraphNode(node));
        }

        /// <summary>
        /// Adds an Edge to the CallGraph.
        /// </summary>
        /// <param name="edge">The Edge that will be added.</param>
        public CallGraphEdge AddEdge(CallGraphEdge edge)
        {
            if (Edges.Contains(edge))
            {
                throw new ArgumentException("Edge already exists in the Graph!");
            }

            if ((edge.Source != null && !Nodes.Contains(edge.Source)) || !Nodes.Contains(edge.Target))
            {
                throw new ArgumentException("Source or Target Node of Edge is not part of the Graph!");
            }

            Edges.Add(edge);
            return edge;
        }

        public CallGraphEdge AddEdge(CallGraphNode? source, CallGraphNode target, bool isNaf, Rule creatorRule)
        {
            return this.AddEdge(new CallGraphEdge(source, target, isNaf, creatorRule));
        }

        public IEnumerable<CallGraphEdge> GetEdgesOfNode(CallGraphNode node)
        {
            return Edges.Where(edge => edge.Source != null && edge.Source.Equals(node));
        }

        public IEnumerable<CallGraphEdge> GetAllEdgesOfNode(CallGraphNode node)
        {
            return Edges.Where(edge => edge.Source != null && edge.Source.Equals(node) || edge.Target.Equals(node));
        }

        public IEnumerable<CallGraphNode> GetNodesOfRule(Rule rule)
        {
            return Nodes.Where(node => GetAllEdgesOfNode(node).Where(edge => edge.CreatorRule == rule).Any());
        }


        /// <summary>
        /// Returns the Node that represents the given Literal. If the Node does not exist, null will be returned.
        /// </summary>
        /// <param name="literal">The Literal that should be looked for in the Graph.</param>
        /// <returns>The Node if given Literal was found; Null if given Literal is not found.</returns>
        public CallGraphNode? GetNode(Literal literal)
        {
            return Nodes.Find(node => node.Literal.Equals(literal));
        }
    }
}
