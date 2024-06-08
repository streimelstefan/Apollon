//-----------------------------------------------------------------------
// <copyright file="Substitution.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Unification.Substitutioners
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Resolution.CallStackAndCHS;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Rules.Operations;

    /// <summary>
    /// The substitution class contains all the variable mapping. It remembers what variable should be replaced with what values.
    /// And provides methods to apply these substitutions to parts of a apollon program.
    /// </summary>
    public class Substitution
    {
        private Dictionary<string, AtomParam> mappings = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Substitution"/> class.
        /// </summary>
        public Substitution()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Substitution"/> class.
        /// </summary>
        /// <param name="mappings">The mappings with wich this substitution should be initialized.</param>
        public Substitution(IEnumerable<Mapping> mappings)
        {
            this.mappings = new Dictionary<string, AtomParam>(mappings.Select(m => new KeyValuePair<string, AtomParam>(m.Variable.Value, m.MapsTo)));
        }

        /// <summary>
        /// Gets the mappings.
        /// </summary>
        public IEnumerable<Mapping> Mappings => this.mappings.Select(m =>
                                                             {
                                                                 return m.Value.Term != null && m.Value.Term.IsVariable
                                                                     ? new Mapping(new Term(m.Key, (PVL)m.Value.Term.ProhibitedValues.Clone()), m.Value)
                                                                     : new Mapping(new Term(m.Key), m.Value);
                                                             });

        /// <summary>
        /// Gets the bound mappings.
        /// </summary>
        public IEnumerable<Mapping> BoundMappings => this.Mappings.Where(m => m.MapsTo.IsLiteral || (m.MapsTo.Term != null && !m.MapsTo.Term.IsVariable));

        /// <summary>
        /// Adds a new substitution to the stubstitutions.
        /// </summary>
        /// <param name="variable">The variable that should be substituted.</param>
        /// <param name="term">The value the variable should be substituted with.</param>
        /// <exception cref="InvalidOperationException">Is thrown when the substitution can not be added under the existing name.</exception>
        public void Add(Term variable, AtomParam term)
        {
            if (this.mappings.ContainsKey(variable.Value))
            {
                if (this.mappings[variable.Value].Term != null && this.mappings[variable.Value].Term!.IsVariable &&
                    term.Term != null && term.Term.IsVariable)
                {
                    // if the value to add and the mapsTo of the mapping here are both variables Unionize their PVLs.
                    // and abort.
                    PVL.Union(this.mappings[variable.Value].Term!.ProhibitedValues, term.Term.ProhibitedValues);
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
        /// Adds a new substitution.
        /// </summary>
        /// <param name="variable">The variable to substitute.</param>
        /// <param name="term">The value to substitute the variable with.</param>
        public void Add(Term variable, Term term)
        {
            this.Add(variable, new AtomParam(term));
        }

        /// <summary>
        /// Focefully adds a subtitution.
        /// </summary>
        /// <param name="variable">The vairable to substitute.</param>
        /// <param name="term">The value to substitute the variable with.</param>
        public void ForceAdd(Term variable, AtomParam term)
        {
            this.mappings[variable.Value] = term;
        }

        /// <summary>
        /// Focefully adds a subtitution.
        /// </summary>
        /// <param name="variable">The vairable to substitute.</param>
        /// <param name="term">The value to substitute the variable with.</param>
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
        /// Applies all saved substitutions to the given statement.
        /// </summary>
        /// <param name="statement">The statement to apply the substitution on.</param>
        /// <returns>A clone substitution of the statement.</returns>
        public Statement Apply(Statement statement)
        {
            Statement copy = (Statement)statement.Clone();

            if (copy.Head != null)
            {
                this.Apply(copy.Head.Atom);
            }

            foreach (BodyPart part in copy.Body)
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
            foreach (KeyValuePair<string, AtomParam> mapping in this.mappings)
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
        /// Applies all saved substitutions on the given literal.
        /// </summary>
        /// <param name="literal">The literal to apply the stubstitutions on.</param>
        /// <returns>The clone literal that was substituted.</returns>
        public Literal Apply(Literal literal)
        {
            Literal copy = (Literal)literal.Clone();
            this.Apply(copy.Atom);

            return copy;
        }

        /// <summary>
        /// Applies all substitutions an the literals of the given CHS.
        /// </summary>
        /// <param name="chs">The CHS to apply the substitutions to.</param>
        public void ApplyInline(CHS chs)
        {
            foreach (var literal in chs.Literals)
            {
                this.ApplyInline(literal);
            }
        }

        public void ApplyInline(Stack<Literal> callStack)
        {
            foreach (var literal in callStack)
            {
                this.ApplyInline(literal);
            }
        }

        public void ApplyInline(Dictionary<string, List<Literal>> things)
        {
            foreach (var key in things.Keys.ToArray())
            {
                if (this.mappings.ContainsKey(key))
                {
                    if (this.mappings[key].IsLiteral || !this.mappings[key].Term.IsVariable)
                        continue;

                    var newList = new List<Literal>();
                    foreach (var literal in things[key])
                    {
                        newList.Add(this.Apply(literal));
                    }

                    things.Remove(key);
                    things.Add(this.mappings[key].Term.Value, newList);
                }
            }
        }

        /// <summary>
        /// Applies all the given substitutions on the given object. The object will be altered!.
        /// </summary>
        /// <param name="literal">The literal to apply the sutbstitutions to.</param>
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
            Operation copy = (Operation)operation.Clone();

            this.ApplyOperation(copy);

            return copy;
        }

        /// <summary>
        /// Performce a backwards propagation using the given substitution. It contects a variable subtstute of this substitutions
        /// with a substitution variable key of the other one.
        ///
        /// sub1: X -> Y
        /// sub2: Y -> b
        ///
        /// sub1.BackPropagate(sub2).
        /// sub1: X -> b.
        /// </summary>
        /// <param name="inductor">The other substitution to use for the back propagation.</param>
        public void BackPropagate(Substitution inductor)
        {
            Substitution inductorCopy = inductor.Clone();

            IEnumerable<(KeyValuePair<string, AtomParam> Im, Mapping)> candidates = this.mappings
                .Where(im => im.Value.IsTerm) // we only care about the term values since they can be variables
                .Where(im => im.Value.Term!.IsVariable) // only the values that are variables
                .Where(im => inductor.Mappings.Where(m => m.Variable.Value == im.Value.Term!.Value).Any()) // only those that look like this ... -> Y | Y -> ...
                .Select(im => (im, inductor.Mappings.Where(m => m.Variable.Value == im.Value.Term!.Value).First()));

            foreach ((KeyValuePair<string, AtomParam> Im, Mapping) candiate in candidates)
            {
                if (candiate.Item2.MapsTo.Term != null && candiate.Item2.MapsTo.Term.IsVariable)
                {
                    // union our PVL with the others.
                    // warning here can be ignores since only variable values are allowed to be in the candidate.
                    PVL.Union(candiate.Im.Value.Term!.ProhibitedValues, (PVL)candiate.Item2.Variable.ProhibitedValues.Clone());
                }
                else
                {
                    // set mappsTo of the other to mappsTo of ours
                    this.mappings[candiate.Im.Key] = (AtomParam)candiate.Item2.MapsTo.Clone();
                }

                inductorCopy.Remove(candiate.Item2.Variable);
            }

            // add all other mappings that do not need to be induced.
            foreach (Mapping missingMapping in inductorCopy.Mappings)
            {
                _ = this.TryAdd(missingMapping.Variable, missingMapping.MapsTo);

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
                IEnumerable<KeyValuePair<string, AtomParam>> inductions = this.mappings
                    .Where(im => im.Value.IsTerm) // we only care about the term values since they can be variables
                    .Where(im => im.Value.Term!.IsVariable) // only the values that are variables
                    .Where(im => this.mappings.ContainsKey(im.Value.Term!.Value))
                    .Where(im => !(im.Value.Term != null && im.Value.Term.Value == im.Key)); // only the ones where the key is not the same as the value

                if (!inductions.Any()) // no more changes need to be made
                {
                    return;
                }

                KeyValuePair<string, AtomParam> induction = inductions.First();

                this.mappings[induction.Key] = this.mappings[induction.Value.Term!.Value];
                _ = this.mappings.Remove(induction.Value.Term.Value);
            }
        }

        /// <summary>
        /// Removes the variable from the current substitution.
        /// </summary>
        /// <param name="variable">The variable that will be removed.</param>
        public void Remove(Term variable)
        {
            _ = this.mappings.Remove(variable.Value);
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
            Dictionary<string, AtomParam> newMappings = new();

            foreach (KeyValuePair<string, AtomParam> mapping in this.mappings)
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

                AtomParam setting = this.mappings[param.Term.Value];

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
                AtomParam param = atom.ParamList[i];
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
            if (part.ForAll != null)
            {
                if (this.mappings.ContainsKey(part.ForAll.Value))
                {
                    var mapping = this.mappings[part.ForAll.Value];
                    if (mapping.Term != null)
                    {
                        part.ForAll = mapping.Term;
                    }
                }
            }
        }

        public void RemovePVLFor(Term variable)
        {
            if (!this.mappings.ContainsKey(variable.Value)) return; 

            var mapped = this.mappings[variable.Value];

            if (mapped.Term != null)
            {
                mapped.Term.ProhibitedValues.Clear();
            }
        }

        public void RemovePVLForMapped(Term variable)
        {
            foreach (var value in this.mappings.Values)
            {
                if (value.Term != null && value.Term.IsVariable && value.Term.Value == variable.Value)
                {
                    value.Term.ProhibitedValues.Clear();
                }
            }
        }

        private void ApplyOperation(Operation copy)
        {
            if (copy.OutputtingVariable != null)
            {
                if (this.mappings.ContainsKey(copy.OutputtingVariable.Value))
                {
                    AtomParam mapped = this.mappings[copy.OutputtingVariable.Value];

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
                    AtomParam mapped = this.mappings[copy.Variable.Term.Value];

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

            if (copy.Condition.Term != null)
            {
                if (this.mappings.ContainsKey(copy.Condition.Term.Value))
                {
                    AtomParam mapped = this.mappings[copy.Condition.Term.Value];

                    if (mapped.Term != null && mapped.Term.IsVariable)
                    {
                        PVL.Union(mapped.Term.ProhibitedValues, copy.Condition.Term.ProhibitedValues);
                    }

                    copy.Condition = mapped;
                }
            }

            if (copy.Variable.Literal != null)
            {
                this.Apply(copy.Variable.Literal.Atom);
            }
        }
    }
}
