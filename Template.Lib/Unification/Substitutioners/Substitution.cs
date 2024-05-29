namespace Apollon.Lib.Unification.Substitutioners
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Rules.Operations;

    /// <summary>
    /// 
    /// </summary>
    public class Substitution
    {
        private Dictionary<string, AtomParam> mappings = new Dictionary<string, AtomParam>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Substitution"/> class.
        /// </summary>
        public Substitution()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Substitution"/> class.
        /// </summary>
        /// <param name="mappings"></param>
        public Substitution(IEnumerable<Mapping> mappings)
        {
            this.mappings = new Dictionary<string, AtomParam>(mappings.Select(m => new KeyValuePair<string, AtomParam>(m.Variable.Value, m.MapsTo)));
        }

        /// <summary>
        /// Gets the mappings.
        /// </summary>
        public IEnumerable<Mapping> Mappings
        {
            get
            {
                return this.mappings.Select(m =>
                {
                    if (m.Value.Term != null && m.Value.Term.IsVariable)
                    {
                        return new Mapping(new Term(m.Key, (PVL)m.Value.Term.ProhibitedValues.Clone()), m.Value);
                    }
                    else
                    {
                        return new Mapping(new Term(m.Key), m.Value);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the bound mappings.
        /// </summary>
        public IEnumerable<Mapping> BoundMappings
        {
            get
            {
                return this.Mappings.Where(m => m.MapsTo.IsLiteral || (m.MapsTo.Term != null && !m.MapsTo.Term.IsVariable));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="term"></param>
        /// <exception cref="InvalidOperationException">Is thrown when the substitution can not be added under the existing name.</exception>
        public void Add(Term variable, AtomParam term)
        {
            if (this.mappings.ContainsKey(variable.Value))
            {
                if (this.mappings[variable.Value].Term != null && this.mappings[variable.Value].Term.IsVariable &&
                    term.Term != null && term.Term.IsVariable)
                {
                    // if the value to add and the mapsTo of the mapping here are both variables Unionize their PVLs.
                    // and abort.
                    PVL.Union(this.mappings[variable.Value].Term.ProhibitedValues, term.Term.ProhibitedValues);
                    return;
                }

                if (!this.mappings[variable.Value].Equals(term))
                {
                    throw new InvalidOperationException("Cannot add new substitution under the existing name");
                }
            }

            // if (term.Term != null && term.Term.IsVariable && variable.Equals(term.Term))
            // {
            //     return;
            // }
            this.mappings[variable.Value] = term;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="term"></param>
        public void Add(Term variable, Term term)
        {
            this.Add(variable, new AtomParam(term));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="term"></param>
        public void ForceAdd(Term variable, AtomParam term)
        {
            this.mappings[variable.Value] = term;
        }

        public void ForceAdd(Term variable, Term term)
        {
            this.ForceAdd(variable, new AtomParam(term));
        }

        /// <summary>
        /// Tries to add a new substitution to the current substitution.
        /// </summary>
        /// <param name="variable">The variable that shall be added.</param>
        /// <param name="term">Ther term that shall be added.</param>
        /// <returns>Returns true when operation was succesfull, false otherwise.</returns>
        public bool TryAdd(Term variable, AtomParam term)
        {
            try
            {
                this.Add(variable, term);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public Statement Apply(Statement statement)
        {
            var copy = (Statement)statement.Clone();

            if (copy.Head != null)
            {
                this.Apply(copy.Head.Atom);
            }

            foreach (var part in copy.Body)
            {
                this.Apply(part);
            }

            return copy;
        }



        /// <summary>
        /// Removes all prohibited values from the variables.
        /// </summary>
        public void RemovePVls()
        {
            foreach (var mapping in this.mappings)
            {
                if (mapping.Value.Term != null && mapping.Value.Term.IsVariable)
                {
                    mapping.Value.Term.ProhibitedValues.Clear();
                }
            }
        }

        /// <summary>
        /// Converts the current substitution to a string.
        /// </summary>
        /// <returns>Returns a string containing the current substitution.</returns>
        public override string ToString()
        {
            return $"{{ {string.Join(", ", this.mappings.Select(kv => $"{kv.Key} -> {kv.Value}"))} }}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        public Literal Apply(Literal literal)
        {
            var copy = (Literal)literal.Clone();
            this.Apply(copy.Atom);

            return copy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="literal"></param>
        public void ApplyInline(Literal literal)
        {
            this.Apply(literal.Atom);
        }

        /// <summary>
        /// Applies the current substitution to the given operation.
        /// </summary>
        /// <param name="operation">The operation that should be applied.</param>
        /// <returns>The Operation.</returns>
        public Operation Apply(Operation operation)
        {
            var copy = (Operation)operation.Clone();

            this.ApplyOperation(copy);

            return copy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inductor"></param>
        public void BackPropagate(Substitution inductor)
        {
            var inductorCopy = inductor.Clone();

            var candidates = this.mappings
                .Where(im => im.Value.IsTerm) // we only care about the term values since they can be variables
                .Where(im => im.Value.Term.IsVariable) // only the values that are variables
                .Where(im => inductor.Mappings.Where(m => m.Variable.Value == im.Value.Term.Value).Any()) // only those that look like this ... -> Y | Y -> ...
                .Select(im => (im, inductor.Mappings.Where(m => m.Variable.Value == im.Value.Term.Value).First()));

            foreach (var candiate in candidates)
            {
                if (candiate.Item2.MapsTo.Term != null && candiate.Item2.MapsTo.Term.IsVariable)
                {
                    // union our PVL with the others.
                    // warning here can be ignores since only variable values are allowed to be in the candidate.
                    PVL.Union(candiate.im.Value.Term.ProhibitedValues, (PVL)candiate.Item2.Variable.ProhibitedValues.Clone());
                } else
                {
                    // set mappsTo of the other to mappsTo of ours
                    this.mappings[candiate.im.Key] = (AtomParam)candiate.Item2.MapsTo.Clone();
                }

                inductorCopy.Remove(candiate.Item2.Variable);
            }

            // add all other mappings that do not need to be induced.
            foreach (var missingMapping in inductorCopy.Mappings)
            {
                this.TryAdd(missingMapping.Variable, missingMapping.MapsTo);
                // mappings[missingMapping.Variable.Value] = missingMapping.MapsTo;
            }
        }

        /// <summary>
        /// Clones the current Substitution.
        /// </summary>
        /// <returns>Returns an object that clones the current substitution.</returns>
        public Substitution Clone()
        {
            return new Substitution(this.Mappings.Select(m => new Mapping((Term)m.Variable.Clone(), (AtomParam)m.MapsTo.Clone())));
        }

        /// <summary>
        /// Contracts the current substitution.
        /// </summary>
        public void Contract()
        {
            while (true)
            {
                var inductions = this.mappings
                    .Where(im => im.Value.IsTerm) // we only care about the term values since they can be variables
                    .Where(im => im.Value.Term.IsVariable) // only the values that are variables
                    .Where(im => this.mappings.ContainsKey(im.Value.Term.Value))
                    .Where(im => !(im.Value.Term != null && im.Value.Term.Value == im.Key)); // only the ones where the key is not the same as the value

                if (!inductions.Any()) // no more changes need to be made
                {
                    return;
                }

                var induction = inductions.First();

                this.mappings[induction.Key] = this.mappings[induction.Value.Term.Value];
                this.mappings.Remove(induction.Value.Term.Value);
            }
        }

        /// <summary>
        /// Removes the variable from the current substitution.
        /// </summary>
        /// <param name="variable">The variable that will be removed.</param>
        public void Remove(Term variable)
        {
            this.mappings.Remove(variable.Value);
        }

        /// <summary>
        /// Clears the current substitution.
        /// </summary>
        public void Clear()
        {
            this.mappings.Clear();
        }

        /// <summary>
        /// Looks if the current substitution contains a mapping for the given term.
        /// </summary>
        /// <param name="mapping">The mapping that should be looked for.</param>
        /// <returns>A value indicating whether the mapping is contained in the current substitution.</returns>
        public bool ContainsMappingFor(Term mapping)
        {
            return this.mappings.ContainsKey(mapping.Value);
        }

        /// <summary>
        /// Gets the mapping of the given term.
        /// </summary>
        /// <param name="mapping">The mapping that should be looked for.</param>
        /// <returns>The AtomParam that fits to the given mapping.</returns>
        public AtomParam GetMappingOf(Term mapping)
        {
            return this.mappings[mapping.Value];
        }

        /// <summary>
        /// Intersects the current substitution with the given set of variables.
        /// </summary>
        /// <param name="variables">The variables that should be intersected with.</param>
        public void Intersect(HashSet<string> variables)
        {
            var newMappings = new Dictionary<string, AtomParam>();

            foreach (var mapping in this.mappings)
            {
                if (variables.Contains(mapping.Key))
                {
                    newMappings.Add(mapping.Key, mapping.Value);
                }
            }

            this.mappings = newMappings;
        }

        private AtomParam Apply(AtomParam param)
        {
            if (param.Term != null)
            {
                // no need to do anything. only looking for variables
                if (!param.Term.IsVariable)
                {
                    return param;
                }

                // no mapping for this variable available.
                if (!this.mappings.ContainsKey(param.Term.Value))
                {
                    return param;
                }

                var setting = this.mappings[param.Term.Value];

                if (setting.Term != null && setting.Term.IsVariable)
                {
                    PVL.Union(setting.Term.ProhibitedValues, param.Term.ProhibitedValues);

                    return setting;
                }

                return setting;
            }

            if (param.Literal != null)
            {
                this.Apply(param.Literal.Atom);
            }

            return param;
        }

        private void Apply(Atom atom)
        {
            for (int i = 0; i < atom.ParamList.Count(); i++)
            {
                var param = atom.ParamList[i];
                atom.ParamList[i] = this.Apply(param);
            }
        }

        private void Apply(BodyPart part)
        {
            if (part.Literal != null)
            {
                this.Apply(part.Literal.Atom);
            }

            if (part.Child != null)
            {
                this.Apply(part.Child);
            }

            if (part.Operation != null)
            {
                this.ApplyOperation(part.Operation);
            }
        }

        private void ApplyOperation(Operation copy)
        {
            if (copy.OutputtingVariable != null)
            {
                if (this.mappings.ContainsKey(copy.OutputtingVariable.Value))
                {
                    var mapped = this.mappings[copy.OutputtingVariable.Value];

                    if (mapped.Term == null || !mapped.Term.IsVariable)
                    {
                        throw new InvalidOperationException($"Unable to map outputting variable of an is operation to a non variable value. {copy} | {copy.OutputtingVariable} -> {mapped}");
                    }

                    copy.OutputtingVariable = mapped.Term;
                }
            }

            if (copy.Variable.Term != null)
            {
                if (this.mappings.ContainsKey(copy.Variable.Term.Value))
                {
                    var mapped = this.mappings[copy.Variable.Term.Value];

                    if (mapped.Term != null && mapped.Term.IsVariable)
                    {
                        PVL.Union(mapped.Term.ProhibitedValues, copy.Variable.Term.ProhibitedValues);
                    }

                    copy.Variable = mapped;

                    // make sure the operation variable is always a literal.
                    // if (copy.Variable.Term != null)
                    // {
                    //     copy.Variable = new AtomParam(new Literal(new Atom(copy.Variable.Term.Value), false, false));
                    // }
                }
            }

            if (copy.Variable.Literal != null)
            {
                this.Apply(copy.Variable.Literal.Atom);
            }
        }
    }
}
