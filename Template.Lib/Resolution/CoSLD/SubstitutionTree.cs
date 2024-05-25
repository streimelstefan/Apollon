using Apollon.Lib.Atoms;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD
{
    /// <summary>
    /// A class that rememebers what variables point to what variables.
    /// </summary>
    public class SubstitutionTree : ICloneable
    {
        /// <summary>
        /// The substition tree.
        /// </summary>
        private List<KeyValuePair<string, string>> tree;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstitutionTree"/> class.
        /// </summary>
        public SubstitutionTree()
        {
            this.tree = new List<KeyValuePair<string, string>>();
        }

        private SubstitutionTree(IEnumerable<KeyValuePair<string, string>> tree)
        {
            this.tree = new List<KeyValuePair<string, string>>(tree);
        }

        /// <summary>
        /// Adds all the substitions of the given <see cref="Substitution"/> to the current tree.
        /// </summary>
        /// <param name="sub">The <see cref="Substitution"/> to add the elements from.</param>
        public void AddAllOf(Substitution sub)
        {
            foreach (var mapping in sub.Mappings)
            {
                this.AddEntry(mapping.Variable, mapping.MapsTo);
            }
        }

        /// <summary>
        /// Adds a tree entry to the tree. Where from substitutes to to. If either of the terms are not variables
        /// nothing will be added.
        /// </summary>
        /// <param name="from">The <see cref="Term"/> the signals the substitution.</param>
        /// <param name="to">The <see cref="AtomParam"/> that will be substitutted.</param>
        public void AddEntry(Term from, Term to)
        {
            ArgumentNullException.ThrowIfNull(from, nameof(from));
            ArgumentNullException.ThrowIfNull(to, nameof(to));

            if (!from.IsVariable || !to.IsVariable)
            {
                return;
            }

            if (this.Contains(from.Value, to.Value))
            {
                return;
            }

            this.tree.Add(new KeyValuePair<string, string>(from.Value, to.Value));
        }

        /// <summary>
        /// Adds a tree entry to the tree. Where from substitutes to to. 
        /// </summary>
        /// <param name="from">The <see cref="Term"/> the signals the substitution.</param>
        /// <param name="to">The <see cref="AtomParam"/> that will be substitutted.</param>
        public void AddEntry(Term from, AtomParam to)
        {
            if (to.Term == null)
            {
                return;
            }

            this.AddEntry(from, to.Term);
        }

        /// <summary>
        /// Checks if there is A connection from the from parameter to the to parameter.
        /// </summary>
        /// <param name="from">The <see cref="Term"/> to start the search from.</param>
        /// <param name="to">The <see cref="Term"/> to end the search at.</param>
        /// <returns>Whether there is a path between the two terms or not.</returns>
        public bool AreConnected(Term from, Term to)
        {
            ArgumentNullException.ThrowIfNull(from, nameof(from));
            ArgumentNullException.ThrowIfNull(to, nameof(to));

            var fromValue = from.Value;
            var toValue = to.Value;
            var visited = new HashSet<string>();  // To track visited nodes
            var queue = new Queue<string>();      // Queue for BFS

            queue.Enqueue(fromValue);
            visited.Add(fromValue);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                // If we reach the target value, return true
                if (current == toValue)
                {
                    return true;
                }

                // Enqueue all connected terms that haven't been visited
                foreach (var neighbor in this.tree.Where(x => x.Key == current))
                {
                    if (!visited.Contains(neighbor.Value))
                    {
                        visited.Add(neighbor.Value);
                        queue.Enqueue(neighbor.Value);
                    }
                }
            }

            return false;  // If no connection found, return false
        }

        /// <summary>
        /// Returns a clone of the current object.
        /// </summary>
        /// <returns>A clone of the current object.</returns>
        public object Clone()
        {
            return new SubstitutionTree(this.tree.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value)));
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>The string representation of the current object.</returns>
        public override string ToString()
        {
            return $"{string.Join(", ", this.tree.Select(kv => $"({kv.Key} -> {kv.Value})"))}]";
        }

        private bool Contains(string from, string to)
        {
            return this.tree.Where(kv => kv.Key == from && kv.Value == to).Any();
        }
    }
}
