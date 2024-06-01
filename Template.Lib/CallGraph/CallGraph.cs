//-----------------------------------------------------------------------
// <copyright file="CallGraph.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Graph
{
    using Apollon.Lib.Rules;

    /// <summary>
    /// Represents a CallGraph. A CallGraph is a directed graph that represents the calls between Literals.
    /// </summary>
    public class CallGraph
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallGraph"/> class.
        /// </summary>
        /// <param name="equalizer">The <see cref="IEqualizer{T}"/> that should be used to compare the <see cref="Literal"/>s.</param>
        public CallGraph(IEqualizer<Literal> equalizer)
        {
            this.Nodes = new List<CallGraphNode>();
            this.Edges = new List<CallGraphEdge>();
            this.Equalizer = equalizer;
        }

        /// <summary>
        /// Gets or sets the root node of the CallGraph.
        /// </summary>
        public CallGraphNode? Root { get; set; }

        /// <summary>
        /// Gets or sets the Nodes of the CallGraph.
        /// </summary>
        public List<CallGraphNode> Nodes { get; set; }

        /// <summary>
        /// Gets or sets the Edges of the CallGraph.
        /// </summary>
        public List<CallGraphEdge> Edges { get; set; }

        /// <summary>
        /// Gets the Equalizer that is used to compare Literals and build the <see cref="CallGraph"/>.
        /// </summary>
        public IEqualizer<Literal> Equalizer { get; private set; }

        /// <summary>
        /// Adds a Node to the CallGraph. If the Root is not set, the Node will be set as the Root.
        /// </summary>
        /// <param name="node">The Node that will be added onto the CallGraph.</param>
        /// <returns>The Node that was added.</returns>
        public CallGraphNode AddNode(CallGraphNode node)
        {
            if (this.Nodes.Contains(node))
            {
                throw new ArgumentException("Node already exists in the Graph!");
            }

            this.Root ??= node;

            // Add Check if Literal already exists inside Graph?
            this.Nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Adds a new Node to the CallGraph.
        /// </summary>
        /// <param name="node">The node that should be added to the <see cref="CallGraph"/>.</param>
        /// <returns>The node that was added.</returns>
        public CallGraphNode AddNode(Literal node)
        {
            return this.AddNode(new CallGraphNode(node));
        }

        /// <summary>
        /// Adds an Edge to the CallGraph.
        /// </summary>
        /// <param name="edge">The Edge that will be added.</param>
        /// <returns>The Edge that was added.</returns>
        public CallGraphEdge AddEdge(CallGraphEdge edge)
        {
            if (this.Edges.Contains(edge))
            {
                throw new ArgumentException("Edge already exists in the Graph!");
            }

            if ((edge.Source != null && !this.Nodes.Contains(edge.Source)) || !this.Nodes.Contains(edge.Target))
            {
                throw new ArgumentException("Source or Target Node of Edge is not part of the Graph!");
            }

            this.Edges.Add(edge);
            return edge;
        }

        /// <summary>
        /// Adds an edge to the CallGraph.
        /// </summary>
        /// <param name="source">The source node of the edge.</param>
        /// <param name="target">The target node of the edge.</param>
        /// <param name="isNaf">Whether or not the edge originated from a NAF connection.</param>
        /// <param name="creatorRule">The rule this edge originated from.</param>
        /// <returns>The edge that was added.</returns>
        public CallGraphEdge AddEdge(CallGraphNode? source, CallGraphNode target, bool isNaf, Statement creatorRule)
        {
            return this.AddEdge(new CallGraphEdge(source, target, isNaf, creatorRule));
        }

        /// <summary>
        /// Returns all the Edges that have the given Node as their Source.
        /// </summary>
        /// <param name="node">The node to return the edges from.</param>
        /// <returns>All the edges of the given node.</returns>
        public IEnumerable<CallGraphEdge> GetEdgesOfNode(CallGraphNode node)
        {
            return this.Edges.Where(edge => edge.Source != null && edge.Source.Equals(node));
        }

        /// <summary>
        /// Returns all the Edges that have the given Node as their Target or their Source.
        /// </summary>
        /// <param name="node">The node to get the edges from.</param>
        /// <returns>All the edges of the node.</returns>
        public IEnumerable<CallGraphEdge> GetAllEdgesOfNode(CallGraphNode node)
        {
            return this.Edges.Where(edge => (edge.Source != null && edge.Source.Equals(node)) || edge.Target.Equals(node));
        }

        /// <summary>
        /// Returns all the Nodes that are releted to the given Statement.
        /// </summary>
        /// <param name="rule">The statement to get the nodes from.</param>
        /// <returns>All the nodes that are related to an statement.</returns>
        public IEnumerable<CallGraphNode> GetNodesOfStatement(Statement rule)
        {
            return this.Nodes.Where(node => this.GetAllEdgesOfNode(node).Where(edge => edge.CreatorRule == rule).Any());
        }

        /// <summary>
        /// Returns the Node that represents the given Literal. If the Node does not exist, null will be returned.
        /// </summary>
        /// <param name="literal">The Literal that should be looked for in the Graph.</param>
        /// <returns>The Node if given Literal was found; Null if given Literal is not found.</returns>
        public CallGraphNode? GetNode(Literal literal)
        {
            return this.Nodes.Find(node => this.Equalizer.AreEqual(node.Literal, literal));
        }
    }
}
