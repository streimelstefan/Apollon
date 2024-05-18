﻿using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification.Substitutioners;
using System.Runtime.ExceptionServices;

namespace Apollon.Lib.Unification.Substitutioners
{
    public class Substitution : ISubstitution
    {
        private Dictionary<string, AtomParam> mappings = new Dictionary<string, AtomParam>();


        public Substitution() { }

        public Substitution(IEnumerable<Mapping> mappings)
        {
            this.mappings = new Dictionary<string, AtomParam>(mappings.Select(m => new KeyValuePair<string, AtomParam>(m.Variable.Value, m.MapsTo)));
        }

        public IEnumerable<Mapping> Mappings
        {
            get
            {
                return mappings.Select(m => { 
                    if (m.Value.Term != null && m.Value.Term.IsVariable)
                    {
                        return new Mapping(new Term(m.Key, (PVL)m.Value.Term.ProhibitedValues.Clone()), m.Value);
                    } else
                    {
                        return new Mapping(new Term(m.Key), m.Value);
                    }
                });

            }
        }

        public void Add(Term variable, AtomParam term)
        {
            if (mappings.ContainsKey(variable.Value))
            {
                if (!mappings[variable.Value].Equals(term))
                {
                    throw new InvalidOperationException("Cannot add new substitution under the existing name");
                }
            }
            mappings[variable.Value] = term;
        }

        public Statement Apply(Statement statement)
        {
            var copy = (Statement)statement.Clone();

            if (copy.Head != null)
            {
                Apply(copy.Head.Atom);
            }

            foreach (var part in copy.Body)
            {
                Apply(part);
            }

            return copy;
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
                if (!mappings.ContainsKey(param.Term.Value))
                {
                    return param;
                }

                var setting = mappings[param.Term.Value];

                if (setting.Term != null && setting.Term.IsVariable)
                {
                    PVL.Union(setting.Term.ProhibitedValues, param.Term.ProhibitedValues);

                    return setting;
                }

                return setting;
            }

            if (param.Literal != null)
            {
                Apply(param.Literal.Atom);
            }

            return param;
        }

        private void Apply(Atom atom)
        {
            for (int i = 0; i < atom.ParamList.Count(); i++)
            {
                var param = atom.ParamList[i];
                atom.ParamList[i] = Apply(param);
            }
        }

        private void Apply(BodyPart part)
        {
            if (part.Literal != null)
            {
                Apply(part.Literal.Atom);
            }
            if (part.Child != null)
            {
                Apply(part.Child);
            }
            if (part.Operation != null)
            {
                ApplyOperation(part.Operation);
            }
        }

        private void ApplyOperation(Operation copy)
        {

            if (copy.Variable.Term != null)
            {
                if (mappings.ContainsKey(copy.Variable.Term.Value))
                {
                    var mapped = mappings[copy.Variable.Term.Value];

                    if (mapped.Term != null && mapped.Term.IsVariable)
                    {
                        PVL.Union(mapped.Term.ProhibitedValues, copy.Variable.Term.ProhibitedValues);
                    }

                    copy.Variable = mapped;


                    // make sure the operation variable is always a literal.
                    if (copy.Variable.Term != null)
                    {
                        copy.Variable = new AtomParam(new Literal(new Atom(copy.Variable.Term.Value), false, false));
                    }
                }
            }
            if (copy.Variable.Literal != null)
            {
                Apply(copy.Variable.Literal.Atom);
            }
        }


        public override string ToString()
        {
            return $"{{ {string.Join(", ", mappings.Select(kv => $"{kv.Key} -> {kv.Value}"))} }}";
        }

        public Literal Apply(Literal literal)
        {
            var copy = (Literal)literal.Clone();
            Apply(copy.Atom);

            return copy;
        }

        public Operation Apply(Operation operation)
        {
            var copy = (Operation)operation.Clone();

            ApplyOperation(copy);

            return copy;
        }

        public void BackPropagate(ISubstitution inductor)
        {
            bool changesMade = false;

            var candidates = mappings
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
                    mappings[candiate.im.Key] = (AtomParam)candiate.Item2.MapsTo.Clone();
                }
            }
        }
    }
}
