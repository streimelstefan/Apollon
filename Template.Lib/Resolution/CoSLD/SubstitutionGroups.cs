namespace Apollon.Lib.Resolution.CoSLD
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// A class that remembers what variables should be equal to each other.
    /// </summary>
    public class SubstitutionGroups : ICloneable
    {
        /// <summary>
        /// The substition tree.
        /// </summary>
        private List<HashSet<string>> groups;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstitutionGroups"/> class.
        /// </summary>
        public SubstitutionGroups()
        {
            this.groups = new List<HashSet<string>>();
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

            foreach (var group in this.groups)
            {
                if (group.Contains(from.Value) || group.Contains(to.Value))
                {
                    group.Add(to.Value);
                    group.Add(from.Value);
                    return;
                }
            }

            var newGroup = new HashSet<string>()
            {
                from.Value,
                to.Value,
            };

            this.groups.Add(newGroup);
        }

        /// <summary>
        /// Returns the substitution group name of the given term.
        /// </summary>
        /// <param name="term">The term of which the substitution group name should be gotten.</param>
        /// <returns>The name of the substitution group name.</returns>
        /// <exception cref="ArgumentException">Is thrown when the term is no variable.</exception>
        /// <exception cref="InvalidOperationException">Is thrown when there is no substitution group.</exception>
        public string GetSubstitionGroupNameOf(Term term)
        {
            if (!term.IsVariable)
            {
                throw new ArgumentException("Term given needs to be a variable");
            }

            for (int i = 0; i < this.groups.Count(); i++)
            {
                var group = this.groups[i];
                if (group.Contains(term.Value))
                {
                    return $"GV/{i}";
                }
            }

            throw new InvalidOperationException($"Unable to find substitution group of variable {term}");
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

            foreach (var group in this.groups)
            {
                if (group.Contains(fromValue) && group.Contains(toValue))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a clone of the current object.
        /// </summary>
        /// <returns>A clone of the current object.</returns>
        public object Clone()
        {
            var newSubTree = new SubstitutionGroups();

            foreach (var group in this.groups)
            {
                var newGroup = new HashSet<string>();

                foreach (var item in group)
                {
                    newGroup.Add(item);
                }

                newSubTree.groups.Add(newGroup);
            }

            return newSubTree;
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>The string representation of the current object.</returns>
        public override string ToString()
        {
            return $"[{string.Join(", ", this.groups.Select(g => $"{{{string.Join(", ", g.Select(i => i.ToString()))}}}"))}]";
        }
    }
}
