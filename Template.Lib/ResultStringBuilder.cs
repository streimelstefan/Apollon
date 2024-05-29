namespace Apollon.Lib
{
    using System.Text;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Resolution;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// The class that is responsible for creating a string representation of a ResolutionResult.
    /// </summary>
    public class ResultStringBuilder
    {
        /// <summary>
        /// Creates a string representation of a ResolutionResult.
        /// </summary>
        /// <param name="res">The ResolutionsResult what a string should be created for.</param>
        /// <returns>A string representing the Result.</returns>
        public string CreateResultString(ResolutionResult res)
        {
            StringBuilder result = new StringBuilder();

            if (!res.Success)
            {
                result.AppendLine("No solution found.");
                return result.ToString();
            }

            result.AppendLine("Solution found:\n");
            result.AppendLine("{");

            foreach (var literal in res.CHS.Literals)
            {
                result.Append("  ");
                result.Append(this.BuildLiteralString(literal));
                result.AppendLine();
            }

            result.AppendLine("}");

            var subRes = this.BuildSubstitutionString(res.Substitution);

            if (subRes.Length > 0)
            {
                result.AppendLine();
                result.Append(subRes);
            }

            return result.ToString();
        }

        private StringBuilder BuildSubstitutionString(Substitution sub)
        {
            var res = new StringBuilder();

            foreach (var mapping in sub.Mappings)
            {
                if (mapping.MapsTo.Term != null && mapping.MapsTo.Term.IsVariable)
                {
                    var variable = mapping.MapsTo.Term;

                    if (variable.ProhibitedValues.GetValues().Count() == 0)
                    {
                        // skip entries that link to normal variables.
                        continue;
                    }

                    foreach (var pvl in variable.ProhibitedValues.GetValues())
                    {
                        res.Append(mapping.Variable.Value);
                        res.Append(" != ");
                        res.Append(this.BuildAtomParamString(pvl, new StringBuilder()));
                        res.AppendLine();
                    }
                }
                else
                {
                    res.Append(mapping.Variable.Value);
                    res.Append(" = ");
                    res.Append(this.BuildAtomParamString(mapping.MapsTo, new StringBuilder()));
                    res.AppendLine();
                }
            }

            return res;
        }

        private StringBuilder BuildLiteralString(Literal literal)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder pvl = new StringBuilder();

            if (literal.IsNAF)
            {
                result.Append("not ");
            }

            if (literal.IsNegative)
            {
                result.Append("-");
            }

            result.Append(this.BuildAtomString(literal.Atom, pvl));

            if (pvl.Length > 0)
            {
                result.Append(" (");
                result.Append(pvl);
                result.Append(")");
            }

            return result;
        }

        private StringBuilder BuildAtomString(Atom atom, StringBuilder pvlStringBuilder)
        {
            StringBuilder res = new StringBuilder();

            res.Append(atom.Name);

            if (atom.ParamList.Count() == 0)
            {
                return res;
            }

            res.Append('(');
            for (int i = 0; i < atom.ParamList.Count() - 1; i++)
            {
                var param = atom.ParamList[i];
                res.Append(this.BuildAtomParamString(param, pvlStringBuilder));
                res.Append(", ");
            }

            res.Append(this.BuildAtomParamString(atom.ParamList.Last(), pvlStringBuilder));

            res.Append(")");
            return res;
        }

        private StringBuilder BuildAtomParamString(AtomParam atomParam, StringBuilder pvlStringBuilder)
        {
            var res = new StringBuilder();
            if (atomParam.Term != null)
            {
                res.Append(atomParam.Term.Value);

                foreach (var pvl in atomParam.Term.ProhibitedValues.GetValues())
                {
                    if (pvlStringBuilder.Length > 0)
                    {
                        pvlStringBuilder.Append(", ");
                    }

                    pvlStringBuilder.Append(atomParam.Term.Value);
                    pvlStringBuilder.Append(" != ");
                    pvlStringBuilder.Append(this.BuildAtomParamString(pvl, pvlStringBuilder));
                }
            }

            if (atomParam.Literal != null)
            {
                res.Append(this.BuildLiteralString(atomParam.Literal));
            }

            return res;
        }
    }
}
